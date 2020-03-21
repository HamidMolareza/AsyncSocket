using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsyncSocket {
    public static class BaseSocket {
        #region ConnectAsync

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="remoteEndPoint">A network endpoint as an IP address and a port number.</param>
        public static Task ConnectAsync (Socket socket, IPEndPoint remoteEndPoint) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, remoteEndPoint, null);
        }

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ipAddress">The IPAddress of the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        public static Task ConnectAsync (Socket socket, IPAddress ipAddress, int port) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, ipAddress, port, null);
        }

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="remoteEp">An EndPoint that represents the remote device.</param>
        public static Task ConnectAsync (Socket socket, EndPoint remoteEp) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, remoteEp, null);
        }

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ipAddresses">At least one IPAddress, designating the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        public static Task ConnectAsync (Socket socket, IPAddress[] ipAddresses, int port) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, ipAddresses, port, null);
        }

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="host">The name of the remote host.</param>
        /// <param name="port">The port number of the remote host.s</param>
        public static Task ConnectAsync (Socket socket, string host, int port) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, host, port, null);
        }
        #endregion

        #region SendAsync

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data">The data to send.</param>
        /// <param name="encoding">The data encoding type.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns></returns>
        public static async Task<int> SendAsync (Socket socket, String data, Encoding encoding, SocketFlags socketFlags) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            var byteData = encoding.GetBytes (data);
            return await SendAsync (socket, byteData, 0, byteData.Length, socketFlags);
        }

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">An array of type Byte that contains the data to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">An array of type Byte that contains the data to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="errorCode">A SocketError object that stores the socket error.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers">An array of type Byte that contains the data to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers">An array of type Byte that contains the data to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="errorCode">A SocketError object that stores the socket error.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends a file asynchronously to a connected Socket object.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="fileName">A string that contains the path and name of the file to send. This parameter can be null.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static Task SendFileAsync (Socket socket, string fileName) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginSendFile, socket.EndSendFile, fileName, null);
        }

        /// <summary>
        /// Sends a file asynchronously to a connected Socket object.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="fileName">A string that contains the path and name of the file to send. This parameter can be null.</param>
        /// <param name="preBuffer">A Byte array that contains data to be sent before the file is sent. This parameter can be null.</param>
        /// <param name="postBuffer">A Byte array that contains data to be sent after the file is sent. This parameter can be null.</param>
        /// <param name="flags">A bitwise combination of TransmitFileOptions values.</param>
        /// <returns>True if successful, otherwise false</returns>
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

        /// <summary>
        /// Sends data asynchronously to a specific remote host.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">An array of type Byte that contains the data to send.</param>
        /// <param name="offset">The zero-based position in buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="remoteEp">An EndPoint that represents the remote device.</param>
        /// <returns>The number of bytes sent.</returns>
        public static Task<int> SendToAsync (Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEp) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            var tcs = new TaskCompletionSource<int> ();
            socket.BeginSendTo (buffer, offset, size, socketFlags, remoteEp, iar => {
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

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="encoding">Data encoding</param>
        /// <param name="timeout"></param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>The string of bytes received.</returns>
        public static async Task<string> ReceiveAsync (Socket socket, Encoding encoding, double timeout = 5000, SocketFlags socketFlags = SocketFlags.None) {
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
                var firstLength = data.Length;
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

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="offset">The location in buffer to store the received data.</param>
        /// <param name="size">The number of bytes to receive.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>The number of bytes received.</returns>
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

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="offset">The location in buffer to store the received data.</param>
        /// <param name="size">The number of bytes to receive.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="errorCode">A SocketError object that stores the socket error.</param>
        /// <returns>The number of bytes received.</returns>
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

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>The number of bytes received.</returns>
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

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="errorCode">A SocketError object that stores the socket error.</param>
        /// <returns>The number of bytes received.</returns>
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

        /// <summary>
        /// Asynchronous operation to accept an incoming connection attempt.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>A Socket for a newly created connection.</returns>
        public static Task<Socket> AcceptAsync (Socket socket) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, null);
        }

        /// <summary>
        /// Asynchronous operation to accept an incoming connection attempt.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="receiveSize">The number of bytes to accept from the sender.</param>
        /// <returns>A Socket for a newly created connection.</returns>
        public static Task<Socket> AcceptAsync (Socket socket, int receiveSize) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, receiveSize, null);
        }

        /// <summary>
        /// Asynchronous operation to accept an incoming connection attempt.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="acceptSocket">The accepted Socket object. This value may be null.</param>
        /// <param name="receiveSize">The maximum number of bytes to receive.</param>
        /// <returns>A Socket for a newly created connection.</returns>
        public static Task<Socket> AcceptAsync (Socket socket, Socket acceptSocket, int receiveSize) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, acceptSocket, receiveSize, null);
        }
        #endregion

        #region DisconnectAsync

        /// <summary>
        /// An asynchronous request to disconnect from a remote endpoint.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="reuseSocket">true if this socket can be reused after the connection is closed; otherwise, false.</param>
        public static Task DisconnectAsync (Socket socket, bool reuseSocket) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginDisconnect, socket.EndDisconnect, reuseSocket, null);
        }
        #endregion
    }
}