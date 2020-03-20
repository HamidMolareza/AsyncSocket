using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocket {
    public class Client {
        private string _hostEntry;
        public string HostEntry {
            get => _hostEntry;
            set => SetHostEntryAndPort (value, Port);
        }

        private int _port;

        /// <summary>
        /// The port number for the remote device. (Http: 80, HTTPS: 443)
        /// </summary>
        public int Port {
            get => _port;
            set => SetHostEntryAndPort (HostEntry, value);
        }

        public IPHostEntry IpHost { get; private set; }

        public IPAddress IpAddress { get; private set; }

        public IPEndPoint RemoteEndPoint { get; private set; }

        public Socket ClientSocket { get; private set; }

        public Client (string hostNameOrAddress, int port) {
            SetHostEntryAndPort (hostNameOrAddress, port);
        }

        private void SetHostEntryAndPort (string hostNameOrAddress, int port) {
            // Establish the remote endpoint for the socket.  
            IpHost = Dns.GetHostEntry (hostNameOrAddress);

            IpAddress = IpHost.AddressList[0];
            RemoteEndPoint = new IPEndPoint (IpAddress, port);

            // Create a TCP/IP socket.  
            ClientSocket = new Socket (IpAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            _hostEntry = hostNameOrAddress;
            _port = port;
        }

        public async Task SendAsync (string message, Encoding encoding, double receiveTimeout = 5000) {
            // Connect to the remote endpoint.  
            await BaseSocket.ConnectAsync (ClientSocket, RemoteEndPoint);

            try {
                // Send data to the remote device. 
                await BaseSocket.SendAsync (ClientSocket, message, encoding, SocketFlags.None);

                // Receive the response from the remote device. 
                await BaseSocket.ReceiveAsync (ClientSocket, encoding, receiveTimeout);
            } finally {
                // Release the socket.  
                ClientSocket.Shutdown (SocketShutdown.Both);
                ClientSocket.Close ();
            }
        }
    }
}