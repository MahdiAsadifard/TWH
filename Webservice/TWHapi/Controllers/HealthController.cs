using Core.ILogs;
using Core.Queue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ServiceProcessing;

namespace TWHapi.Controllers
{
    [Authorize]
    [Route("api/{customerUri?}/health")]
    public class HealthController(
            ILoggerHelpers<HealthController> logger,
            IBackgroundTaskQueue queue,
            IServiceProcessing serviceProcessing,
            IHealthOperations healthOperations
        ) : Controller
    {
        private readonly ILoggerHelpers<HealthController> _logger = logger;
        private readonly IBackgroundTaskQueue _queue = queue;
        private readonly IServiceProcessing _serviceProcessing = serviceProcessing;
        private readonly IHealthOperations _healthOperations = healthOperations;

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



        [HttpGet("{customerId}")]
        public async Task<IActionResult> TestCustomerIdFromRoute([FromRoute] string customerUri)
        {
            return Ok(new { message = $"CustomerUri from route: {customerUri}" });
        }

        [HttpPost("testRedis")]
        public async Task<IActionResult> TestRedis()
        {
            var result = await this._healthOperations.AddSampleRedisCache();
            return Ok(result);
        }
    }
}
