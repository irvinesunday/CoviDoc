﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoviDoc.Common;
using CoviDoc.Models;
using CoviDoc.Models.Interfaces;
using CoviDoc.ViewModels;
using MessagingService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoviDoc.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {

        private readonly string _emailPassword;
        private readonly string _emailUserName;
        readonly string _emailFrom;
        readonly IPatientRepository _patientRepository;
        private IWebHostEnvironment _env;
        private const string QrCodeFolderName = "QRCodes";

        public PatientsController(IPatientRepository patientRepository,
                                  IConfiguration configuration,
                                  IWebHostEnvironment env)
        {
            _patientRepository = patientRepository;
            _emailFrom = configuration["EmailConfigs:RegistrationsEmailAddresss"];
            _emailPassword = configuration["EmailConfigs:Password"];
            _emailUserName = configuration["EmailConfigs:UserName"];
            _env = env;
        }

        [HttpGet]
        [Route("api/[controller]/{patientId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetPatientsAsync(Guid patientId)
        {
            if (patientId == Guid.Empty)
            {
                return NotFound();
            }

            var patient = await _patientRepository.GetPatient(patientId);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);

        }

        // GET: api/Patients
        [HttpGet]
        [Route("api/[controller]")]
        [Produces("application/json")]
        public async Task<IActionResult> GetPatientsAsync(string idNumber, string name)
        {
            if (!string.IsNullOrEmpty(idNumber) && !string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            List<Patient> patients = await _patientRepository.GetPatients();

            if (!string.IsNullOrEmpty(idNumber))
            {
                patients = patients.Where(p => p.IdNumber.Equals(idNumber, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(name))
            {
                patients = patients.Where(p => p.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(patients);
        }

        // POST: api/Patients
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> PostPatientAsync([FromBody] Patient patient)
        {
            try
            {
                if (patient != null)
                {
                    patient.ID = Guid.NewGuid();
                    patient.DateRegistered = DateTime.UtcNow.AddHours(3);
                    patient.MobileNumber = Helpers.FormatMobileNumber(patient.MobileNumber);
                    patient.IsAdult = Helpers.IsAdult(patient.DoB);
                   // await _patientRepository.AddPatient(patient);

                    // Send Email to patient, if has email address
                    string emailBody = GenerateEmailBody(patient, Request);

                    if (emailBody != null)
                    {
                        Email email = new Email
                        {
                            From = _emailFrom,
                            To = patient.EmailAddress,
                            Subject = $"Registration Successful - {patient.FullName}",
                            Body = emailBody
                        };
                        await EmailProcessor.SendEmailAsync(email, _emailUserName, _emailPassword);
                    }

                    string patientUri = string.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.Path.Value, patient.ID.ToString());

                    return Created(patientUri, patient);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message) { StatusCode = StatusCodes.Status404NotFound };
            }
        }

        private string GenerateEmailBody(Patient patient, HttpRequest request)
        {
            var certificateUrl = string.Format("{0}://{1}/api/DiagnosisReports/{2}", request.Scheme, request.Host, patient.ID.ToString());
            var qrCodeUrl = $"https://chart.googleapis.com/chart?cht=qr&chs={500}x{500}&chl={certificateUrl}";

            WebRequest webRequest = WebRequest.Create(qrCodeUrl);
            using WebResponse webResponse = webRequest.GetResponse();
            using Stream remoteStream = webResponse.GetResponseStream();
            using StreamReader streamReader = new StreamReader(remoteStream);

            var qrCodeImage = Image.FromStream(remoteStream);
            var qrCodeImageName = $"{patient.ID}.png";
            var qrCodesPath = Path.Combine(Directory.GetCurrentDirectory(), "QRCodes", qrCodeImageName);
            qrCodeImage.Save(qrCodesPath);

            if (patient.EmailAddress != null)
            {
                string body = string.Empty;
                var imgSource = $"{request.Scheme}://{request.Host}{Url.Content($"~/MyCovid19Certificate/{qrCodeImageName}")}";
                using (StreamReader reader = new StreamReader("./Resources/RegistrationsTemplate.html"))
                {
                    body = reader.ReadToEnd();
                };
                body = body.Replace("{PatientFirstName}", patient.FirstName);
                body = body.Replace("{PatientFullName}", patient.FullName.ToUpper());
                body = body.Replace("{PatientIdNumber}", patient.IdNumber.ToUpper());
                body = body.Replace("{RegistrationDate}", patient.DateRegistered.ToString("dddd, dd MMMM yyyy hh:mm tt"));
                body = body.Replace("{LinkToCertificate}", imgSource.ToString());
                body = body.Replace("{BaseUrl}", $"{Request.Scheme}://{Request.Host}");
                body = body.Replace("{QRCode}", imgSource);

                return body;
            }

            return null;
        }

        // PUT: api/patients/5
        [HttpPut]
        [Route("api/[controller]/{patientId}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdatePatientAsync(Guid patientId, [FromBody] Patient patient)
        {
            if (patient == null || patientId == Guid.Empty)
            {
                return BadRequest();
            }
            // Check if patient exists
            var updatedPatient = await _patientRepository.GetPatient(patientId);

            if (updatedPatient == null)
            {
                return BadRequest();
            }

            updatedPatient.Constituency = patient.Constituency ?? updatedPatient.Constituency;
            updatedPatient.County = patient.County ?? updatedPatient.County;
            updatedPatient.DoB = patient.DoB;
            updatedPatient.FirstName = patient.FirstName ?? updatedPatient.FirstName;
            updatedPatient.Gender = patient.Gender;
            updatedPatient.IdNumber = patient.IdNumber ?? updatedPatient.IdNumber;
            updatedPatient.LastName = patient.LastName ?? updatedPatient.LastName;
            updatedPatient.MiddleName = patient.MiddleName ?? updatedPatient.MiddleName;
            updatedPatient.MobileNumber = patient.MobileNumber ?? updatedPatient.MobileNumber;
            updatedPatient.Nationality = patient.Nationality;
            updatedPatient.Ward = patient.Ward;

            await _patientRepository.UpdatePatient(updatedPatient);

            return Ok(updatedPatient);
        }

        // DELETE: api/patients/5
        [HttpDelete]
        [Route("api/[controller]/{patientId}")]
        public async Task<IActionResult> DeactivatePatientAsync(Guid patientId)
        {
            if (patientId == Guid.Empty)
            {
                return BadRequest();
            }
            // Check if patient exists
            var patient = await _patientRepository.GetPatient(patientId);
            ResponseViewModel response;

            if (patient == null)
            {
                response = new ResponseViewModel
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "Patient not found."
                };
                return NotFound(response);
            }

            await _patientRepository.DeactivatePatient(patientId);

            response = new ResponseViewModel
            {
                Code = StatusCodes.Status204NoContent,
                Message = "Patient deleted successfully."
            };

            return  new JsonResult(response) { StatusCode = StatusCodes.Status204NoContent };
        }
    }
}
