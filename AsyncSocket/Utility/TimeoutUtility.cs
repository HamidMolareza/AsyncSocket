using System;
using System.Threading.Tasks;

//TODO: Code Quality checklist

namespace AsyncSocket.Utility {
    public static class TimeoutUtility {
        public static Task<T> ExecuteAsync<T> (Task<T> task, int timeout) {
            task.Start ();
            return AwaitAsync (task, timeout);
        }

        public static async Task<T> AwaitAsync<T> (Task<T> task, int timeout) {
            if (await Task.WhenAny (task, Task.Delay (timeout)) == task)
                return task.Result;

            throw new TimeoutException ();
        }
    }
}