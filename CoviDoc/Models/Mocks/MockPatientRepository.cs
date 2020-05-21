using CoviDoc.Models.Interfaces;
using FileService;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Mocks
{
    public class MockPatientRepository : IPatientRepository
    {
        private readonly IFileUtility _fileUtility;
        private const string patientsFilePath = ".//Resources//MockPatients.json";
        private List<Patient> _patients;

        public MockPatientRepository(IFileUtility fileUtility)
        {
            _fileUtility = fileUtility;
            FetchPatients().GetAwaiter().GetResult();
        }

        public async Task AddPatient(Patient patient)
        {
            bool adultExists = _patients.Exists(p => p.IdNumber.Equals(patient.IdNumber) && p.IsAdult == true);

            if (patient.IsAdult && adultExists)
            {
                throw new InvalidOperationException($"An adult patient is already registered with the Id/Passport number. {patient.IdNumber} - {patient.FullName}");
            }

            int index = _patients.FindIndex(p => p.IdNumber.Equals(patient.IdNumber) &&
                                            p.FirstName.Equals(patient.FirstName, StringComparison.OrdinalIgnoreCase) &&
                                            p.MiddleName.Equals(patient.MiddleName, StringComparison.OrdinalIgnoreCase) &&
                                            p.LastName.Equals(patient.LastName, StringComparison.OrdinalIgnoreCase));


            if (index == -1) // patient doesn't exist
            {
                _patients.Add(patient);
                await PostPatients(_patients);
            }
            else
            {
                throw new InvalidOperationException($"A patient with a similar Id/Passport number and name is already registered. {patient.IdNumber} - {patient.FullName}");
            }
        }

        public async Task<Patient> GetPatient(Guid? patientId)
        {
            if (patientId == null)
            {
                return null;
            }
            return _patients.FirstOrDefault(p => p.ID.Equals(patientId));
        }
        public async Task<List<Patient>> GetPatients(string idNumber)
        {
            // Patients with matching IdNumbers could be parent/child
            return _patients.FindAll(p => p.IdNumber.Equals(idNumber) && p.IsActive)
                            .OrderByDescending(p => p.DateRegistered)
                            .ToList();
        }

        public async Task<List<Patient>> GetPatients()
        {
            return _patients.Where(p => p.IsActive)
                            .OrderByDescending(p => p.DateRegistered)
                            .ToList();
        }

        public async Task UpdatePatient(Patient patient)
        {
            int index = _patients.FindIndex(p => p.ID.Equals(patient.ID) && p.IsActive);

            // Check that we are not updating to an already existing Id/Passport Number
            bool adultExists = _patients.Exists(p => p.IdNumber.Equals(patient.IdNumber, StringComparison.OrdinalIgnoreCase) &&
                                                p.IsAdult == true && p.ID != patient.ID);

            // Check that we are not making this patient parent-less
            bool hasParent = _patients.Exists(p => p.IdNumber.Equals(patient.IdNumber, StringComparison.OrdinalIgnoreCase) &&
                                             p.IsAdult == true);

            if (!adultExists || !hasParent)
            {
                throw new InvalidOperationException();
            }

            if (index > -1) // patient exists
            {
                _patients.RemoveAt(index);
                _patients.Insert(index, patient);
                await PostPatients(_patients);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public async Task DeactivatePatient(Patient patient)
        {
            int index = _patients.FindIndex(p => p.ID.Equals(patient.ID));

            if (index > -1)
            {
                patient.IsActive = false;
            }

            await PostPatients(_patients);
        }

        private async Task FetchPatients()
        {
            try
            {
                string jsonString = await _fileUtility.ReadFromFileAsync(patientsFilePath);
                _patients = JsonConvert.DeserializeObject<List<Patient>>(jsonString, new IsoDateTimeConverter());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task PostPatients(List<Patient> patients)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(patients, Formatting.Indented);
                await _fileUtility.WriteToFileAsync(jsonString, patientsFilePath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
