using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoviDoc.Models;
using CoviDoc.Models.Interfaces;
using CoviDoc.ViewModels;
using CoviDoc.Common;
using MessagingService;

namespace CoviDoc.Controllers
{
    public class PatientsController : Controller
    {
        readonly IPatientRepository _patientRepository;
        readonly ITestCentreRepository _testCentreRepository;
        readonly ILocationRepository _locationRepository;
        readonly IDiagnosisReportRepository _diagnosisReportRepository;

        public PatientsController(IPatientRepository patientRepository,
                                 ITestCentreRepository testCentreRepository,
                                 ILocationRepository locationRepository,
                                 IDiagnosisReportRepository diagnosisReportRepository)
        {
            _patientRepository = patientRepository;
            _testCentreRepository = testCentreRepository;
            _locationRepository = locationRepository;
            _diagnosisReportRepository = diagnosisReportRepository;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            var patients = await _patientRepository.GetPatients();
            return View(patients);
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientRepository.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }

            DiagnosisReport diagnosisReport = await _diagnosisReportRepository.GetDiagnosisReport(patient.ID);
            TestCentre testCentre = null;

            if (diagnosisReport != null)
            {
                testCentre = _testCentreRepository.GetTestCentre(diagnosisReport.TestCentreId);
            }

            PatientViewModel patientViewModel = new PatientViewModel()
            {
                Patient = patient,
                DiagnosisReport = diagnosisReport ?? new DiagnosisReport(),
                TestCentre = testCentre
            };

            return View(patientViewModel);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            PatientViewModel patientCreateViewModel = new PatientViewModel()
            {
                Countries = _locationRepository.GetCountries(),
                Counties = _locationRepository.GetCounties(),
                Constituencies = _locationRepository.GetConstituencies(),
                Wards = _locationRepository.GetWards()
            };

            return View(patientCreateViewModel);
        }

        public ActionResult GetConstituencies(string CountyName)
        {
            if (!string.IsNullOrEmpty(CountyName))
            {
                IEnumerable<SelectListItem> constituencies = _locationRepository.GetConstituencies(CountyName);
                return Json(constituencies);
            }
            return null;
        }

        public ActionResult GetWards(string CountyName, string constituencyId)
        {
            if (!string.IsNullOrEmpty(CountyName) && !string.IsNullOrEmpty(constituencyId))
            {
                IEnumerable<SelectListItem> wards = _locationRepository.GetWards(CountyName, constituencyId);
                return Json(wards);
            }
            return null;
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,MiddleName,LastName,IdNumber,DoB,Gender,Nationality,MobileNumber,County,Constituency,Ward")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    patient.ID = Guid.NewGuid();
                    patient.IsAdult = Helpers.IsAdult(patient.DoB);
                    patient.DateRegistered = DateTime.Now;

                    await _patientRepository.AddPatient(patient);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }

            PatientViewModel patientViewModel = new PatientViewModel()
            {
                Countries = _locationRepository.GetCountries(),
                Counties = _locationRepository.GetCounties(),
                Constituencies = _locationRepository.GetConstituencies(),
                Wards = _locationRepository.GetWards()
            };

            return View(patientViewModel);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientRepository.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }

            PatientViewModel patientViewModel = new PatientViewModel()
            {
                Patient = patient,
                Countries = _locationRepository.GetCountries(),
                Counties = _locationRepository.GetCounties(),
                Constituencies = _locationRepository.GetConstituencies(patient.County),
                Wards = _locationRepository.GetWards(patient.County, Helpers.GetConstituencyId(patient.Constituency))
            };

            return View(patientViewModel);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
                                             [Bind("ID,FirstName,MiddleName,LastName,IdNumber,DoB,Gender,Nationality,MobileNumber,County,Constituency,Ward,DateRegistered")] Patient patient)
        {
            if (id != patient.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    patient.IsAdult = Helpers.IsAdult(patient.DoB);
                    await _patientRepository.UpdatePatient(patient);
                }
                catch (ArgumentNullException)
                {
                    return NotFound();
                }
                catch (InvalidOperationException)
                {
                    return BadRequest();
                }
                return RedirectToAction(nameof(Index));
            }

            PatientViewModel patientViewModel = new PatientViewModel()
            {
                Patient = patient,
                Countries = _locationRepository.GetCountries(),
                Counties = _locationRepository.GetCounties(),
                Constituencies = _locationRepository.GetConstituencies(patient.County),
                Wards = _locationRepository.GetWards(patient.County, Helpers.GetConstituencyId(patient.Constituency))
            };

            return View(patientViewModel);
        }

        public async Task<IActionResult> Test(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientRepository.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }

            PatientTestViewModel patientTestViewModel = new PatientTestViewModel()
            {
                PatientId = patient.ID,
                PatientName = patient.FullName,
                PatientIdNumber = patient.IdNumber,
                PatientGender = patient.Gender,
                TestCentres = _testCentreRepository.GetTestCentres()
            };

            return View(patientTestViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Test([Bind("PatientId,TestCentreId,TestStatus")] PatientTestViewModel patientTestViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DiagnosisReport diagnosisReport = new DiagnosisReport
                    {
                        DiagnosisReportId = Guid.NewGuid(),
                        PatientId = patientTestViewModel.PatientId,
                        TestStatus = patientTestViewModel.TestStatus,
                        TestCentreId = patientTestViewModel.TestCentreId,
                        DateTested = DateTime.Now
                    };

                    await _diagnosisReportRepository.AddDiagnosisReport(diagnosisReport);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _patientRepository.GetPatient(id);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var patient = await _patientRepository.GetPatient(id);
            await _patientRepository.DeactivatePatient(patient);
            return RedirectToAction(nameof(Index));
        }
    }
}
