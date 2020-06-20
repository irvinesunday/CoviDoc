using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Interfaces
{
    public interface IHealthCertificateRepository
    {
        Task AddHealthCertificate(HealthCertificate healthCertificate);
        List<HealthCertificate> GetHealthCertificates(string idNumber, string mobileNumber);
        public HealthCertificate GetHealthCertificate(Guid patientId);
    }
}
