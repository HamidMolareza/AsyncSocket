using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// This template use base socket syntax to change Pattern. (like Send, Receive, and so on)
// Convert to Task-based Asynchronous Pattern. (TAP)

namespace AsynchronousClient
{
    public static class Template2
    {
        public static void Main()
        {
            try
            {
                StartClientAsync().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task StartClientAsync()
        {
            // The port number for the remote device.  
            const int port = 443; //Default HTTPS port. (80 for HTTP)

            // Establish the remote endpoint for the socket.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry("Input your host name or address.");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            var client = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            var isConnect = await client.ConnectAsync(remoteEndPoint).ConfigureAwait(false);
            if (!isConnect)
            {
                Console.WriteLine("Can not connect.");
                return;
            }

            // Send test data to the remote device. 
            var bytesSent = await client.SendAsync("This is a test<EOF>").ConfigureAwait(false);
            Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            // Receive the response from the remote device.  
            var response = await client.ReceiveAsync().ConfigureAwait(false);

            // Write the response to the console.  
            Console.WriteLine("Response received : {0}", response);

            // Release the socket.  
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private static Task<bool> ConnectAsync(this Socket client, IPEndPoint remoteEndPoint)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (remoteEndPoint == null) throw new ArgumentNullException(nameof(remoteEndPoint));

            return Task.FromResult(Connect(client, remoteEndPoint));
        }

        private static bool Connect(this Socket client, EndPoint remoteEndPoint)
        {
            if (client == null || remoteEndPoint == null)
                return false;

            try
            {
                client.Connect(remoteEndPoint);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<string> ReceiveAsync(this Socket client, int waitForFirstDelaySeconds = 3)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Timeout for wait to receive and prepare data.
            for (var i = 0; i < waitForFirstDelaySeconds; i++)
            {
                if (client.Available > 0)
                    break;
                await Task.Delay(1000).ConfigureAwait(false);
            }

            // return null If data is not available.
            if (client.Available < 1)
                return null;

            // Size of receive buffer.
            const int bufferSize = 1024;
            var buffer = new byte[bufferSize];

            // Get data
            var response = new StringBuilder(bufferSize);
            do
            {
                var size = Math.Min(bufferSize, client.Available);
                await Task.FromResult(client.Receive(buffer)).ConfigureAwait(false);
                response.Append(Encoding.ASCII.GetString(buffer, 0, size));

            } while (client.Available > 0);

            // Return result.
            return response.ToString();
        }

        private static async Task<int> SendAsync(this Socket client, string data)
        {
            var byteData = Encoding.ASCII.GetBytes(data);
            return await SendAsync(client, byteData, 0, byteData.Length, 0).ConfigureAwait(false);
        }

        private static Task<int> SendAsync(this Socket client, byte[] buffer, int offset,
            int size, SocketFlags socketFlags)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return Task.FromResult(client.Send(buffer, offset, size, socketFlags));
        }
    }
}
