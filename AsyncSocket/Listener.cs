using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncSocket.Exceptions;

//TODO: Unit Test

namespace AsyncSocket {
    public abstract class Listener : IDisposable {

        #region Properties

        /// <summary>
        /// Cancellation for stop threads.
        /// </summary>
        private CancellationTokenSource cancellationThreads;

        #region IsStart
        private bool _isStart;

        /// <summary>
        /// True if the listener is active, otherwise false.
        /// </summary>
        public bool IsStart {
            get => _isStart && !cancellationThreads.IsCancellationRequested;
            private set => _isStart = value;

        }

        #endregion

        #region Port

        private int _port;
        public const int DefaultPort = 11000;

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
        public const int DefaultNumOfThreads = 25;
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

        #region ReceiveTimeout

        private int _receiveTimeout;

        /// <summary>
        /// Default receive timeout base milliseconds.
        /// </summary>
        public const int DefaultReceiveTimeout = 5000; //ms

        /// <summary>
        /// Receive timeout base milliseconds. | To set property, the listener must be stop.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when value is out of range.</exception>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        public int ReceiveTimeout {
            get => _receiveTimeout;
            set {
                ListenerIsStopAndNotDisposed ();

                if (value < BaseSocket.MinimumTimeout)
                    throw new ArgumentOutOfRangeException ($"The value must equal or more than {BaseSocket.MinimumTimeout}.");

                _receiveTimeout = value;
            }
        }

        #endregion

        #region AcceptTimeout

        private int _acceptTimeout;

        /// <summary>
        /// Default accept timeout base milliseconds.
        /// </summary>
        public const int DefaultAcceptTimeout = 2000; //ms

        /// <summary>
        /// Accept timeout base milliseconds. | To set property, the listener must be stop.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when value is out of range.</exception>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        public int AcceptTimeout {
            get => _acceptTimeout;
            set {
                ListenerIsStopAndNotDisposed ();

                if (value < BaseSocket.MinimumTimeout)
                    throw new ArgumentOutOfRangeException ($"The value must equal or more than {BaseSocket.MinimumTimeout}.");

                _acceptTimeout = value;
            }
        }

        #endregion

        /// <summary>
        /// A network endpoint as an IP address and a port number.
        /// </summary>
        public IPEndPoint LocalEndPoint { get; private set; }

        /// <summary>
        /// An IPHostEntry instance that contains address information about the host specified in address.
        /// </summary>
        public static readonly IPHostEntry IpHostInfo = Dns.GetHostEntry (Dns.GetHostName ());

        //TODO: Check on windows and linux
        public static readonly IPAddress IpAddress = IpHostInfo.AddressList[0];

        /// <summary>
        /// TCP/IP socket
        /// </summary>
        public readonly Socket ListenerSocket = new Socket (IpAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        #endregion

        #region Ctor

        public Listener (int port = DefaultPort, int numOfThreads = DefaultNumOfThreads, int receiveTimeout = DefaultReceiveTimeout) {
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

            cancellationThreads = new CancellationTokenSource ();
            IsStart = true;

            for (var i = 0; i < NumOfThreads; i++)
                Task.Run (StartListeningAsync);

            //TODO: Big numbers problem; Find best way to ensure all threads are run.
            //Delay to ensure all threads is run. 
            Task.Delay (NumOfThreads).Wait ();
        }

        /// <summary>
        /// Stop the listener.
        /// </summary>
        public void Stop () {
            if (!IsStart)
                return;

            IsStart = false;
            cancellationThreads.Cancel ();

            //TODO: Add task delay for ensure all threads are stop? or another good way.
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The method that handle requests.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data">Request data.</param>
        protected abstract void MainHandlerAsync (Socket handler, string data);

        /// <summary>
        /// When the request takes too long, this method will be called.
        /// </summary>
        /// <param name="handler">The socket can be null.</param>
        /// <param name="timeoutException">Exception details.</param>
        protected virtual void TimeoutExceptionHandler (Socket handler, TimeoutException timeoutException) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// When an unknown error occurs, this method will be called.
        /// </summary>
        /// <param name="handler">The socket can be null.</param>
        /// <param name="exception">Exception details.</param>
        protected virtual void UnExpectedExceptionHandler (Socket handler, Exception exception) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
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
            if (isDisposed)
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

        //TODO: When add "async" word to methods name? when they return Task<> or has async key?
        //TODO: Refactor (Try-Catch)
        private async Task StartListeningAsync () {
            Socket localSocket;
            while (IsStart) {
                try {
                    localSocket = null;
                    try {
                        //AcceptAsync
                        //TODO: Refactor - Add utility?
                        var acceptTask = BaseSocket.AcceptAsyncByTimeout (ListenerSocket, AcceptTimeout);
                        acceptTask.Wait (cancellationThreads.Token);
                        localSocket = acceptTask.Result;

                        //ReceiveAsync
                        //TODO: Refactor - Add utility?
                        //TODO: Encoding.UTF?
                        var receiveTask = BaseSocket.ReceiveAsync (localSocket, Encoding.UTF32, ReceiveTimeout);
                        receiveTask.Wait (cancellationThreads.Token);
                        var data = receiveTask.Result;

                        //TODO: I want use cancellation Token for every methods in this method. How?
                        MainHandlerAsync (localSocket, data);
                    } catch (OperationCanceledException) {
                        return;
                    } catch (ObjectDisposedException) {
                        return;
                    } catch (TimeoutException te) {
                        //TODO: Cancellation Token
                        TimeoutExceptionHandler (localSocket, te);
                    } catch (Exception e) {
                        //TODO: Cancellation Token
                        UnExpectedExceptionHandler (localSocket, e);
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
                } catch (System.Exception) {
                    //TODO: Ignore??
                }
            }
        }

        #endregion

        #region IDisposable Support
        private bool isDisposed = false;

        protected virtual void Dispose (bool disposing) {
            if (isDisposed) return;

            if (disposing) {
                // dispose managed state (managed objects).
                Stop (); //Stop the listener if is active.
            }

            // free unmanaged resources (unmanaged objects) and set large fields to null.
            cancellationThreads.Dispose ();
            ListenerSocket.Dispose ();

            //override a finalizer below
            isDisposed = true;

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