using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocket {
    public class Client {

        private string _hostEntry;

        /// <summary>
        /// Host name or address
        /// </summary>
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

        /// <summary>
        /// An IPHostEntry instance that contains address information about the host specified in address.
        /// </summary>
        public IPHostEntry IpHost { get; private set; }

        public IPAddress IpAddress { get; private set; }

        /// <summary>
        /// A network endpoint as an IP address and a port number.
        /// </summary>
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

        /// <summary>
        /// Send data to the remote device, then receive the response from the remote device.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="encoding">Message encoding</param>
        /// <param name="receiveTimeout"></param>
        /// <returns>Response from the remote device.</returns>
        public async Task<string> SendAsync (string message, Encoding encoding, double receiveTimeout = 5000) {
            // Connect to the remote endpoint.  
            await BaseSocket.ConnectAsync (ClientSocket, RemoteEndPoint);

            try {
                // Send data to the remote device. 
                await BaseSocket.SendAsync (ClientSocket, message, encoding, SocketFlags.None);

                // Receive the response from the remote device. 
                return await BaseSocket.ReceiveAsync (ClientSocket, encoding, receiveTimeout);
            } finally {
                // Release the socket.  
                ClientSocket.Shutdown (SocketShutdown.Both);
                ClientSocket.Close ();
            }
        }
    }
}