using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncSocket.Exceptions;

//TODO: Are you using the properties correctly?
//TODO: Check throw exceptions, Are they necessary?

namespace AsyncSocket {
    public abstract class Listener : IDisposable {

        #region Properties

        private CancellationTokenSource cancellationSource = new CancellationTokenSource ();

        /// <summary>
        /// True if the listener is active, otherwise false.
        /// </summary>
        public bool IsListenerStart { get; private set; }

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
                ListenerMustBeStop ();
                if (value < MinimumThreads)
                    throw new ArgumentOutOfRangeException ($"The value must equal or more than {MinimumThreads}.");

                _numOfThreads = value;
            }
        }

        #endregion

        #region ReceiveTimeout

        private int _receiveTimeout;
        public const int DefaultReceiveTimeout = BaseSocket.DefaultReceiveTimeout;
        public const int MinimumReceiveTimeout = BaseSocket.MinimumReceiveTimeout;

        /// <summary>
        /// To set property, The listener must be stop.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when value is out of range.</exception>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        public int ReceiveTimeout {
            get => _receiveTimeout;
            set {
                ListenerMustBeStop ();
                if (value < MinimumReceiveTimeout)
                    throw new ArgumentOutOfRangeException ($"The value must equal or more than {MinimumReceiveTimeout}.");

                _receiveTimeout = value;
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

            if (IsListenerStart)
                return;

            IsListenerStart = true;
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
            ListenerIsNotDisposed ();

            if (!IsListenerStart)
                return;

            IsListenerStart = false;
            cancellationSource.Cancel ();

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
        protected virtual void TimeoutExceptionHandler (Socket handler, TimeoutException timeoutException) { /*Ignore*/ }

        /// <summary>
        /// When an unknown error occurs, this method will be called.
        /// </summary>
        /// <param name="handler">The socket can be null.</param>
        /// <param name="exception">Exception details.</param>
        protected abstract void UnExpectedExceptionHandler (Socket handler, Exception exception);

        #endregion

        #region Private Methods

        //TODO: Check and correct names like ListenerMustBeStop, IsListenerStart and so on. are they rational?
        /// <summary>
        /// Throw exception if listener is active.
        /// </summary>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        private void ListenerMustBeStop () {
            ListenerIsNotDisposed ();
            if (IsListenerStart)
                throw new ListenerIsActiveException ("The listener is active. Please stop the listener first.");
        }

        private void ListenerIsNotDisposed () {
            if (isDisposed)
                throw new ObjectDisposedException (nameof (Listener), "This object is disposed.");
        }

        /// <summary>
        /// </summary>
        /// <param name="port"></param>
        /// <exception cref="AsyncSocket.Exceptions.ListenerIsActiveException">Throw exception if listener is active.</exception>
        private void BindToLocalEndPoint (int port) {
            ListenerMustBeStop ();

            LocalEndPoint = new IPEndPoint (IpAddress, port);

            // Bind the socket to the local endpoint.  
            ListenerSocket.Bind (LocalEndPoint);

            //listen for incoming connections
            ListenerSocket.Listen (Backlog);

            _port = port;
        }

        //TODO: When add "async" word to methods name? when they return Task<> or has async key?
        private async Task StartListeningAsync () {
            //TODO: Check throw exceptions, Are they necessary?
            ListenerIsNotDisposed ();

            Socket localSocket;
            while (IsListenerStart && !cancellationSource.IsCancellationRequested) {
                try {
                    localSocket = null;
                    try {
                        //AcceptAsync
                        //TODO: Refactor - Add utility?
                        var acceptTask = BaseSocket.AcceptAsync (ListenerSocket);
                        acceptTask.Wait (cancellationSource.Token);
                        localSocket = acceptTask.Result;

                        //ReceiveAsync
                        //TODO: Refactor - Add utility?
                        var receiveTask = BaseSocket.ReceiveAsync (localSocket, Encoding.UTF32, ReceiveTimeout);
                        receiveTask.Wait (cancellationSource.Token);
                        var data = receiveTask.Result;

                        //TODO: I want use cancellation Token for every methods in this method. How?
                        MainHandlerAsync (localSocket, data);
                    } catch (OperationCanceledException) {
                        //TODO: break or return?
                        break;
                    } catch (ObjectDisposedException) {
                        //TODO: break or return?
                        break;
                    } catch (TimeoutException te) {
                        //TODO: Is it necessary? if yes, where?
                        TimeoutExceptionHandler (localSocket, te);
                    } catch (Exception e) {
                        //TODO: Is it necessary? if yes, where?
                        UnExpectedExceptionHandler (localSocket, e);
                    } finally {
                        if (localSocket != null) {
                            localSocket.Shutdown (SocketShutdown.Both);
                            localSocket.Close ();
                        }
                    }
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

                //TODO: Find best way....
                //TODO: When call stop, we must ensure the listener completely stop.
                Task.Delay (100).Wait (); //Ensure all threads are stop.
            }

            // free unmanaged resources (unmanaged objects) and set large fields to null.
            cancellationSource.Dispose ();
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