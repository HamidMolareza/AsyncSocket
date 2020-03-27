using System;
using System.Threading.Tasks;

namespace AsyncSocket.Utility {
    public static class TaskUtility {

        #region ExecuteAsync

        /// <summary>
        /// Start the task and consider timeout.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task that must be execute.</param>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException"></exception>
        public static Task<T> ExecuteAsync<T> (Task<T> task, int timeout) {
            task.Start ();
            return WaitAsync (task, timeout);
        }

        /// <summary>
        /// Start the task and consider timeout.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task that must be execute.</param>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException"></exception>
        public static Task ExecuteAsync (Task task, int timeout) {
            task.Start ();
            return WaitAsync (task, timeout);
        }

        #endregion

        #region WaitAsync

        /// <summary>
        /// If the task is completed within a specified timeout, return result otherwise throw timeout exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task that must be execute.</param>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException"></exception>
        public static async Task<T> WaitAsync<T> (Task<T> task, int timeout) {
            if (await Task.WhenAny (task, Task.Delay (timeout)) == task)
                return task.Result;

            throw new TimeoutException ();
        }

        /// <summary>
        /// If the task is not completed within a specified timeout, throw timeout exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task that must be execute.</param>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException"></exception>
        public static async Task WaitAsync (Task task, int timeout) {
            if (await Task.WhenAny (task, Task.Delay (timeout)) == task)
                return;

            throw new TimeoutException ();
        }

        #endregion
    }
}