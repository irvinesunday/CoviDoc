using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoviDoc.Models;
using CoviDoc.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoviDoc.Controllers
{
    public class PatientController : Controller
    {
        readonly IPatientRepository _patientRepository;
        readonly ITestCentreRepository _testCentreRepository;

        public PatientController(IPatientRepository patientRepository,
                                 ITestCentreRepository testCentreRepository)
        {
            _patientRepository = patientRepository;
            _testCentreRepository = testCentreRepository;
        }
        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test(Guid id)
        {
            var patient = _patientRepository.GetPatient(id);
            var patientTestViewModel = new PatientTestViewModel
            {
                DoB = patient.DoB,
                IdNumber = patient.IdNumber,
                IsAdult = patient.IsAdult,
                PatientName = $"{patient.FirstName} {patient.MiddleName} {patient.LastName}",
                TestCentres = _testCentreRepository.GetTestCentres()
            };

            return View(patientTestViewModel);
        }

        // GET: Patient/Details/5
        [Produces("application/json")]
        [HttpGet]
        public JsonResult Details(string id)
        {
            var results = _patientRepository.GetPatients(id);
            return Json(results);
           // return View();
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
             return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Patient/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}