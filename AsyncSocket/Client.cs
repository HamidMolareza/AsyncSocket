namespace AsyncSocket {
    public class Client {
        public static async Task StartClientAsync () {
            // The port number for the remote device.  
            const int port = 443; //Default HTTPS port. (80 for HTTP)

            // Establish the remote endpoint for the socket.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry ("Input your host name or address");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEndPoint = new IPEndPoint (ipAddress, port);

            // Create a TCP/IP socket.  
            var client = new Socket (ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            await client.ConnectAsync (remoteEndPoint).ConfigureAwait (false);

            // Send test data to the remote device. 
            var bytesSent = await client.SendAsync ("This is a test<EOF>").ConfigureAwait (false);
            Console.WriteLine ("Sent {0} bytes to server.", bytesSent);

            // Receive the response from the remote device.  
            var response = await client.ReceiveAsync ().ConfigureAwait (false);

            // Write the response to the console.  
            Console.WriteLine ("Response received : {0}", response);

            // Release the socket.  
            client.Shutdown (SocketShutdown.Both);
            client.Close ();
        }
    }
}