using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Interfaces
{
    public interface IPatientRepository
    {
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void TestPatient(Patient patient, Encounter encounter);
        List<Patient> GetPatient(string idNumber);
    }
}
