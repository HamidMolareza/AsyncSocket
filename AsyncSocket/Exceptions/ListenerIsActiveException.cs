namespace AsyncSocket.Exceptions {
    using System;

    public class ListenerIsActiveException : Exception {
        public ListenerIsActiveException () { }

        public ListenerIsActiveException (string message) : base (message) { }

        public ListenerIsActiveException (string message, Exception inner) : base (message, inner) { }
    }
}