using System;
using System.Threading.Tasks;
using Xunit;

namespace Test_AsyncSocket.Test_Utility {
    public class TaskUtility {
        #region  WaitAsync

        #region async Task<T> WaitAsync<T> (Task<T> task, int timeout)

        [Theory]
        [InlineData (5, 20)]
        [InlineData (10, 1000)]
        public async Task WaitAsyncT_LessThanTimeoutTakesLong_ReturnAnswer (int delay, int timeout) {
            var task = Task.Run (() => Sum (2, 3, delay));
            var actual = await AsyncSocket.Utility.TaskUtility.WaitAsync (task, timeout);
            Assert.Equal (5, actual);
        }

        [Theory]
        [InlineData (20, 5)]
        [InlineData (10000, 10)]
        public void WaitAsyncT_MoreThanTimeoutTakesLong_ThrowTimeoutException (int delay, int timeout) {
            var task = Task.Run (() => Sum (2, 3, delay));
            Assert.ThrowsAsync<TimeoutException> (() => AsyncSocket.Utility.TaskUtility.WaitAsync (task, timeout));
        }

        private static int Sum (int a, int b, int delay) {
            Task.Delay (delay).Wait ();
            return a + b;
        }

        [Fact]
        public void WaitAsyncT_TaskIsNull_ThrowArgumentNullException () {
            Assert.ThrowsAsync<ArgumentNullException> (() => AsyncSocket.Utility.TaskUtility.WaitAsync<int> (null, 500));
        }

        [Theory]
        [InlineData (-1)]
        [InlineData (int.MaxValue * -1)]
        public void WaitAsyncT_OutOfRangeTimeout_ThrowOutOfRangeException (int timeout) {
            //Fake Task
            var task = new Task<int> (() => Sum (2, 3, 20));

            Assert.ThrowsAsync<ArgumentOutOfRangeException> (() => AsyncSocket.Utility.TaskUtility.WaitAsync (task, timeout));
        }

        #endregion

        #region async Task WaitAsync (Task task, int timeout)

        [Theory]
        [InlineData (5, 20)]
        [InlineData (10, 1000)]
        public async Task WaitAsync_LessThanTimeoutTakesLong_ReturnAnswer (int delay, int timeout) {
            var task = Task.Run (() => Task.Delay (delay).Wait ());
            await AsyncSocket.Utility.TaskUtility.WaitAsync (task, timeout);
            Assert.True (true);
        }

        [Theory]
        [InlineData (20, 5)]
        [InlineData (10000, 10)]
        public void WaitAsync_MoreThanTimeoutTakesLong_ThrowTimeoutException (int delay, int timeout) {
            var task = Task.Run (() => { Task.Delay (delay).Wait (); });
            Assert.ThrowsAsync<TimeoutException> (() => AsyncSocket.Utility.TaskUtility.WaitAsync (task, timeout));
        }

        [Fact]
        public void WaitAsync_TaskIsNull_ThrowArgumentNullException () {
            Assert.ThrowsAsync<ArgumentNullException> (() => AsyncSocket.Utility.TaskUtility.WaitAsync<int> (null, 500));
        }

        [Theory]
        [InlineData (-1)]
        [InlineData (int.MaxValue * -1)]
        public void WaitAsync_OutOfRangeTimeout_ThrowOutOfRangeException (int timeout) {
            //Fake Task
            var task = Task.Run (() => { });

            Assert.ThrowsAsync<ArgumentOutOfRangeException> (() => AsyncSocket.Utility.TaskUtility.WaitAsync (task, timeout));
        }

        #endregion

        #endregion

        #region ExecuteAsync

        #region Task<T> ExecuteAsync<T> (Task<T> task, int timeout)

        [Theory]
        [InlineData (5, 20)]
        [InlineData (10, 1000)]
        public async Task ExecuteAsyncT_LessThanTimeoutTakesLong_ReturnAnswer (int delay, int timeout) {
            var task = new Task<int> (() => Sum (2, 3, delay));
            var actual = await AsyncSocket.Utility.TaskUtility.ExecuteAsync (task, timeout);
            Assert.Equal (5, actual);
        }

        [Theory]
        [InlineData (20, 5)]
        [InlineData (10000, 10)]
        public void ExecuteAsyncT_MoreThanTimeoutTakesLong_ThrowTimeoutException (int delay, int timeout) {
            var task = new Task<int> (() => Sum (2, 3, delay));
            Assert.ThrowsAsync<TimeoutException> (() => AsyncSocket.Utility.TaskUtility.ExecuteAsync (task, timeout));
        }

        [Fact]
        public void ExecuteAsyncT_TaskIsNull_ThrowArgumentNullException () {
            Assert.ThrowsAsync<ArgumentNullException> (() => AsyncSocket.Utility.TaskUtility.ExecuteAsync<int> (null, 500));
        }

        [Theory]
        [InlineData (-1)]
        [InlineData (int.MaxValue * -1)]
        public void ExecuteAsyncT_OutOfRangeTimeout_ThrowOutOfRangeException (int timeout) {
            //Fake Task
            var task = new Task<int> (() => Sum (2, 3, 20));

            Assert.ThrowsAsync<ArgumentOutOfRangeException> (() => AsyncSocket.Utility.TaskUtility.ExecuteAsync (task, timeout));
        }

        #endregion

        #region Task ExecuteAsync (Task task, int timeout)

        [Theory]
        [InlineData (5, 20)]
        [InlineData (10, 1000)]
        public async Task ExecuteAsync_LessThanTimeoutTakesLong_ReturnAnswer (int delay, int timeout) {
            var task = new Task (() => Task.Delay (delay).Wait ());
            await AsyncSocket.Utility.TaskUtility.ExecuteAsync (task, timeout);
            Assert.True (true);
        }

        [Theory]
        [InlineData (20, 5)]
        [InlineData (10000, 10)]
        public void ExecuteAsync_MoreThanTimeoutTakesLong_ThrowTimeoutException (int delay, int timeout) {
            var task = new Task (() => Task.Delay (delay).Wait ());
            Assert.ThrowsAsync<TimeoutException> (() => AsyncSocket.Utility.TaskUtility.ExecuteAsync (task, timeout));
        }

        [Fact]
        public void ExecuteAsync_TaskIsNull_ThrowArgumentNullException () {
            Assert.ThrowsAsync<ArgumentNullException> (() => AsyncSocket.Utility.TaskUtility.ExecuteAsync<int> (null, 500));
        }

        [Theory]
        [InlineData (-1)]
        [InlineData (int.MaxValue * -1)]
        public void ExecuteAsync_OutOfRangeTimeout_ThrowOutOfRangeException (int timeout) {
            //Fake Task
            var task = new Task (() => { });

            Assert.ThrowsAsync<ArgumentOutOfRangeException> (() => AsyncSocket.Utility.TaskUtility.ExecuteAsync (task, timeout));
        }

        #endregion

        #endregion
    }
}