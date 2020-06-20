using System.Threading.Tasks;

using CoviDoc.Controllers.Notifications.Model;
using CoviDoc.Models.Interfaces;
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
        private readonly IPatientRepository _patientRepository;
        private readonly IHealthCertificateRepository _healthCertificateRepository;

        public InboxController(ILogger<InboxController> logger,
                               AfricasTalkingGateway gateWayService,
                               IPatientRepository patientRepository,
                               IHealthCertificateRepository healthCertificateRepository)
        {
            _logger = logger;
            _gateWayService = gateWayService;
            _patientRepository = patientRepository;
            _healthCertificateRepository = healthCertificateRepository;
        }

        public async Task<IActionResult> Post([FromForm] IncomingMessageNotification notification)
        {
            // Get the sender's details
            var senderNumber = notification.From;
            var idNumber = notification.Text;

            // Get the certificates
            var certificates = _healthCertificateRepository.GetHealthCertificates(idNumber, senderNumber);

            _logger.LogInformation(notification.To);
            _logger.LogInformation(notification.From);
            var result = await _gateWayService.SendSmsMessage(new SmsMessage(notification.From, "Welcome to our service"));

            return Ok();
        }
    }
}