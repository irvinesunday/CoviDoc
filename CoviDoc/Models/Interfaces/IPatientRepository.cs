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
        void TestPatient(Patient patient, DiagnosisReport encounter);
        List<Patient> GetPatients(string idNumber);
        List<Patient> GetPatients();
        Patient GetPatient(Guid? id);
    }
}
