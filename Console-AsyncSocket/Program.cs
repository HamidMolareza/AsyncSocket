using System;

namespace Console_AsyncSocket {
    class Program {
        static void Main (string[] args) {
            using (var myListener = new MyListener (numOfThreads: 25)) {
                System.Console.WriteLine ($"{MyListener.IpAddress}:{myListener.Port}");

                System.Console.Write ("Starting...");
                myListener.Start ();
                System.Console.WriteLine ("\nStart.");

                System.Console.Write ("Stopping...");
                myListener.Stop ();
                System.Console.WriteLine ("\nStop.");
            }

            System.Console.Write ("Press any key to exit...");
            Console.ReadKey ();
        }
    }
}