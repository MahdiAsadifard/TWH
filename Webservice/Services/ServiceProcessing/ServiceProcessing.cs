using Core.Common;
using Core.Exceptions;
using Core.Queue;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Services.ServiceProcessing
{
    public class ServiceProcessing : IServiceProcessing
    {
        private readonly ILogger<ServiceProcessing> _logger;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IOptions<BackgroundTaskQueueOptions> _backgroundTaskQueueOptions;

        public ServiceProcessing(
            ILogger<ServiceProcessing> logger,
            IBackgroundTaskQueue backgroundTaskQueue,
            IOptions<BackgroundTaskQueueOptions> backgroundTaskQueueOptions)
        {
            this._logger = logger;
            this._backgroundTaskQueue = backgroundTaskQueue;
            this._backgroundTaskQueueOptions = backgroundTaskQueueOptions;
        }
        public async Task StartProcessing(
            Func<CancellationToken, Task> workItem,
            ServiceProcessingName processName)
        {
            _logger.LogInformation("ServiceProcessing/StartProcessing: Starting service processing... ProcessName: {Name} ", processName);

            ArgumentsValidator.ThrowIfNull(nameof(workItem), workItem);

            CancellationToken ct = new CancellationToken();

            try
            {
                await _backgroundTaskQueue.EnqueueAsync(workItem, processName.ToString());
            }
            catch (Exception ex)
            {
                try
                {
                    _logger.LogError(ex, "ServiceProcessing/StartProcessing: Error occurred executing {Name}. Retrying in {Delay}s, ErrorMessage: {Message}",
                        processName,
                        _backgroundTaskQueueOptions.Value.RetryQueueDelaySeconds,
                        ex.Message);

                    await CoreUtility.DelayTask(TimeSpan.FromSeconds(_backgroundTaskQueueOptions.Value.RetryQueueDelaySeconds));

                    await _backgroundTaskQueue.EnqueueAsync(workItem, processName.ToString());
                }
                catch (Exception)
                {
                    _logger.LogCritical(ex, "ServiceProcessing/StartProcessing: Error occurred executing {Name} on retry. Exiting process...", processName);
                    throw;
                }
            }
        }
    }
}
