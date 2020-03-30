using System;
using System.Net.Sockets;
using System.Text;
using AsyncSocket;

namespace Console_AsyncSocket {
    public class MyListener : Listener {
        public MyListener (int port = 11000, int numOfThreads = 25, int receiveTimeout = 5000) : base (port, numOfThreads, receiveTimeout) { }
        protected override void MainHandlerAsync (Socket handler, string data) {
            BaseSocket.SendAsync (handler, data, Encoding.UTF8).Wait ();
        }

        protected override void TimeoutExceptionHandler (Socket handler, TimeoutException timeoutException) {
            Console.WriteLine ("============================================");
            Console.WriteLine ($"TimeoutExceptionHandler: {timeoutException}");
            Console.WriteLine ("============================================");

        }

        protected override void UnExpectedExceptionHandler (Socket handler, Exception exception) {
            Console.WriteLine ("============================================");
            Console.WriteLine ($"UnknownExceptionHandler: {exception}");
            Console.WriteLine ("============================================");

        }
    }
}