using Core.Queue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.BackgroundProcessing
{
    public sealed class BackgroundWorker : BackgroundService, IBackgroundWorker
    {
        private readonly ILogger<BackgroundWorker> _logger;
        private readonly IBackgroundTaskQueue _queue;

        public BackgroundWorker(
            ILogger<BackgroundWorker> logger,
            IBackgroundTaskQueue queue)
        {
            this._logger = logger;
            this._queue = queue;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await DoWork(cancellationToken);
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Worker is starting.");
            while (!cancellationToken.IsCancellationRequested)
            {
                Func<CancellationToken, Task> workItem = null;
                try
                {
                    workItem = await _queue.DequeuAsync(cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError(ex, "Background Worker is stopping due to cancellation.");
                    throw;
                }

                try
                {
                    if (workItem is not null)
                    {
                        _logger.LogInformation("=== Worker Started picking task - IsCancellationRequested: {IsCancellationRequested}", cancellationToken.IsCancellationRequested);
                        await workItem(cancellationToken);

                        _logger.LogInformation("=== Worker Finished - IsCancellationRequested: {IsCancellationRequested}", cancellationToken.IsCancellationRequested);
                    }
                }
                catch (Exception)
                {
                    _logger.LogError("Error occurred executing work item.");
                    throw;
                }
            }
            _logger.LogInformation("Background Worker is stopping.");
        }
    }
}
