using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocket {
    public class BaseSocket {
        #region ConnectAsync
        public static Task ConnectAsync (Socket socket, IPEndPoint remoteEndPoint) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, remoteEndPoint, null);
        }

        public static Task ConnectAsync (Socket socket, IPAddress address, int port) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, address, port, null);
        }

        public static Task ConnectAsync (Socket socket, EndPoint remoteEP) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, remoteEP, null);
        }

        public static Task ConnectAsync (Socket socket, IPAddress[] addresses, int port) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, addresses, port, null);
        }

        public static Task ConnectAsync (Socket socket, string host, int port) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, host, port, null);
        }
        #endregion

        #region SendAsync
        public static async Task<int> SendAsync (Socket socket, String data, Encoding encoding, SocketFlags socketFlags) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            var byteData = encoding.GetBytes (data);
            return await SendAsync (socket, byteData, 0, byteData.Length, socketFlags);
        }

        public static Task<int> SendAsync (Socket socket, byte[] buffer, int offset,
            int size, SocketFlags socketFlags) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginSend (buffer, offset, size, socketFlags, iar => {
                try {
                    tcs.TrySetResult (socket.EndSend (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);

            return tcs.Task;
        }

        public static Task<int> SendAsync (Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginSend (buffer, offset, size, socketFlags, out errorCode, iar => {
                try {
                    tcs.TrySetResult (socket.EndSend (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);

            return tcs.Task;
        }

        public static Task<int> SendAsync (Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginSend (buffers, socketFlags, iar => {
                try {
                    tcs.TrySetResult (socket.EndSend (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);

            return tcs.Task;
        }

        public static Task<int> SendAsync (Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginSend (buffers, socketFlags, out errorCode, iar => {
                try {
                    tcs.TrySetResult (socket.EndSend (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);

            return tcs.Task;
        }
        #endregion

        #region SendFileAsync
        public static Task SendFileAsync (Socket socket, string fileName) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginSendFile, socket.EndSendFile, fileName, null);
        }

        public static Task<bool> SendFileAsync (Socket socket, string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<bool> ();
            socket.BeginSendFile (fileName, preBuffer, postBuffer, flags, iar => {
                try {
                    socket.EndSendFile (iar);
                    tcs.TrySetResult (true);
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);

            return tcs.Task;
        }

        #endregion

        #region SendToAsync
        public static Task<int> SendToAsync (Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginSendTo (buffer, offset, size, socketFlags, remoteEP, iar => {
                try {
                    tcs.TrySetResult (socket.EndSendTo (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);

            return tcs.Task;
        }
        #endregion

        #region ReceiveAsync
        public static async Task<string> ReceiveAsync (Socket socket, Encoding encoding, SocketFlags socketFlags = SocketFlags.None, double timeout = 5000) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));
            if (timeout < 1)
                throw new ArgumentException (nameof (timeout) + "Can not less than 1ms.");

            const int defaultCapacity = 1024; //1MB
            var data = new StringBuilder (defaultCapacity);

            //For timeout....
            var stopWatch = new Stopwatch ();
            stopWatch.Start ();

            while (true) {
                var firstLength = data?.Length ?? 0;
                var bytes = new byte[defaultCapacity];
                var bytesRec = await ReceiveAsync (socket, bytes, firstLength, defaultCapacity, socketFlags);
                data.Append (encoding.GetString (bytes, 0, bytesRec));

                if (socket.Available == 0) //Receive all bytes....
                    return data.ToString ();

                switch (data.Length) {
                    case 0 when stopWatch.Elapsed.TotalMilliseconds > timeout:
                        throw new TimeoutException ();
                    case 0:
                        continue;
                }

                if (data.Length - firstLength > 0) stopWatch.Restart ();
                if (data.Length - firstLength <= 0 && stopWatch.Elapsed.TotalMilliseconds > timeout)
                    throw new TimeoutException ();
            }
        }

        public static Task<int> ReceiveAsync (Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginReceive (buffer, offset, size, socketFlags, iar => {
                try {
                    tcs.TrySetResult (socket.EndReceive (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);
            return tcs.Task;
        }

        public static Task<int> ReceiveAsync (Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginReceive (buffer, offset, size, socketFlags, out errorCode, iar => {
                try {
                    tcs.TrySetResult (socket.EndReceive (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);
            return tcs.Task;
        }

        public static Task<int> ReceiveAsync (Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginReceive (buffers, socketFlags, iar => {
                try {
                    tcs.TrySetResult (socket.EndReceive (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);
            return tcs.Task;
        }

        public static Task<int> ReceiveAsync (Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginReceive (buffers, socketFlags, out errorCode, iar => {
                try {
                    tcs.TrySetResult (socket.EndReceive (iar));
                } catch (OperationCanceledException) {
                    tcs.TrySetCanceled ();
                } catch (Exception exc) {
                    tcs.TrySetException (exc);
                }
            }, null);
            return tcs.Task;
        }

        #endregion

        #region AcceptAsync
        public static Task<Socket> AcceptAsync (Socket socket) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, null);
        }

        public static Task<Socket> AcceptAsync (Socket socket, int receiveSize) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, receiveSize, null);
        }

        public static Task<Socket> AcceptAsync (Socket socket, Socket acceptSocket, int receiveSize) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, acceptSocket, receiveSize, null);
        }
        #endregion

        #region DisconnectAsync
        public static Task DisconnectAsync (Socket socket, bool reuseSocket) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginDisconnect, socket.EndDisconnect, reuseSocket, null);
        }
        #endregion
    }
}