using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoviDoc.Models;
using CoviDoc.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoviDoc.Controllers
{
    public class PatientTestController : Controller
    {
        readonly IPatientRepository _patientRepository;
        readonly ITestCentreRepository _testCentreRepository;

        public PatientTestController(IPatientRepository patientRepository,
                                     ITestCentreRepository testCentreRepository)
        {
            _patientRepository = patientRepository;
            _testCentreRepository = testCentreRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Patient/Details/5
        [Produces("application/json")]
        [HttpGet]
        public List<Patient> Details(string idNumber)
        {
            var results = _patientRepository.GetPatient(idNumber);
            List<PatientTestViewModel> patientTestViews = new List<PatientTestViewModel>();

            foreach(var result in results)
            {
                var patientTestView = new PatientTestViewModel
                {
                    DoB = result.DoB,
                    IdNumber = result.IdNumber,
                    IsAdult = result.IsAdult,
                    PatientName = $"{result.FirstName} {result.MiddleName} {result.LastName}",
                    TestCentres = _testCentreRepository.GetTestCentres()
                };
            }
            return results;
            // return View();
        }
    }
}