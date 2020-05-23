using System.Threading.Tasks;
using CoviDoc.Controllers.Notifications.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoviDoc.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkSmsOptoutController : ControllerBase
    {
        private readonly ILogger<BulkSmsOptoutController> _logger;

        public BulkSmsOptoutController(ILogger<BulkSmsOptoutController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Post([FromForm] BulkSmsOptOutNotification optOutNotification)
        {
            _logger.LogInformation(optOutNotification.PhoneNumber);
            return Ok();
        }
    }
}