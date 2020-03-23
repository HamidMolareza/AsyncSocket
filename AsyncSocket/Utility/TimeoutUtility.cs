using System;
using System.Threading.Tasks;

namespace AsyncSocket.Utility {
    public static class TimeoutUtility {

        /// <summary>
        /// Start the task and consider timeout
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task must be execute.</param>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException"></exception>
        public static Task<T> ExecuteAsync<T> (Task<T> task, int timeout) {
            task.Start ();
            return AwaitAsync (task, timeout);
        }

        /// <summary>
        /// If the task is completed within a specified timeout, return result otherwise throw timeout exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task must be execute.</param>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException"></exception>
        public static async Task<T> AwaitAsync<T> (Task<T> task, int timeout) {
            if (await Task.WhenAny (task, Task.Delay (timeout)) == task)
                return task.Result;

            throw new TimeoutException ();
        }
    }
}