using Core.ILogs;
using Core.Queue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ServiceProcessing;

namespace TWHapi.Controllers
{
    [Route("api/health")]
    public class HealthController(
            ILoggerHelpers<HealthController> logger,
            IBackgroundTaskQueue queue,
            IServiceProcessing serviceProcessing
        ) : Controller
    {
        private readonly ILoggerHelpers<HealthController> _logger = logger;
        private readonly IBackgroundTaskQueue _queue = queue;
        private readonly IServiceProcessing _serviceProcessing = serviceProcessing;

        [AllowAnonymous]
        [HttpGet]
        [Route("enqueue")]
        public IActionResult CheckBackgroundWorker()
        {
            _ = _serviceProcessing.StartProcessing(async ct =>
            {
                _logger.Log(Core.ILogs.LogLevel.Information, "Service Processing Task Executed.");
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
                _logger.Log(Core.ILogs.LogLevel.Information, "1- Service Processing Task Completed.");
                _logger.Log(Core.ILogs.LogLevel.Information, "2- Service Processing Task Completed.");
            }, ServiceProcessingName.HealthControllerCheck);
            return Ok();
        }
    }
}
