using Core.BackgroundProcessing;
using Core.Queue;
using DnsClient.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceProcessing
{
    public class ServiceProcessing
    {
        private readonly ILogger _logger;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        public ServiceProcessing(ILogger logger, IBackgroundTaskQueue backgroundTaskQueue)
        {
            this._logger = logger;
            this._backgroundTaskQueue = backgroundTaskQueue;
        }
        public async Task StartProcessing(Func<CancellationToken, Task> workItem)
        {
            _logger.LogInformation("Starting service processing...");
            // Implement service processing logic here
            CancellationToken ct = new CancellationToken();
            await _backgroundTaskQueue.EnqueueAsync(workItem);
        }
    }
}
