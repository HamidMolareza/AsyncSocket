using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocket {
    public abstract class Listener {
        public bool IsListenerActive { get; private set; }

        private int _port;

        /// <summary>
        /// The port number for the remote device. (Http: 80, HTTPS: 443)
        /// </summary>
        public int Port {
            get => _port;
            set => InitProperties (value, NumOfThreads, Backlog);
        }

        private int _backlog;
        public int Backlog {
            get => _backlog;
            set => InitProperties (Port, NumOfThreads, value);
        }

        private int _numOfThreads;

        public int NumOfThreads {
            get => _numOfThreads;
            set => InitProperties (Port, value, Backlog);
        }

        public IPEndPoint LocalEndPoint { get; private set; }

        public static readonly IPHostEntry IpHostInfo = Dns.GetHostEntry (Dns.GetHostName ());

        public static readonly IPAddress IpAddress = IpHostInfo.AddressList.Length > 3 ?
            IpHostInfo.AddressList[2] : IpHostInfo.AddressList[1];

        public Socket ListenerSocket { get; private set; }

        protected Listener (int port = 11000, int numOfThreads = 25, int backlog = 100) {
            InitProperties (port, numOfThreads, backlog);
        }

        private void InitProperties (int port, int numOfThreads, int backlog) {
            if (IsListenerActive)
                throw new Exception ("The listener is active. Please stop the listener first.");

            _port = port;
            _numOfThreads = numOfThreads;
            _backlog = backlog;
            LocalEndPoint = new IPEndPoint (IpAddress, Port);
            CreateTcpIpListener (backlog);
        }

        private void CreateTcpIpListener (int backlog = 100) {
            // Create a TCP/IP socket.  
            ListenerSocket = new Socket (IpAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            ListenerSocket.Bind (LocalEndPoint);
            ListenerSocket.Listen (backlog);
        }

        public void Start () {
            if (IsListenerActive)
                return;

            IsListenerActive = true;
            for (var i = 0; i < NumOfThreads; i++)
                Task.Run (StartListeningAsync);
        }

        public abstract Task HandlerAsync (Socket handler, string data);

        private async Task StartListeningAsync () {
            Socket socket = null;

            while (IsListenerActive) {
                try {
                    socket = await BaseSocket.AcceptAsync (ListenerSocket);
                    var data = await BaseSocket.ReceiveAsync (socket, Encoding.UTF32);
                    await HandlerAsync (socket, data);
                } catch (TimeoutException) {
                    //TODO: Use Dependency Inversion/Injection for logging.
                } catch (Exception) {
                    //TODO: Use Dependency Inversion/Injection for logging.
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