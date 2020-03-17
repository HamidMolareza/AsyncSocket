using System;

namespace AsynchronousClient {
    public static class Template1 {
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

        private static Task ConnectAsync (this Socket client, IPEndPoint remoteEndPoint) {
            if (client == null) throw new ArgumentNullException (nameof (client));
            if (remoteEndPoint == null) throw new ArgumentNullException (nameof (remoteEndPoint));

            return Task.Factory.FromAsync (client.BeginConnect,
                client.EndConnect, remoteEndPoint, null);
        }

        private static async Task<string> ReceiveAsync (this Socket client) {
            if (client == null) throw new ArgumentNullException (nameof (client));

            // Size of receive buffer.
            const int bufferSize = 1024;
            var buffer = new byte[bufferSize];

            var response = new StringBuilder (bufferSize);

            var taskCompletion = new TaskCompletionSource<int> ();
            do {
                var size = Math.Min (bufferSize, client.Available);
                client.BeginReceive (buffer, 0, size, SocketFlags.None, iar => {
                    try {
                        taskCompletion.TrySetResult (client.EndReceive (iar));
                    } catch (OperationCanceledException) {
                        taskCompletion.TrySetCanceled ();
                    } catch (Exception exc) {
                        taskCompletion.TrySetException (exc);
                    }
                }, null);

                await taskCompletion.Task;
                response.Append (Encoding.ASCII.GetString (buffer, 0, size));

            } while (client.Available > 0);

            return response.ToString ();
        }

        private static async Task<int> SendAsync (this Socket client, String data) {
            var byteData = Encoding.ASCII.GetBytes (data);
            return await SendAsync (client, byteData, 0, byteData.Length, 0).ConfigureAwait (false);
        }

        public static Task<int> SendAsync (this Socket client, byte[] buffer, int offset,
            int size, SocketFlags socketFlags) {
            if (client == null) throw new ArgumentNullException (nameof (client));

            var tcs = new TaskCompletionSource<int> ();
            client.BeginSend (buffer, offset, size, socketFlags, iar => {
                try {
                    tcs.TrySetResult (client.EndSend (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);

            return tcs.Task;
        }
    }
}