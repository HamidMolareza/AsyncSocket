using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AsyncSocket {
    public abstract class Listener {
        public bool IsServerActive { get; private set; }

        public int Port { get; }

        public int NumOfThreads { get; }

        public IPEndPoint LocalEndPoint { get; }

        public static readonly IPHostEntry IpHostInfo = Dns.GetHostEntry (Dns.GetHostName ());

        public static readonly IPAddress IpAddress = IpHostInfo.AddressList.Length > 3 ?
            IpHostInfo.AddressList[2] : IpHostInfo.AddressList[1];

        private Socket listener { get; set; }

        protected Listener (int port = 11000, int numOfThreads = 25, int backlog = 100) {
            Port = port;
            NumOfThreads = numOfThreads;
            LocalEndPoint = new IPEndPoint (IpAddress, Port);
            CreateTcpIpListener (backlog);
        }

        private void CreateTcpIpListener (int backlog = 100) {
            // Create a TCP/IP socket.  
            listener = new Socket (IpAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            listener.Bind (LocalEndPoint);
            listener.Listen (backlog);
        }

        public void Start () {
            if (IsServerActive)
                return;

            IsServerActive = true;
            for (var i = 0; i < NumOfThreads; i++)
                _ = Task.Run (StartListeningAsync);
        }

        private async Task StartListeningAsync () {
            Socket socket = null;
            while (IsServerActive) {
                try {
                    socket = await Accept (listener);
                    var data = await GetDataAsync (socket);
                    await HandlerAsync (socket, data);
                } catch (TimeoutException t) {
                    //Use Dependency Inversion/Injection for remove simpleLog.
                    SimpleLog.Append (t);
                } catch (Exception e) {
                    SimpleLog.Append (e);
                } finally {
                    if (socket != null) {
                        socket.Shutdown (SocketShutdown.Both);
                        socket.Close ();
                    }
                }
            }
        }

        public abstract Task HandlerAsync (Socket handler, string data);

        public async Task StopAsync () {
            if (!IsServerActive)
                return;

            IsServerActive = false;
            await ForceCloseServerAsync ();
        }

        private async Task ForceCloseServerAsync () {
            IsServerActive = false;
            var counter = 0;
            const int limit = 2;

            do {
                Socket sender = null;
                try {
                    sender = new Socket (IpAddress.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);
                    // Connect the socket to the remote endpoint.
                    await ConnectAsync (sender, LocalEndPoint);
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