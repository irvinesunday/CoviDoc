using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoviDoc.Models;
using CoviDoc.Models.Interfaces;
using CoviDoc.ViewModels;

namespace CoviDoc.Controllers
{
    public class PatientsController : Controller
    {
        readonly IPatientRepository _patientRepository;
        readonly ITestCentreRepository _testCentreRepository;
        readonly ILocationRepository _mockCountyRepository;

        public PatientsController(IPatientRepository patientRepository,
                                 ITestCentreRepository testCentreRepository,
                                 ILocationRepository mockCountyRepository)
        {
            _patientRepository = patientRepository;
            _testCentreRepository = testCentreRepository;
            _mockCountyRepository = mockCountyRepository;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            var patients = _patientRepository.GetPatients();
            return View(patients);
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _patientRepository.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var patient = _patientRepository.GetPatients(id);
        //    if (patient == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(patient);
        //}

        // GET: Patients/Create
        public IActionResult Create()
        {
            //var counties = _mockCountyRepository.GetCounties();
            //List<string> countyName = new List<string>();
            //foreach(var county in counties)
            //{
            //    countyName.Add(county.CountyName);
            //}
            //ViewData["Counties"] = countyName;

            PatientCreateViewModel patientCreateViewModel = new PatientCreateViewModel()
            {
                Counties = _mockCountyRepository.GetCounties(),
                Constituencies = _mockCountyRepository.GetConstituencies(),
                Wards = _mockCountyRepository.GetWards()
            };

            return View(patientCreateViewModel);
        }

        public ActionResult GetConstituencies(int CountyId)
        {
            if (CountyId > 0)
            {
                IEnumerable<SelectListItem> constituencies = _mockCountyRepository.GetConstituencies(CountyId);
                return Json(constituencies);
            }
            return null;
        }

        public ActionResult GetWards(int CountyId, string constituencyId)
        {
            if (CountyId > 0 && !string.IsNullOrEmpty(constituencyId))
            {
                IEnumerable<SelectListItem> wards = _mockCountyRepository.GetWards(CountyId, constituencyId);
                return Json(wards);
            }
            return null;
        }

    //    // POST: Patients/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    //    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("ID,FirstName,MiddleName,LastName,IdNumber,DoB,Gender,Nationality,MobileNumber,County,Constituency,Ward,IsAdult,IsActive,DateRegistered")] Patient patient)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            patient.ID = Guid.NewGuid();
    //            _context.Add(patient);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(patient);
    //    }

    //    // GET: Patients/Edit/5
    //    public async Task<IActionResult> Edit(Guid? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var patient = await _context.Patient.FindAsync(id);
    //        if (patient == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(patient);
    //    }

    //    // POST: Patients/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    //    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(Guid id, [Bind("ID,FirstName,MiddleName,LastName,IdNumber,DoB,Gender,Nationality,MobileNumber,County,Constituency,Ward,IsAdult,IsActive,DateRegistered")] Patient patient)
    //    {
    //        if (id != patient.ID)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(patient);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!PatientExists(patient.ID))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(patient);
    //    }

    //    // GET: Patients/Delete/5
    //    public async Task<IActionResult> Delete(Guid? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var patient = await _context.Patient
    //            .FirstOrDefaultAsync(m => m.ID == id);
    //        if (patient == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(patient);
    //    }

    //    // POST: Patients/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(Guid id)
    //    {
    //        var patient = await _context.Patient.FindAsync(id);
    //        _context.Patient.Remove(patient);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool PatientExists(Guid id)
    //    {
    //        return _context.Patient.Any(e => e.ID == id);
    //    }
    }
}
