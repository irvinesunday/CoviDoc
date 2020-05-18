﻿using CoviDoc.Common;
using CoviDoc.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoviDoc.Models
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
            int index = _patients.FindIndex(p => p.IdNumber.Equals(patient.IdNumber) &&
                                            p.FirstName.Equals(patient.FirstName, StringComparison.OrdinalIgnoreCase) &&
                                            p.MiddleName.Equals(patient.MiddleName, StringComparison.OrdinalIgnoreCase) &&
                                            p.LastName.Equals(patient.LastName, StringComparison.OrdinalIgnoreCase));
            if (index == -1) // patient doesn't exist
            {
                _patients.Add(patient);
            }
        }

        public List<Patient> GetPatient(string idNumber)
        {
            return _patients.FindAll(p => p.IdNumber.Equals(idNumber)); // patients with matching IdNumbers could be parent/child
        }

        public void TestPatient(Patient patient, Encounter encounter)
        {
            throw new NotImplementedException();
        }

        public void UpdatePatient(Patient patient)
        {
            int index = _patients.FindIndex(p => p.IdNumber.Equals(patient.IdNumber) &&
                                            p.FirstName.Equals(patient.FirstName, StringComparison.OrdinalIgnoreCase) &&
                                            p.MiddleName.Equals(patient.MiddleName, StringComparison.OrdinalIgnoreCase) &&
                                            p.LastName.Equals(patient.LastName, StringComparison.OrdinalIgnoreCase));
            if (index > -1) // patient exists
            {
                _patients.RemoveAt(index);
                _patients.Insert(index, patient);
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