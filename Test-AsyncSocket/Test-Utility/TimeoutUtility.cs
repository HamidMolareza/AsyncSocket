using System;
using System.Threading.Tasks;
using Xunit;

namespace Test_AsyncSocket.Test_Utility {
    public class TaskUtility {
        #region  WaitAsync

        [Theory]
        [InlineData (5, 10)]
        [InlineData (10, 30)]
        [InlineData (50, 60)]
        [InlineData (100, 110)]
        [InlineData (50, 10000)]
        public async Task WaitAsync_LessThanTimoutTakesLong_ReturnAnswer (int delay, int timeout) {
            var task = Task.Run (() => Sum (2, 3, delay));
            var actual = await AsyncSocket.Utility.TaskUtility.WaitAsync (task, timeout);
            Assert.Equal (5, actual);
        }

        [Theory]
        [InlineData (10, 5)]
        [InlineData (30, 10)]
        [InlineData (60, 50)]
        [InlineData (110, 100)]
        [InlineData (10000, 50)]
        public void WaitAsync_MoreThanTimoutTakesLong_ThrowTimeoutException (int delay, int timeout) {
            var task = Task.Run (() => Sum (2, 3, delay));
            Assert.ThrowsAsync<TimeoutException> (() => AsyncSocket.Utility.TaskUtility.WaitAsync (task, timeout));
        }

        private static int Sum (int a, int b, int delay) {
            Task.Delay (delay).Wait ();
            return a + b;
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
            var task = new Task<int> (() => Sum (2, 3, 20));

            Assert.ThrowsAsync<ArgumentOutOfRangeException> (() => AsyncSocket.Utility.TaskUtility.WaitAsync (task, timeout));
        }

        #endregion

        #region ExecuteAsync

        [Theory]
        [InlineData (5, 10)]
        [InlineData (10, 30)]
        [InlineData (50, 60)]
        [InlineData (100, 110)]
        [InlineData (50, 10000)]
        public async Task ExecuteAsync_LessThanTimoutTakesLong_ReturnAnswer (int delay, int timeout) {
            var task = new Task<int> (() => Sum (2, 3, delay));
            var actual = await AsyncSocket.Utility.TaskUtility.ExecuteAsync (task, timeout);
            Assert.Equal (5, actual);
        }

        [Theory]
        [InlineData (10, 5)]
        [InlineData (30, 10)]
        [InlineData (60, 50)]
        [InlineData (110, 100)]
        [InlineData (10000, 50)]
        public void ExecuteAsync_MoreThanTimoutTakesLong_ThrowTimeoutException (int delay, int timeout) {
            var task = new Task<int> (() => Sum (2, 3, delay));
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
            var task = new Task<int> (() => Sum (2, 3, 20));

            Assert.ThrowsAsync<ArgumentOutOfRangeException> (() => AsyncSocket.Utility.TaskUtility.ExecuteAsync (task, timeout));
        }

        #endregion
    }
}