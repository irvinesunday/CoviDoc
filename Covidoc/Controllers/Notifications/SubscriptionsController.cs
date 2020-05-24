using System.Threading.Tasks;
using CoviDoc.Controllers.Notifications.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoviDoc.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ILogger<SubscriptionsController> _logger;

        public SubscriptionsController(ILogger<SubscriptionsController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Post([FromForm] SubscriptionNotification content)
        {
            _logger.LogInformation(content.Keyword);
            return Ok();
        }
    }
}