using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AsyncSocket.Utility;

//TODO: Unit Test
//TODO: Try add timout to most methods.

namespace AsyncSocket {
    public static class BaseSocket {
        public const int MinimumTimeout = 1; //ms

        #region ConnectAsync

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="remoteEndPoint">A network endpoint as an IP address and a port number.</param>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task ConnectAsync (Socket socket, IPEndPoint remoteEndPoint) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, remoteEndPoint, null);
        }

        public static Task ConnectAsync (Socket socket, IPEndPoint remoteEndPoint, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ipAddress">The IPAddress of the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task ConnectAsync (Socket socket, IPAddress ipAddress, int port) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, ipAddress, port, null);
        }

        public static Task ConnectAsync (Socket socket, IPAddress ipAddress, int port, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="remoteEp">An EndPoint that represents the remote device.</param>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task ConnectAsync (Socket socket, EndPoint remoteEp) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, remoteEp, null);
        }

        public static Task ConnectAsync (Socket socket, EndPoint remoteEp, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ipAddresses">At least one IPAddress, designating the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task ConnectAsync (Socket socket, IPAddress[] ipAddresses, int port) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, ipAddresses, port, null);
        }

        public static Task ConnectAsync (Socket socket, IPAddress[] ipAddresses, int port, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="host">The name of the remote host.</param>
        /// <param name="port">The port number of the remote host.s</param>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task ConnectAsync (Socket socket, string host, int port) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginConnect,
                socket.EndConnect, host, port, null);
        }

        public static Task ConnectAsync (Socket socket, string host, int port, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
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
        /// <returns>The number of bytes sent</returns>
        /// <exception cref="ArgumentNullException">Throw if socket or data or encoding are null.</exception>
        public static async Task<int> SendAsync (Socket socket, string data, Encoding encoding, SocketFlags socketFlags = SocketFlags.None) {
            if (socket == null) throw new ArgumentNullException (nameof (socket));
            if (data == null) throw new ArgumentNullException (nameof (data));
            if (encoding == null) throw new ArgumentNullException (nameof (encoding));

            var byteData = encoding.GetBytes (data);
            return await SendAsync (socket, byteData, 0, byteData.Length, socketFlags);
        }

        public static async Task<int> SendAsync (Socket socket, string data, Encoding encoding, int timeout, SocketFlags socketFlags = SocketFlags.None) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer">An array of type Byte that contains the data to send.</param>
        /// <param name="offset">The zero-based position in the buffer parameter at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>The number of bytes sent</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<int> SendAsync (Socket socket, byte[] buffer, int offset,
            int size, SocketFlags socketFlags = SocketFlags.None) {
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

        public static Task<int> SendAsync (Socket socket, byte[] buffer, int offset,
            int size, int timeout, SocketFlags socketFlags = SocketFlags.None) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
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
        /// <returns>The number of bytes sent</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<int> SendAsync (Socket socket, byte[] buffer, int offset, int size, out SocketError errorCode, SocketFlags socketFlags = SocketFlags.None) {
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

        public static Task<int> SendAsync (Socket socket, byte[] buffer, int offset, int size, int timeout, out SocketError errorCode, SocketFlags socketFlags = SocketFlags.None) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers">An array of type Byte that contains the data to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>The number of bytes sent</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<int> SendAsync (Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags = SocketFlags.None) {
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

        public static Task<int> SendAsync (Socket socket, IList<ArraySegment<byte>> buffers, int timeout, SocketFlags socketFlags = SocketFlags.None) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers">An array of type Byte that contains the data to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="errorCode">A SocketError object that stores the socket error.</param>
        /// <returns>The number of bytes sent</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<int> SendAsync (Socket socket, IList<ArraySegment<byte>> buffers, out SocketError errorCode, SocketFlags socketFlags = SocketFlags.None) {
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

        public static Task<int> SendAsync (Socket socket, IList<ArraySegment<byte>> buffers, out SocketError errorCode, int timeout, SocketFlags socketFlags = SocketFlags.None) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        #endregion

        #region SendFileAsync

        /// <summary>
        /// Sends a file asynchronously to a connected Socket object.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="fileName">A string that contains the path and name of the file to send. This parameter can be null.</param>
        /// <returns>True if successful, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task SendFileAsync (Socket socket, string fileName) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginSendFile, socket.EndSendFile, fileName, null);
        }

        public static Task SendFileAsync (Socket socket, string fileName, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
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
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
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

        public static Task<bool> SendFileAsync (Socket socket, string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
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
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<int> SendToAsync (Socket socket, byte[] buffer, int offset, int size, EndPoint remoteEp, SocketFlags socketFlags = SocketFlags.None) {
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

        public static Task<int> SendToAsync (Socket socket, byte[] buffer, int offset, int size, EndPoint remoteEp, int timeout, SocketFlags socketFlags = SocketFlags.None) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        #endregion

        #region ReceiveAsync

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="encoding">Data encoding</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>The string of bytes received.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket or encoding are null.</exception>
        public static async Task<string> ReceiveAsync (Socket socket, Encoding encoding, SocketFlags socketFlags = SocketFlags.None) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));
            if (encoding == null) throw new ArgumentNullException (nameof (encoding));

            var receiveBytes = await ReceiveAsync (socket, socketFlags);

            return encoding.GetString (receiveBytes);
        }

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="encoding">Data encoding</param>
        /// <param name="timeout"></param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>The string of bytes received.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket or encoding are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throw if timeout is out of range.</exception>
        /// <exception cref="System.TimeoutException">Throw exception if the method processing takes too long.</exception>
        public static async Task<string> ReceiveAsync (Socket socket, Encoding encoding, int timeout, SocketFlags socketFlags = SocketFlags.None) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));
            if (encoding == null) throw new ArgumentNullException (nameof (encoding));
            if (!IsTimeoutValid (timeout)) throw new ArgumentOutOfRangeException (nameof (timeout));

            var receiveTask = ReceiveAsync (socket, encoding, socketFlags);
            return await TimeoutUtility.AwaitAsync (receiveTask, timeout);
        }

        private static bool IsTimeoutValid (int timeout) {
            return timeout >= MinimumTimeout;
        }

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>The string of bytes received.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static async Task<byte[]> ReceiveAsync (Socket socket, SocketFlags socketFlags = SocketFlags.None) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            //Init
            const int defaultCapacity = 1024; //1MB
            var data = new MemoryStream (defaultCapacity);

            while (socket.Available > 0) {
                //TODO: Is offset required?
                // var offset = data.Length;

                var buffer = new byte[defaultCapacity];

                //TODO: If offset is required, set offset to method.
                var bytesRec = await ReceiveAsync (socket, buffer, 0, defaultCapacity, socketFlags);

                await data.WriteAsync (buffer, 0, bytesRec);
            }

            return data.ToArray ();
        }

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="timeout"></param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>bytes received</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throw if timeout is out of range.</exception>
        /// <exception cref="System.TimeoutException">Throw exception if the method processing takes too long.</exception>
        public static async Task<byte[]> ReceiveAsync (Socket socket, int timeout, SocketFlags socketFlags = SocketFlags.None) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));
            if (!IsTimeoutValid (timeout)) throw new ArgumentOutOfRangeException (nameof (timeout));

            var receiveTask = ReceiveAsync (socket, socketFlags);
            return await TimeoutUtility.AwaitAsync (receiveTask, timeout);
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
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<int> ReceiveAsync (Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags = SocketFlags.None) {
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
        /// <param name="timeout"></param>
        /// <returns>The number of bytes received.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throw if timeout is out of range.</exception>
        /// <exception cref="System.TimeoutException">Throw exception if the method processing takes too long.</exception>
        public static async Task<int> ReceiveAsync (Socket socket, byte[] buffer, int offset, int size, int timeout, SocketFlags socketFlags = SocketFlags.None) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));
            if (!IsTimeoutValid (timeout)) throw new ArgumentOutOfRangeException (nameof (timeout));

            var receiveTask = ReceiveAsync (socket, buffer, offset, size, socketFlags);
            return await TimeoutUtility.AwaitAsync (receiveTask, timeout);
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
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<int> ReceiveAsync (Socket socket, byte[] buffer, int offset, int size, out SocketError errorCode, SocketFlags socketFlags = SocketFlags.None) {
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

        public static Task<int> ReceiveAsync (Socket socket, byte[] buffer, int offset, int size, int timeout, out SocketError errorCode, SocketFlags socketFlags = SocketFlags.None) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <returns>The number of bytes received.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<int> ReceiveAsync (Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags = SocketFlags.None) {
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
        /// <param name="timeout"></param>
        /// <returns>The number of bytes received.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throw if timeout is out of range.</exception>
        /// <exception cref="TimeoutException">Throw exception if the method processing takes too long.</exception>
        public static async Task<int> ReceiveAsync (Socket socket, IList<ArraySegment<byte>> buffers, int timeout, SocketFlags socketFlags = SocketFlags.None) {
            //Validation
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));
            if (!IsTimeoutValid (timeout)) throw new ArgumentOutOfRangeException (nameof (timeout));

            var receiveTask = ReceiveAsync (socket, buffers, socketFlags);
            return await TimeoutUtility.AwaitAsync (receiveTask, timeout);
        }

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers">An array of type Byte that is the storage location for the received data.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="errorCode">A SocketError object that stores the socket error.</param>
        /// <returns>The number of bytes received.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<int> ReceiveAsync (Socket socket, IList<ArraySegment<byte>> buffers, out SocketError errorCode, SocketFlags socketFlags = SocketFlags.None) {
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

        public static Task<int> ReceiveAsync (Socket socket, IList<ArraySegment<byte>> buffers, int timeout, out SocketError errorCode, SocketFlags socketFlags = SocketFlags.None) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        #endregion

        #region AcceptAsync

        /// <summary>
        /// Asynchronous operation to accept an incoming connection attempt.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>A Socket for a newly created connection.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<Socket> AcceptAsync (Socket socket) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, null);
        }

        //TODO: Name: ByTimeout?
        public static Task<Socket> AcceptAsyncByTimeout (Socket socket, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Asynchronous operation to accept an incoming connection attempt.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="receiveSize">The number of bytes to accept from the sender.</param>
        /// <returns>A Socket for a newly created connection.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<Socket> AcceptAsync (Socket socket, int receiveSize) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, receiveSize, null);
        }

        public static Task<Socket> AcceptAsyncByTimeout (Socket socket, int receiveSize, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Asynchronous operation to accept an incoming connection attempt.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="acceptSocket">The accepted Socket object. This value may be null.</param>
        /// <param name="receiveSize">The maximum number of bytes to receive.</param>
        /// <returns>A Socket for a newly created connection.</returns>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task<Socket> AcceptAsync (Socket socket, Socket acceptSocket, int receiveSize) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginAccept, socket.EndAccept, acceptSocket, receiveSize, null);
        }

        public static Task<Socket> AcceptAsyncByTimeout (Socket socket, Socket acceptSocket, int receiveSize, int timeout) {
            //TODO: NotImplementedException 
            //TODO: XML + Exceptions
            //TODO: Comment
            //TODO: Exception handler
            //TODO: Check inputs
            //TODO: don't repeat yourself (DRY)
            //TODO: Add the word "Async" to async method's name.
            //TODO: Does the code have magic numbers?
            throw new NotImplementedException ();
        }

        #endregion

        #region DisconnectAsync

        /// <summary>
        /// An asynchronous request to disconnect from a remote endpoint.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="reuseSocket">true if this socket can be reused after the connection is closed; otherwise, false.</param>
        /// <exception cref="ArgumentNullException">Throw if socket is null.</exception>
        public static Task DisconnectAsync (Socket socket, bool reuseSocket) {
            if (socket == null)
                throw new ArgumentNullException (nameof (socket));

            return Task.Factory.FromAsync (socket.BeginDisconnect, socket.EndDisconnect, reuseSocket, null);
        }
        #endregion
    }
}