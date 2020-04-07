using System;
using System.Threading.Tasks;
using AsyncSocket;

namespace Console_AsyncSocket {
    public class Program {
        public static void Main (string[] args) {
            try {
                InnerMain ();
            } catch (Exception e) {
                Console.WriteLine ("\n\n");
                Console.WriteLine (e);
            }
        }

        private static void InnerMain () {
            using (var myListener = new MyListener (numOfThreads: 10)) {
                Console.WriteLine ($"{Listener.IpAddress}:{myListener.Port}");

                Console.Write ("Starting...");
                myListener.Start ();
                Console.WriteLine ("\nStart.");

                Handle (myListener).Wait ();

                Console.Write ("Press any key to stop...");
                Console.ReadKey ();
                Console.Write ("\nStopping...");
                myListener.Stop ();
                Console.WriteLine ("\nStop.");
            }

            Console.Write ("Press any key to exit...");
            Console.ReadKey ();
        }

        private static async Task Handle (MyListener listener) {
            //TODO: Manual testing...
            throw new NotImplementedException ();
        }
    }
}