using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoviDoc.Common;
using CoviDoc.Models;
using CoviDoc.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoviDoc.Controllers
{
    // [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisReportsController : ControllerBase
    {

        readonly IPatientRepository _patientRepository;
        readonly IDiagnosisReportRepository _diagnosisReportRepository;
        readonly IHealthCertificateRepository _healthCertificateRepository;

        public DiagnosisReportsController(IPatientRepository patientRepository,
                                        IDiagnosisReportRepository diagnosisReportRepository,
                                        IHealthCertificateRepository healthCertificateRepository)
        {
            _patientRepository = patientRepository;
            _diagnosisReportRepository = diagnosisReportRepository;
            _healthCertificateRepository = healthCertificateRepository;
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

                // Create Health Certificate
                var healthCertificate = new HealthCertificate
                {
                    DiagnosisReportId = diagnosisReport.DiagnosisReportId,
                    PatientId = diagnosisReportVM.PatientId,
                    IdNumber = diagnosisReportVM.PatientIdNumber,
                    MobileNumber = diagnosisReportVM.MobileNumber,
                    IsAdult = diagnosisReportVM.IsAdult,
                    Name = diagnosisReportVM.PatientName,
                    Status = diagnosisReportVM.TestStatus,
                    TestCentre = diagnosisReportVM.TestCentre,
                    TestDate = diagnosisReport.DateTested,
                    CertificateId = $"{diagnosisReportVM.PatientIdNumber}-{Helpers.GenerateCertificateId(5)}"
                };
                await _healthCertificateRepository.AddHealthCertificate(healthCertificate);

                // Send Test success alert

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
    }
}