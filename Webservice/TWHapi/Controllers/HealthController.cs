using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Queue;
using Services.ServiceProcessing;

namespace TWHapi.Controllers
{
    [Route("api/health")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IBackgroundTaskQueue _queue;
        private readonly IServiceProcessing _serviceProcessing;

        public HealthController(
            ILogger<HealthController> logger,
            IBackgroundTaskQueue queue,
            IServiceProcessing serviceProcessing)
        {
            this._logger = logger;
            this._queue = queue;
            this._serviceProcessing = serviceProcessing;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("enqueue")]
        public IActionResult CheckBackgroundWorker()
        {
            var x = _queue.EnqueueAsync(async ct =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
                _logger.LogWarning("1");
                _logger.LogWarning("2");
                Console.WriteLine("3");
            });
            _ = _serviceProcessing.StartProcessing(async ct =>
            {
                _logger.LogWarning("Service Processing Task Executed.");
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
                _logger.LogInformation("1- Service Processing Task Completed.");
                _logger.LogInformation("2- Service Processing Task Completed.");
            });
            return Ok(x.Result.ToString());
        }
    }
}
