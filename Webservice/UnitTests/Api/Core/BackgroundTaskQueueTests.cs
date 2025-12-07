using Core.Queue;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Api.Core
{
    public class BackgroundTaskQueueTests
    {
        private readonly BackgroundTaskQueue _backgroundTaskQueue;
        private readonly Mock<ILogger<BackgroundTaskQueue>> _mockLogger;
        private readonly Func<CancellationToken, Task> _workItem;

        public BackgroundTaskQueueTests()
        {
            _mockLogger = new Mock<ILogger<BackgroundTaskQueue>>();
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
                     LogLevel.Information,
                     It.IsAny<EventId>(),
                     It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Enqueue work item")),
                     null,
                     It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                 Times.AtLeastOnce);
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
