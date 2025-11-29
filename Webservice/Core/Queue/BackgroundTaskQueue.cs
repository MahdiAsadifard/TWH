using Core.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async ValueTask<ValueTask> EnqueueAsync(Func<CancellationToken, Task> workItem)
        {
            ArgumentsValidator.ThrowIfNull(nameof(workItem), workItem);

            var tryWrite = _queue.Writer.TryWrite(workItem);
            _logger.LogInformation("Enqueue work item. Success: {TryWrite}", tryWrite);

            if (!tryWrite)
            {
                _logger.LogInformation("Queue is full, waiting to enqueue work item.");

                // Fallback to async wait if TryWrite fails (rare for unbounded)
                return _queue.Writer.WriteAsync(workItem);
            }
            return ValueTask.CompletedTask;
        }

        public async ValueTask<Func<CancellationToken, Task>> DequeuAsync(CancellationToken cancellationToken)
        {
            try
            {
                var workItem = await _queue.Reader.ReadAsync(cancellationToken);
                _logger.LogInformation("Dequeued work item.");
                return workItem;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Error occurred while dequeuing work item.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while dequeuing work item.");
                throw;
            }
        }
    }
}
