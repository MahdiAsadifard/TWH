using Core.Queue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ServiceProcessing;

namespace TWHapi.Controllers
{
    [Route("api/health")]
    public class HealthController(
            ILogger<HealthController> logger,
            IBackgroundTaskQueue queue,
            IServiceProcessing serviceProcessing
        ) : Controller
    {
        private readonly ILogger<HealthController> _logger = logger;
        private readonly IBackgroundTaskQueue _queue = queue;
        private readonly IServiceProcessing _serviceProcessing = serviceProcessing;

        [AllowAnonymous]
        [HttpGet]
        [Route("enqueue")]
        public IActionResult CheckBackgroundWorker()
        {
            _ = _serviceProcessing.StartProcessing(async ct =>
            {
                _logger.LogWarning("Service Processing Task Executed.");
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
                _logger.LogInformation("1- Service Processing Task Completed.");
                _logger.LogInformation("2- Service Processing Task Completed.");
            }, ServiceProcessingName.HealthControllerCheck);
            return Ok();
        }
    }
}
