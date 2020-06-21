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
    public class MockHealthCertificateRepository : IHealthCertificateRepository
    {
        private readonly IFileUtility _fileUtility;
        private const string _healthCertificatesFilePath = ".//Resources//MockHealthCertificates.json";
        private List<HealthCertificate> _healthCertificates;

        public MockHealthCertificateRepository(IFileUtility fileUtility)
        {
            _fileUtility = fileUtility;
            FetchHealthCertificates().GetAwaiter().GetResult();
        }

        public async Task AddHealthCertificate(HealthCertificate healthCertificate)
        {
            if (healthCertificate == null)
            {
                throw new ArgumentNullException();
            }

            // Check if there exists a Health Certificate with given patient Id
            int certificateIndex = _healthCertificates.FindIndex(x => x.PatientId == healthCertificate.PatientId);

            if (certificateIndex >= 0)
            {
                // Remove previous health certificate
                _healthCertificates.RemoveAt(certificateIndex);
            }

            // Add the new certificate
            _healthCertificates.Add(healthCertificate);

            await PostHealthCertificates(_healthCertificates);
        }

        public List<HealthCertificate> GetHealthCertificates(string idNumber, string mobileNumber)
        {
            if (string.IsNullOrEmpty(idNumber))
            {
                return null;
            }

            try
            {
                return _healthCertificates.FindAll(x => x.IdNumber.Equals(idNumber, StringComparison.OrdinalIgnoreCase) &&
                                                    x.MobileNumber.Equals(mobileNumber));
            }
            catch
            {
                return null;
            }
        }

        public HealthCertificate GetHealthCertificate(Guid patientId)
        {
            if (patientId == Guid.Empty)
            {
                return null;
            }

            try
            {
                return _healthCertificates.LastOrDefault(x => x.PatientId == patientId);
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdateHealthCertificate(HealthCertificate healthCertificate)
        {
            if (healthCertificate == null)
            {
                throw new ArgumentNullException();
            }

            var index = _healthCertificates.FindIndex(x => x.CertificateId == healthCertificate.CertificateId);

            if (index > -1) // certificate exists
            {
                _healthCertificates.RemoveAt(index);
                _healthCertificates.Insert(index, healthCertificate);
                await PostHealthCertificates(_healthCertificates);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public async Task PostHealthCertificates(List<HealthCertificate> healthCertificates)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(healthCertificates, Formatting.Indented);
                await _fileUtility.WriteToFileAsync(jsonString, _healthCertificatesFilePath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task FetchHealthCertificates()
        {
            try
            {
                string jsonString = await _fileUtility.ReadFromFileAsync(_healthCertificatesFilePath);
                _healthCertificates = JsonConvert.DeserializeObject<List<HealthCertificate>>(jsonString, new IsoDateTimeConverter()) ?? new List<HealthCertificate>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
