using System.Threading.Tasks;
using CoviDoc.Controllers.Notifications.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoviDoc.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsDeliveryReportsController : ControllerBase
    {
        private readonly ILogger<SmsDeliveryReportsController> _logger;

        public SmsDeliveryReportsController(ILogger<SmsDeliveryReportsController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Post([FromForm] SmsDeliveryReportNotification deliveryReportNotification)
        {
            _logger.LogInformation(deliveryReportNotification.FailureReason);
            return Ok();
        }
    }
}
