using Core.Common;
using Core.Exceptions;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Core.Queue
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue;
        private readonly ILogger<BackgroundTaskQueue> _logger;

        public BackgroundTaskQueue(ILogger<BackgroundTaskQueue> logger)
        {
            this._logger = logger;

            var options = new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false,
            };
            _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>(options);
        }

        public async ValueTask<ValueTask> EnqueueAsync(Func<CancellationToken, Task> workItem, string processName)
        {
            using var spw = new StopWatchHelper();
            try
            {

                ArgumentsValidator.ThrowIfNull(nameof(workItem), workItem);

                var tryWrite = _queue.Writer.TryWrite(workItem);
                _logger.LogInformation("BackgroundTaskQueue/EnqueueAsync: Enqueue work item. Success: {TryWrite}, ProcessName {ProcessName}, time: [{time}]ms",
                    tryWrite,
                    processName, spw.ElapsedMilliseconds);

                if (!tryWrite)
                {
                    _logger.LogInformation("BackgroundTaskQueue/EnqueueAsync: Queue is full, waiting to enqueue work item, ProcessName: {ProcessName}, time: [{time}]ms", processName, spw.ElapsedMilliseconds);

                    // Fallback to async wait if TryWrite fails (rare for unbounded)
                    return _queue.Writer.WriteAsync(workItem);
                }
                return ValueTask.CompletedTask;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "BackgroundTaskQueue/EnqueueAsync: OperationCanceledException Error occurred while enqueuing work item. time: [{time}]ms", spw.ElapsedMilliseconds);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BackgroundTaskQueue/EnqueueAsync: Error occurred while enqueuing work item. ProcessName: {ProcessName}, time: [{time}]ms", processName, spw.ElapsedMilliseconds);
                throw;
            }
        }

        public async ValueTask<Func<CancellationToken, Task>> DequeuAsync(CancellationToken cancellationToken)
        {
            using var spw = new StopWatchHelper();
            try
            {
                var workItem = await _queue.Reader.ReadAsync(cancellationToken);
                _logger.LogInformation("BackgroundTaskQueue/DequeuAsync: Dequeued work item. time: [{time}]ms", spw.ElapsedMilliseconds);
                return workItem;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "BackgroundTaskQueue/DequeuAsync: OperationCanceledException Error occurred while dequeuing work item. time: [{time}]ms", spw.ElapsedMilliseconds);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BackgroundTaskQueue/DequeuAsync: Error occurred while dequeuing work item. time: [{time}]ms", spw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
