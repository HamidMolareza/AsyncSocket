using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AsyncSocket;

namespace Client {
    public class Program {
        public static void Main (string[] args) {
            try {
                Handle ().Wait ();
            } catch (Exception e) {
                Console.WriteLine ("\n");
                Console.WriteLine (e);
            }
        }

        private static async Task Handle () {
            using var client = new Socket (SocketType.Stream, ProtocolType.Tcp);

            while (true) {
                try {
                    await BaseSocket.ConnectAsync (client, Listener.IpAddress, 11000);

                    var sentBytes = await BaseSocket.SendAsync (client, "Message", Encoding.UTF8);
                    System.Console.WriteLine ($"SentBytes:{sentBytes}");

                    var receiveMessage = await BaseSocket.ReceiveAsync (client, Encoding.UTF8);
                    System.Console.WriteLine ($"receiveMessage:{receiveMessage}");
                } finally {
                    client.Shutdown (SocketShutdown.Both);
                    client.Close ();
                }
                System.Console.WriteLine ();
            }
        }
    }
}