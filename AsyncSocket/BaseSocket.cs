﻿using System;
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
        public static async Task<int> SendAsync (Socket socket, String data) {
            var byteData = Encoding.ASCII.GetBytes (data);
            return await SendAsync (socket, byteData, 0, byteData.Length, 0);
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
        #endregion

        #region ReceiveAsync
        private static async Task<string> ReceiveAsync (Socket handler, double timeout = 5000) {
            if (handler == null)
                throw new ArgumentNullException (nameof (handler));
            if (timeout < 1)
                throw new ArgumentException (nameof (timeout) + "Can not less than 1ms.");

            string data = null;

            //For timeout....
            var stopWatch = new Stopwatch ();
            stopWatch.Start ();

            while (true) {
                var firstLength = data?.Length ?? 0;
                var bytes = new byte[3072];
                var bytesRec = await ReceiveAsync (handler, bytes, firstLength, 3072);
                data += Encoding.UTF8.GetString (bytes, 0, bytesRec);

                if (handler.Available == 0) //Receive all bytes....
                    return data;

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

        private static Task<int> ReceiveAsync (Socket socket, byte[] buffer, int offset, int size) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));
            if (buffer == null)
                throw new ArgumentNullException (nameof (buffer));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginReceive (buffer, offset, size, SocketFlags.None, iar => {
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
        private static Task<Socket> AcceptAsync (Socket socket) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, null);
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