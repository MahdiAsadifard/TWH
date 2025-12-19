using Core.ILogs;
using Core.Queue;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Api.CoreTests
{
    public class BackgroundTaskQueueTests
    {
        private readonly BackgroundTaskQueue _backgroundTaskQueue;
        private readonly Mock<ILoggerHelpers<BackgroundTaskQueue>> _mockLogger;
        private readonly Func<CancellationToken, Task> _workItem;

        public BackgroundTaskQueueTests()
        {
            _mockLogger = new Mock<ILoggerHelpers<BackgroundTaskQueue>>();
            _workItem = ct => Task.CompletedTask;

            _backgroundTaskQueue = new BackgroundTaskQueue(_mockLogger.Object);
        }

        [Fact]
        public async Task EnqueueAsync_And_DequeueAsync_Success()
        {
            // Arrange

            // Act
            await _backgroundTaskQueue.EnqueueAsync(_workItem, "TestProcess");
            var dequeued = await _backgroundTaskQueue.DequeuAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(dequeued);
            await dequeued(CancellationToken.None);

            
            _mockLogger.Verify(
                 l => l.Log(
                     Core.ILogs.LogLevel.Information,
                     It.Is<string>(s => s.Contains("Enqueue work item")),
                     It.Is<object[]>(args => args.Length == 3 && args[0].ToString() == "True" && args[1].ToString() == "TestProcess")
                 ),Times.AtLeastOnce);
        }

        [Fact]
        public async Task EnqueueAsync_NullWorkItem_ThrowsArgumentNullException()
        {
            // Arrange

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _backgroundTaskQueue.EnqueueAsync(null, "NullProcess"));
        }

        [Fact]
        public async Task DequeueAsync_WhenCancelled_ThrowsOperationCanceledException()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                async () => await _backgroundTaskQueue.DequeuAsync(cts.Token));
        }
    }
}
