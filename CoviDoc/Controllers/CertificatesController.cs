using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoviDoc.Models;
using CoviDoc.Models.Interfaces;
using CoviDoc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CoviDoc.Controllers
{
    public class CertificatesController : Controller
    {
        readonly IPatientRepository _patientRepository;
        readonly ITestCentreRepository _testCentreRepository;
        readonly IDiagnosisReportRepository _diagnosisReportRepository;
        readonly IHealthCertificateRepository _healthCertificateRepository;

        public CertificatesController(IPatientRepository patientRepository,
                                         ITestCentreRepository testCentreRepository,
                                         IDiagnosisReportRepository diagnosisReportRepository,
                                         IHealthCertificateRepository healthCertificateRepository)
        {
            _patientRepository = patientRepository;
            _testCentreRepository = testCentreRepository;
            _diagnosisReportRepository = diagnosisReportRepository;
            _healthCertificateRepository = healthCertificateRepository;
        }
        public async Task<IActionResult> Index(string idNumber, string name)
        {
            List<DiagnosisReport> diagnosisReports = new List<DiagnosisReport>();
            List<Patient> patients = await _patientRepository.GetPatients();

            if (!string.IsNullOrEmpty(idNumber))
            {
                List<Patient> filteredPatients = patients.Where(p => p.IdNumber.Equals(idNumber, StringComparison.OrdinalIgnoreCase)).ToList();

                if (filteredPatients != null)
                {
                    diagnosisReports = await _diagnosisReportRepository.GetDiagnosisReports(filteredPatients);
                }
            }
            else if (!string.IsNullOrEmpty(name))
            {
                List<Patient> filteredPatients = patients.Where(p => p.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                if (filteredPatients != null)
                {
                    diagnosisReports = await _diagnosisReportRepository.GetDiagnosisReports(filteredPatients);
                }
            }
            else
            {
                diagnosisReports = await _diagnosisReportRepository.GetDiagnosisReports();
            }

            List<TestsViewModel> diagnosisReportsVM = new List<TestsViewModel>();

            foreach(var report in diagnosisReports)
            {
                var diagnosisReportVM = new TestsViewModel
                {
                    Patient = patients.FirstOrDefault(p => p.ID == report.PatientId),
                  //  TestCentre =  _testCentreRepository.GetTestCentre(report.TestCentreId),
                    DiagnosisReport = report
                };
                diagnosisReportsVM.Add(diagnosisReportVM);
            }

            return View(diagnosisReportsVM);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var healthCertificate = _healthCertificateRepository.GetHealthCertificate(id);

            if (healthCertificate == null)
            {
                return NotFound();
            }

            return View(healthCertificate);
        }

    }
}