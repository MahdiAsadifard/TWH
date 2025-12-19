using Core.Common;
using Core.ILogs;
using Core.Queue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Core.BackgroundProcessing
{
    public sealed class BackgroundWorker(
            ILoggerHelpers<BackgroundWorker> logger,
            IBackgroundTaskQueue queue,
            IOptions<BackgroundTaskQueueOptions> backgroundTaskQueueOptions
        ) : BackgroundService
    {
        private readonly ILoggerHelpers<BackgroundWorker> _logger = logger;
        private readonly IBackgroundTaskQueue _queue = queue;
        private readonly IOptions<BackgroundTaskQueueOptions> _backgroundTaskQueueOptions = backgroundTaskQueueOptions;

        private CancellationToken _cancellationToken;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            this._cancellationToken = cancellationToken;

            while (!this._cancellationToken.IsCancellationRequested)
            {
                var workItem = await this.GetTask();
                await this.RunTask(workItem);
            }
        }

        private async Task<Func<CancellationToken, Task>> GetTask()
        {
            _logger.Log(ILogs.LogLevel.Information, "BackgroundWorker/DoWork: Background Worker is listening...");
            try
            {
                using var spw = new StopWatchHelper();

                _logger.Log(ILogs.LogLevel.Information, "---> BackgroundWorker/GetTask: Start picking task. time: [{time}]ms", spw.ElapsedMilliseconds);

                Func<CancellationToken, Task>? workItem = await _queue.DequeuAsync(this._cancellationToken);

                _logger.Log(ILogs.LogLevel.Information, "---> BackgroundWorker/GetTask: End picking task. time: [{time}]ms", spw.ElapsedMilliseconds);

                return workItem;
            }
            catch (OperationCanceledException ex)
            {
                _logger.Log(ex, "BackgroundWorker/GetTask: Background Worker is stopping due to cancellation.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "BackgroundWorker/GetTask: Error occurred dequeue in backgroundworker, ErrorMessage: {Message}", ex.Message);
                throw;
            }
        }

        private async Task RunTask(Func<CancellationToken, Task> workItem)
        {
            try
            {
                if (workItem is not null)
                {
                    using var spw = new StopWatchHelper();
                    _logger.Log(ILogs.LogLevel.Information, "---> BackgroundWorker/GetTask: Worker Started running task, time: [{time}]ms", spw.ElapsedMilliseconds);

                    await workItem(this._cancellationToken);

                    _logger.Log(ILogs.LogLevel.Information, "---> BackgroundWorker/RunTask: Worker Finished running task. time:[{time}]ms", spw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, "BackgroundWorker/RunTask: Error occurred executing work item.");
                throw;
            }
        }
    }
}
