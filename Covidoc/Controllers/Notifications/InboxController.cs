using System.Threading.Tasks;

using CoviDoc.Controllers.Notifications.Model;
using CoviDoc.Services;
using CoviDoc.Services.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoviDoc.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : ControllerBase
    {
        private readonly ILogger<InboxController> _logger;
        private readonly AfricasTalkingGateway _gateWayService;

        public InboxController(ILogger<InboxController> logger, AfricasTalkingGateway gateWayService)
        {
            _logger = logger;
            _gateWayService = gateWayService;
        }

        public async Task<IActionResult> Post([FromForm] IncomingMessageNotification notification)
        {
            _logger.LogInformation(notification.To);
            _logger.LogInformation(notification.From);
            var result =await _gateWayService.SendSmsMessage(new SmsMessage(notification.From, "Welcome to our service"));
            
            return Ok();
        }
    }
}