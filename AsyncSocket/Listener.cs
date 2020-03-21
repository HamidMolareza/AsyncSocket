using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

//TODO: Property validation

namespace AsyncSocket {
    public abstract class Listener {
        /// <summary>
        /// True if the listener is active, otherwise false.
        /// </summary>
        public bool IsListenerActive { get; private set; }

        private int _port;
        private const int DefaultPort = 11000;

        /// <summary>
        /// The port number for the remote device. (Http: 80, HTTPS: 443)
        /// To set property, The listener must be stop.
        /// </summary>
        public int Port {
            get => _port;
            set => BindToLocalEndPoint (value);
        }

        /// <summary>
        /// The maximum length of the pending connections queue.
        /// </summary>
        private const int Backlog = 100;

        private int _numOfThreads;
        private const int DefaultNumOfThreads = 25;

        /// <summary>
        /// To set property, The listener must be stop.
        /// </summary>
        public int NumOfThreads {
            get => _numOfThreads;
            set {
                ListenerMustBeStop ();
                _numOfThreads = value;
            }
        }

        private double _receiveTimeout;
        private const double DefaultReceiveTimeout = 5000;

        /// <summary>
        /// To set property, The listener must be stop.
        /// </summary>
        public double ReceiveTimeout {
            get => _receiveTimeout;
            set {
                ListenerMustBeStop ();
                _receiveTimeout = value;
            }
        }

        /// <summary>
        /// A network endpoint as an IP address and a port number.
        /// </summary>
        public IPEndPoint LocalEndPoint { get; private set; }

        /// <summary>
        /// An IPHostEntry instance that contains address information about the host specified in address.
        /// </summary>
        public static readonly IPHostEntry IpHostInfo = Dns.GetHostEntry (Dns.GetHostName ());

        public static readonly IPAddress IpAddress = IpHostInfo.AddressList.Length > 3 ?
            IpHostInfo.AddressList[2] : IpHostInfo.AddressList[1];

        /// <summary>
        /// TCP/IP socket
        /// </summary>
        public readonly Socket ListenerSocket = new Socket(IpAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        protected Listener (int port = DefaultPort, int numOfThreads = DefaultNumOfThreads, double receiveTimeout = DefaultReceiveTimeout) {
            BindToLocalEndPoint (port);
            NumOfThreads = numOfThreads;
            ReceiveTimeout = receiveTimeout;
        }

        /// <summary>
        /// Throw exception if listener is active.
        /// </summary>
        /// <exception cref="System.Exception">Throw exception if listener is active.</exception>
        private void ListenerMustBeStop () {
            if (IsListenerActive)
                throw new Exception ("The listener is active. Please stop the listener first.");
        }

        private void BindToLocalEndPoint (int port) {
            ListenerMustBeStop ();

            LocalEndPoint = new IPEndPoint (IpAddress, port);

            // Bind the socket to the local endpoint.  
            ListenerSocket.Bind (LocalEndPoint);

            //listen for incoming connections
            ListenerSocket.Listen (Backlog);

            _port = port;
        }

        /// <summary>
        /// Start the listener/
        /// </summary>
        public void Start () {
            if (IsListenerActive)
                return;

            IsListenerActive = true;
            for (var i = 0; i < NumOfThreads; i++)
                Task.Run (StartListeningAsync);

            //Delay to ensure all threads is run. 
            Task.Delay(NumOfThreads).Wait();
        }

        /// <summary>
        /// The method that handle requests.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data">Request data.</param>
        public abstract Task MainHandlerAsync (Socket handler, string data);

        /// <summary>
        /// When the request takes too long, this method will be called.
        /// </summary>
        /// <param name="handler">The socket can be null.</param>
        /// <param name="timeoutException">Exception details.</param>
        public abstract Task TimeoutExceptionHandler (Socket handler, TimeoutException timeoutException);

        /// <summary>
        /// When an unknown error occurs, this method will be called.
        /// </summary>
        /// <param name="handler">The socket can be null.</param>
        /// <param name="exception">Exception details.</param>
        public abstract Task UnknownExceptionHandler (Socket handler, Exception exception);

        private async Task StartListeningAsync () {
            Socket socket = null;

            while (IsListenerActive) {
                try {
                    socket = await BaseSocket.AcceptAsync (ListenerSocket);
                    var data = await BaseSocket.ReceiveAsync (socket, Encoding.UTF32, ReceiveTimeout);
                    await MainHandlerAsync (socket, data);
                } catch (TimeoutException te) {
                    await TimeoutExceptionHandler (socket, te);
                } catch (Exception e) {
                    await UnknownExceptionHandler (socket, e);
                } finally {
                    if (socket != null) {
                        socket.Shutdown (SocketShutdown.Both);
                        socket.Close ();
                    }
                }
            }
        }

        public async Task StopAsync () {
            if (!IsListenerActive)
                return;

            IsListenerActive = false;
            await ForceCloseServerAsync ();
        }

        private async Task ForceCloseServerAsync () {
            IsListenerActive = false;
            var counter = 0;
            const int limit = 2;

            do {
                Socket sender = null;
                try {
                    //TODO: Use client class
                    sender = new Socket (IpAddress.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);
                    // Connect the socket to the remote endpoint.
                    await BaseSocket.ConnectAsync (sender, LocalEndPoint);
                    counter = 0;
                } catch (Exception) {
                    counter++;
                } finally {
                    if (sender?.Connected == true) {
                        // Release the socket.  
                        sender.Shutdown (SocketShutdown.Both);
                        sender.Close ();
                    }
                }
            } while (counter < limit);
        }
    }
}