using CoviDoc.Common;
using CoviDoc.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
// using System.Text.Json;

namespace CoviDoc.Models.Mocks
{
    public class MockPatientRepository : IPatientRepository
    {
        private const string patientsFilePath = ".//Resources//MockPatients.json";
        private List<Patient> _patients;

        public MockPatientRepository()
        {
            FetchPatients();
        }

        public void AddPatient(Patient patient)
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
                PostPatients(_patients);
            }
            else
            {
                throw new InvalidOperationException($"A patient with a similar Id/Passport number and name is already registered. {patient.IdNumber} - {patient.FullName}");
            }
        }

        public Patient GetPatient(Guid? id)
        {
            if (id == null)
            {
                return null;
            }
            return _patients.FirstOrDefault(p => p.ID.Equals(id));
        }
        public List<Patient> GetPatients(string idNumber)
        {
            // Patients with matching IdNumbers could be parent/child
            return _patients.FindAll(p => p.IdNumber.Equals(idNumber))
                            .OrderByDescending(p => p.DateRegistered)
                            .ToList();
        }

        public List<Patient> GetPatients()
        {
            return _patients.OrderByDescending(p => p.DateRegistered).ToList();
        }

        public void TestPatient(Patient patient, DiagnosisReport encounter)
        {
            throw new NotImplementedException();
        }

        public void UpdatePatient(Patient patient)
        {
            int index = _patients.FindIndex(p => p.ID.Equals(patient.ID));

            // Checked that we are not updating to an already existing Id/Passport Number
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
                PostPatients(_patients);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        private void FetchPatients()
        {
            try
            {
                string jsonString = ReadFromFile(patientsFilePath).GetAwaiter().GetResult();
                _patients = JsonConvert.DeserializeObject<List<Patient>>(jsonString, new IsoDateTimeConverter());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void PostPatients(List<Patient> patients)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(patients, Formatting.Indented);
                WriteToFile(jsonString, patientsFilePath).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> ReadFromFile(string filePathSource)
        {
            using StreamReader streamReader = new StreamReader(filePathSource);
            return await streamReader.ReadToEndAsync();
        }

        public async Task WriteToFile(string fileContents, string filePathSource)
        {
            using StreamWriter streamWriter = new StreamWriter(filePathSource);
            await streamWriter.WriteLineAsync(fileContents);
        }
    }
}
