using System;
using System.Threading.Tasks;

namespace Console_AsyncSocket {
    public class Program {
        public static void Main (string[] args) {
            try {
                InnerMain ();
            } catch (System.Exception e) {
                System.Console.WriteLine ("\n\n");
                System.Console.WriteLine (e);
            }
        }

        private static void InnerMain () {
            using (var myListener = new MyListener (numOfThreads: 10)) {
                System.Console.WriteLine ($"{MyListener.IpAddress}:{myListener.Port}");

                System.Console.Write ("Starting...");
                myListener.Start ();
                System.Console.WriteLine ("\nStart.");

                Handle (myListener).Wait ();

                System.Console.Write ("Press any key to stop...");
                Console.ReadKey ();
                System.Console.Write ("\nStopping...");
                myListener.Stop ();
                System.Console.WriteLine ("\nStop.");
            }

            System.Console.Write ("Press any key to exit...");
            Console.ReadKey ();
        }

        private static async Task Handle (MyListener listener) {
            //TODO: Manual testing...
            throw new NotImplementedException ();
        }
    }
}