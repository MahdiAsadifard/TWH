using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Queue;

namespace TWHapi.Controllers
{
    [Route("api/health")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IBackgroundTaskQueue _queue;

        public HealthController(
            ILogger<HealthController> logger,
            IBackgroundTaskQueue queue)
        {
            this._logger = logger;
            this._queue = queue;
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
            return Ok(x.Result.ToString());
        }
    }
}
