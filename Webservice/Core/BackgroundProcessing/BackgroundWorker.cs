using Core.Common;
using Core.Queue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.BackgroundProcessing
{
    public sealed class BackgroundWorker : BackgroundService
    {
        private readonly ILogger<BackgroundWorker> _logger;
        private readonly IBackgroundTaskQueue _queue;
        private readonly IOptions<BackgroundTaskQueueOptions> _backgroundTaskQueueOptions;

        private CancellationToken _cancellationToken;

        public BackgroundWorker(
            ILogger<BackgroundWorker> logger,
            IBackgroundTaskQueue queue,
            IOptions<BackgroundTaskQueueOptions> backgroundTaskQueueOptions)
        {
            this._logger = logger;
            this._queue = queue;
            this._backgroundTaskQueueOptions = backgroundTaskQueueOptions;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            this._cancellationToken = cancellationToken;

            while (!this._cancellationToken.IsCancellationRequested)
            {
                var workItem = await this.GetTask();
                await this.RunTask(workItem);
            }
        }

        public async Task _DoWork()
        {
            _logger.LogInformation("BackgroundWorker/DoWork: Background Worker is starting.");
            while (!this._cancellationToken.IsCancellationRequested)
            {
                Func<CancellationToken, Task> workItem = null;
                try
                {
                    throw new Exception("DOWORK EX");
                    _logger.LogInformation("BackgroundWorker/DoWork: start dequeue item in backgroundworker.");
                    workItem = await _queue.DequeuAsync(this._cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError(ex, "BackgroundWorker/DoWork: Background Worker is stopping due to cancellation.");
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "BackgroundWorker/DoWork: Error occurred dequeue in backgroundworker. Retrying in {Delay}s, ErrorMessage: {Message}",
                       _backgroundTaskQueueOptions.Value.RetryQueueDelaySeconds,
                       ex.Message);

                    try
                    {
                        await CoreUtility.DelayTask(TimeSpan.FromSeconds(_backgroundTaskQueueOptions.Value.RetryQueueDelaySeconds));
                        workItem = await _queue.DequeuAsync(this._cancellationToken);
                    }
                    catch (Exception)
                    {
                        _logger.LogCritical(ex, "BackgroundWorker/DoWork: Error occurred dequeue in backgroundworker. Exiting process...");
                        throw;
                    }
                }

                try
                {
                    if (workItem is not null)
                    {
                        _logger.LogInformation("---> BackgroundWorker/DoWork: Worker Started picking task - IsCancellationRequested: {IsCancellationRequested}", this._cancellationToken.IsCancellationRequested);
                        await workItem(this._cancellationToken);

                        _logger.LogInformation("---> BackgroundWorker/DoWork: Worker Finished - IsCancellationRequested: {IsCancellationRequested}", this._cancellationToken.IsCancellationRequested);
                    }
                }
                catch (Exception)
                {
                    _logger.LogError("BackgroundWorker/DoWork: Error occurred executing work item.");
                    throw;
                }
            }
            _logger.LogInformation("BackgroundWorker/DoWork: Background Worker is stopping.");
        }

        private async Task<Func<CancellationToken, Task>> GetTask()
        {
            _logger.LogInformation("BackgroundWorker/DoWork: Background Worker is listening...");
            Func<CancellationToken, Task> workItem = null;
            try
            {
                using var spw = new StopWatchHelper();

                _logger.LogInformation("---> BackgroundWorker/GetTask: Start picking task. time: [{time}]ms", spw.ElapsedMilliseconds);

                workItem = await _queue.DequeuAsync(this._cancellationToken);

                _logger.LogInformation("---> BackgroundWorker/GetTask: End picking task. time: [{time}]ms", spw.ElapsedMilliseconds);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "BackgroundWorker/GetTask: Background Worker is stopping due to cancellation.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BackgroundWorker/GetTask: Error occurred dequeue in backgroundworker, ErrorMessage: {Message}", ex.Message);
                throw;
            }
            return workItem;
        }

        private async Task RunTask(Func<CancellationToken, Task> workItem)
        {
            try
            {
                if (workItem is not null)
                {
                    using var spw = new StopWatchHelper();
                    _logger.LogInformation("---> BackgroundWorker/GetTask: Worker Started running task, time: [{time}]ms", spw.ElapsedMilliseconds);

                    await workItem(this._cancellationToken);

                    _logger.LogInformation("---> BackgroundWorker/RunTask: Worker Finished running task. time:[{time}]ms", spw.ElapsedMilliseconds);
                }
            }
            catch (Exception)
            {
                _logger.LogError("BackgroundWorker/RunTask: Error occurred executing work item.");
                throw;
            }
        }
    }
}
