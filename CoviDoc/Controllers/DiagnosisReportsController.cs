using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoviDoc.Models;
using CoviDoc.Models.Interfaces;
using MessagingService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoviDoc.Controllers
{
    // [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisReportsController : ControllerBase
    {

        readonly IPatientRepository _patientRepository;
        readonly IDiagnosisReportRepository _diagnosisReportRepository;
        readonly IHealthCertificateRepository _healthCertificateRepository;
        readonly string _emailFrom;
        private readonly string _emailPassword;
        private readonly string _emailUserName;

        public DiagnosisReportsController(IPatientRepository patientRepository,
                                        IDiagnosisReportRepository diagnosisReportRepository,
                                        IHealthCertificateRepository healthCertificateRepository,
                                        IConfiguration configuration)
        {
            _patientRepository = patientRepository;
            _diagnosisReportRepository = diagnosisReportRepository;
            _healthCertificateRepository = healthCertificateRepository;
            _emailFrom = configuration["EmailConfigs:CertificateEmailAddress"];
            _emailPassword = configuration["EmailConfigs:Password"];
            _emailUserName = configuration["EmailConfigs:UserName"];
        }

        // GET: api/DiagnosisReports
        [HttpGet]
        [Produces("application/json")]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetDiagnosisReportssAsync(string idNumber, Guid patientId)
        {
            if (!string.IsNullOrEmpty(idNumber) && patientId != Guid.Empty)
            {
                // Cannot define both parameters
                return BadRequest();
            }

            var diagnosisReportsVMs = new List<DiagnosisReportViewModel>();

            if (patientId != Guid.Empty)
            {
               var result = await GetUserDiagnosisReportsByPatientIdAsync(patientId);

                if (result == null)
                {
                    // Fetch patient
                    var patient = await _patientRepository.GetPatient(patientId);

                    if (patient == null)
                    {
                        return NotFound();
                    }

                    // Patient not yet tested, return default data
                    var diagnosisReportVM = new DiagnosisReportViewModel
                    {
                        PatientId = patient.ID,
                        PatientGender = patient.Gender,
                        PatientName = patient.FullName,
                        PatientIdNumber = patient.IdNumber
                    };
                    diagnosisReportsVMs.Add(diagnosisReportVM);
                    return Ok(diagnosisReportsVMs);
                }

                diagnosisReportsVMs.Add(result);
                return Ok(diagnosisReportsVMs);
            }

            if (!string.IsNullOrEmpty(idNumber))
            {
                var result = await GetUserDiagnosisByIdNumberAsync(idNumber);

                if (result == null || !result.Any())
                {
                    return NotFound();
                }

                return Ok(result);
            }

            // Fetch all diagnosis reports
            var diagnosisReports = await _diagnosisReportRepository.GetDiagnosisReports();

            if (diagnosisReports == null || !diagnosisReports.Any())
            {
                return NotFound();
            }

            foreach (var diagnosisReport in diagnosisReports)
            {
                var patient = await _patientRepository.GetPatient(diagnosisReport.PatientId);

                if (patient != null)
                {
                    var diagnosisReportVM = new DiagnosisReportViewModel
                    {
                        PatientId = patient.ID,
                        PatientIdNumber = patient.IdNumber,
                        PatientName = patient.FullName,
                        PatientGender = patient.Gender,
                        TestStatus = diagnosisReport.TestStatus,
                        TestCentre = diagnosisReport.TestCentre,
                        DateTested = diagnosisReport.DateTested
                    };

                    diagnosisReportsVMs.Add(diagnosisReportVM);
                }
            }

            return Ok(diagnosisReportsVMs);
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("api/[controller]/{patientId}")]
        public async Task<IActionResult> GetDiagnosisReportsAsync(Guid patientId)
        {
            if (patientId == Guid.Empty)
            {
                return BadRequest();
            }

            var result = await GetUserDiagnosisReportsByPatientIdAsync(patientId);

            if (result == null)
            {
                // Fetch patient
                var patient = await _patientRepository.GetPatient(patientId);

                if (patient == null)
                {
                    return NotFound();
                }

                // Patient not yet tested, return default data
                var diagnosisReportVM = new DiagnosisReportViewModel
                {
                    PatientId = patient.ID,
                    PatientGender = patient.Gender,
                    PatientName = patient.FullName,
                    PatientIdNumber = patient.IdNumber
                };
                return Ok(diagnosisReportVM);
            }

            return Ok(result);
        }

        // POST: api/DiagnosisReports
        [HttpPost]
        [Route("api/[controller]")]
        [Produces("application/json")]
        public async Task<IActionResult> PostDiagnosisReportsAsync([FromBody] DiagnosisReportViewModel diagnosisReportVM)
        {
            if (diagnosisReportVM != null)
            {
                var diagnosisReport = new DiagnosisReport
                {
                    DiagnosisReportId = Guid.NewGuid(),
                    DateTested = DateTime.UtcNow.AddHours(3),
                    PatientId = diagnosisReportVM.PatientId,
                    TestCentre = diagnosisReportVM.TestCentre,
                    TestStatus = diagnosisReportVM.TestStatus
                };
                await _diagnosisReportRepository.AddDiagnosisReport(diagnosisReport);
                string diagnosisReportUri = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.Path.Value, diagnosisReport.DiagnosisReportId.ToString());

                var patient = await _patientRepository.GetPatient(diagnosisReport.PatientId);

                var healthCertificate = _healthCertificateRepository.GetHealthCertificate(patient.ID);

                healthCertificate.TestStatus = diagnosisReportVM.TestStatus;
                healthCertificate.TestCentre = diagnosisReportVM.TestCentre;
                healthCertificate.TestDate = diagnosisReport.DateTested;

                await _healthCertificateRepository.UpdateHealthCertificate(healthCertificate);

                string emailBody = GenerateEmailBody(healthCertificate, patient);

                if (emailBody != null)
                {
                    Email email = new Email
                    {
                        From = _emailFrom,
                        To = patient.EmailAddress,
                        Subject = $"COVID-19 Digital Health Certificate - {diagnosisReportVM.PatientName}",
                        Body = emailBody
                    };
                    await EmailProcessor.SendEmailAsync(email, _emailUserName, _emailPassword);
                }

                return Created(diagnosisReportUri, diagnosisReport);
            }
            return BadRequest();
        }

        private async Task<DiagnosisReportViewModel> GetUserDiagnosisReportsByPatientIdAsync(Guid patientId)
        {
            if (patientId == Guid.Empty)
            {
                return null;
            }

            var patient = await _patientRepository.GetPatient(patientId);

            if (patient == null)
            {
                return null;
            }
            var diagnosisReport = await _diagnosisReportRepository.GetDiagnosisReport(patient);

            if (diagnosisReport == null)
            {
                return null;
            }

            var diagnosisReportVM = new DiagnosisReportViewModel
            {
                PatientId = patient.ID,
                PatientIdNumber = patient.IdNumber,
                PatientName = patient.FullName,
                PatientGender = patient.Gender,
                MobileNumber = patient.MobileNumber,
                IsAdult = patient.IsAdult,
                TestStatus = diagnosisReport.TestStatus,
                TestCentre = diagnosisReport.TestCentre,
                DateTested = diagnosisReport.DateTested
            };

            return diagnosisReportVM;
        }

        private async Task<List<DiagnosisReportViewModel>> GetUserDiagnosisByIdNumberAsync(string idNumber)
        {
            var patients = await _patientRepository.GetPatients(idNumber);

            if (patients == null)
            {
                return null;
            }

            var diagnosisReportsVMs = new List<DiagnosisReportViewModel>();

            foreach (var patient in patients)
            {
                var diagnosisReport = await _diagnosisReportRepository.GetDiagnosisReport(patient);

                if (diagnosisReport != null)
                {
                    var diagnosisReportVM = new DiagnosisReportViewModel
                    {
                        PatientId = patient.ID,
                        PatientIdNumber = patient.IdNumber,
                        PatientName = patient.FullName,
                        PatientGender = patient.Gender,
                        TestStatus = diagnosisReport.TestStatus,
                        TestCentre = diagnosisReport.TestCentre,
                        DateTested = diagnosisReport.DateTested
                    };

                    diagnosisReportsVMs.Add(diagnosisReportVM);
                }
                else
                {
                    // Add default patient bio
                    var diagnosisReportVM = new DiagnosisReportViewModel
                    {
                        PatientId = patient.ID,
                        PatientGender = patient.Gender,
                        PatientName = patient.FullName,
                        PatientIdNumber = patient.IdNumber
                    };
                    diagnosisReportsVMs.Add(diagnosisReportVM);
                }
            }

            return diagnosisReportsVMs;
        }

        private string GenerateEmailBody(HealthCertificate healthCertificate, Patient patient)
        {
            if (patient.EmailAddress != null)
            {
                string body = string.Empty;
                using (StreamReader reader = new StreamReader("./Resources/TestsTemplate.html"))
                {
                    body = reader.ReadToEnd();
                };
                body = body.Replace("{PatientFirstName}", patient.FirstName);
                body = body.Replace("{PatientFullName}", patient.FullName.ToUpper());
                body = body.Replace("{PatientIdNumber}", patient.IdNumber);
                body = body.Replace("{TestStatus}", healthCertificate.TestStatus.ToString());
                body = body.Replace("{TestDate}", healthCertificate.TestDate.ToString("dddd, dd MMMM yyyy hh:mm tt"));
                body = body.Replace("{TestCentre}", healthCertificate.TestCentre.ToString());
                body = body.Replace("{LinkToCertificate}", healthCertificate.BaseUrlLocation);
                body = body.Replace("{CertificateId}", healthCertificate.CertificateId);
                body = body.Replace("{BaseUrl}", $"{Request.Scheme}://{Request.Host}");
                body = body.Replace("{QRCode}", healthCertificate.BaseUrlLocation);

                return body;
            }

            return null;
        }
    }
}