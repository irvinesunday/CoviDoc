using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Interfaces
{
    public interface IPatientRepository
    {
        Task AddPatient(Patient patient);
        Task UpdatePatient(Patient patient);
        Task<List<Patient>> GetPatients(string idNumber, string mobileNumber);
        Task<List<Patient>> GetPatients(string idNumber);
        Task<List<Patient>> GetPatients();
        Task<Patient> GetPatient(Guid? patientId);
        Task DeactivatePatient(Guid? patientId);
    }
}
