using System.Threading.Tasks;
using CoviDoc.Controllers.Notifications.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoviDoc.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : ControllerBase
    {
        private readonly ILogger<InboxController> _logger;

        public InboxController(ILogger<InboxController> logger)
        {
            _logger = logger;
        }
        
        public async Task<IActionResult> Post([FromForm] IncomingMessageNotification notification)
        {
            _logger.LogInformation(notification.To);
            _logger.LogInformation(notification.From);
            return Ok();
        }
    }
}