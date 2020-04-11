using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncSocket.Exceptions;
using AsyncSocket.Utility;

//TODO: Unit Test

namespace AsyncSocket {
    public abstract class Listener : IDisposable {

        #region Properties

        /// <summary>
        /// Cancellation for stop threads.
        /// </summary>
        private CancellationTokenSource _cancellationThreads;

        #region IsStart
        private bool _isStart;

        /// <summary>
        /// True if the listener is active, otherwise false.
        /// </summary>
        public bool IsStart {
            get => _isStart && !_cancellationThreads.IsCancellationRequested;
            private set => _isStart = value;

        }

        #endregion

        #region Port

        private int _port;
        protected const int DefaultPort = 11000;

        /// <summary>
        /// The port number for the remote device. (Http: 80, HTTPS: 443)
        /// To set property, The listener must be stop.
        /// </summary>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        public int Port {
            get => _port;
            set => BindToLocalEndPoint (value);
        }

        #endregion

        /// <summary>
        /// The maximum length of the pending connections queue.
        /// </summary>
        protected const int Backlog = 100;

        #region NumOfThreads

        private int _numOfThreads;
        protected const int DefaultNumOfThreads = 25;
        public const int MinimumThreads = 1;

        /// <summary>
        /// To set property, The listener must be stop.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when value is out of range.</exception>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        public int NumOfThreads {
            get => _numOfThreads;
            set {
                ListenerIsStopAndNotDisposed ();

                if (value < MinimumThreads)
                    throw new ArgumentOutOfRangeException ($"The value must equal or more than {MinimumThreads}.");

                _numOfThreads = value;
            }
        }

        #endregion

        private List<Task> _listenerTasks;

        #region ReceiveTimeout

        private int _receiveTimeout;

        /// <summary>
        /// Default receive timeout base milliseconds.
        /// </summary>
        protected const int DefaultReceiveTimeout = 5000; //ms

        /// <summary>
        /// Receive timeout base milliseconds. | To set property, the listener must be stop.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when value is out of range.</exception>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        public int ReceiveTimeout {
            get => _receiveTimeout;
            set {
                ListenerIsNotDisposed ();

                if (value < BaseSocket.MinimumTimeout)
                    throw new ArgumentOutOfRangeException ($"The value must equal or more than {BaseSocket.MinimumTimeout}.");

                _receiveTimeout = value;
            }
        }

        #endregion

        /// <summary>
        /// A network endpoint as an IP address and a port number.
        /// </summary>
        public IPEndPoint LocalEndPoint { get; private set; }

        public Encoding Encode { get; set; } = Encoding.UTF8;

        /// <summary>
        /// An IPHostEntry instance that contains address information about the host specified in address.
        /// </summary>
        public static readonly IPHostEntry IpHostInfo = Dns.GetHostEntry (Dns.GetHostName ());

        //TODO: Check on windows and linux
        public static readonly IPAddress IpAddress = OperatingSystemUtility.IsLinux ? IpHostInfo.AddressList[0] : IpHostInfo.AddressList[2];

        /// <summary>
        /// TCP/IP socket
        /// </summary>
        protected readonly Socket ListenerSocket = new Socket (IpAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        #endregion

        #region Ctor

        protected Listener (int port = DefaultPort, int numOfThreads = DefaultNumOfThreads, int receiveTimeout = DefaultReceiveTimeout) {
            BindToLocalEndPoint (port);
            NumOfThreads = numOfThreads;
            ReceiveTimeout = receiveTimeout;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the listener.
        /// </summary>
        public void Start () {
            ListenerIsNotDisposed ();

            if (IsStart)
                return;

            _cancellationThreads = new CancellationTokenSource ();
            IsStart = true;

            _listenerTasks = new List<Task> (NumOfThreads);
            for (var i = 0; i < NumOfThreads; i++) {
                _listenerTasks.Add (Task.Run (StartListening));
            }

            TaskUtility.EnsureAllTasksAreStable (_listenerTasks, false, true, false);
        }

        /// <summary>
        /// Stop the listener.
        /// </summary>
        public void Stop () {
            if (!IsStart)
                return;

            IsStart = false;
            _cancellationThreads.Cancel ();

            TaskUtility.EnsureAllTasksAreStable (_listenerTasks, true, false, false);
            _listenerTasks = null;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The method that handle requests.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data">Request data.</param>
        protected abstract void MainHandler (Socket handler, string data);

        /// <summary>
        /// When the request takes too long, this method will be called.
        /// </summary>
        /// <param name="handler">The socket can be null.</param>
        /// <param name="timeoutException">Exception details.</param>
        protected virtual void TimeoutExceptionHandler (Socket handler, TimeoutException timeoutException) {
            if (handler == null || !handler.Connected)
                return;

            //A simple message.
            var message = timeoutException == null ?
                "Time out!" : timeoutException.Message;

            //Send the response to the client.
            BaseSocket.SendAsync (handler, message, Encode).Wait ();
        }

        /// <summary>
        /// When an unknown error occurs, this method will be called.
        /// </summary>
        /// <param name="handler">The socket can be null.</param>
        /// <param name="exception">Exception details.</param>
        protected virtual void UnExpectedExceptionHandler (Socket handler, Exception exception) {
            if (handler == null || !handler.Connected)
                return;

            //A simple message.
            var message = exception == null ?
                "Un expected exception!" : exception.Message;

            //Send the response to the client.
            BaseSocket.SendAsync (handler, message, Encode).Wait ();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Throw exception if listener is start.
        /// </summary>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        private void ListenerMustBeStop () {
            if (IsStart)
                throw new ListenerIsActiveException ("The listener is active. Please stop the listener first.");
        }

        /// <summary>
        /// Throw exception if listener is disposed.
        /// </summary>
        private void ListenerIsNotDisposed () {
            if (_isDisposed)
                throw new ObjectDisposedException (nameof (Listener), "This object is disposed.");
        }

        /// <summary>
        /// Throw exception if the listener is disposed or start.
        /// </summary>
        private void ListenerIsStopAndNotDisposed () {
            ListenerIsNotDisposed ();
            ListenerMustBeStop ();
        }

        /// <summary>
        /// </summary>
        /// <param name="port"></param>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        private void BindToLocalEndPoint (int port) {
            ListenerIsStopAndNotDisposed ();

            LocalEndPoint = new IPEndPoint (IpAddress, port);

            // Bind the socket to the local endpoint.  
            ListenerSocket.Bind (LocalEndPoint);

            //listen for incoming connections
            ListenerSocket.Listen (Backlog);

            _port = port;
        }

        private void StartListening () {
            while (IsStart) {
                try {
                    Socket localSocket = null;
                    try {
                        //AcceptAsync
                        localSocket = TaskUtility.Wait (BaseSocket.AcceptAsync (ListenerSocket), _cancellationThreads.Token);

                        //ReceiveAsync
                        var data = TaskUtility.Wait (BaseSocket.ReceiveAsync (localSocket, Encode, ReceiveTimeout), _cancellationThreads.Token);

                        //MainHandler
                        TaskUtility.RunSynchronously (() => MainHandler (localSocket, data), _cancellationThreads.Token);
                    } catch (OperationCanceledException) {
                        return;
                    } catch (ObjectDisposedException) {
                        return;
                    } catch (TimeoutException te) {
                        //TimeoutExceptionHandler
                        TaskUtility.RunSynchronously (() => TimeoutExceptionHandler (localSocket, te), _cancellationThreads.Token);

                    } catch (Exception e) {
                        //UnExpectedExceptionHandler
                        TaskUtility.RunSynchronously (() => UnExpectedExceptionHandler (localSocket, e), _cancellationThreads.Token);

                    } finally {
                        if (localSocket != null) {
                            localSocket.Shutdown (SocketShutdown.Both);
                            localSocket.Close ();
                        }
                    }
                } catch (OperationCanceledException) {
                    return;
                } catch (ObjectDisposedException) {
                    return;
                } catch (Exception) {
                    // Ignore
                }
            }
        }

        #endregion

        #region IDisposable Support
        private bool _isDisposed;

        protected virtual void Dispose (bool disposing) {
            if (_isDisposed) return;

            if (disposing) {
                // dispose managed state (managed objects).
                Stop (); //Stop the listener if is active.
            }

            // free unmanaged resources (unmanaged objects) and set large fields to null.
            _cancellationThreads.Dispose ();
            ListenerSocket.Dispose ();

            //override a finalizer below
            _isDisposed = true;

        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~Listener () {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose (false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose () {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose (true);
            GC.SuppressFinalize (this);
        }
        #endregion
    }
}