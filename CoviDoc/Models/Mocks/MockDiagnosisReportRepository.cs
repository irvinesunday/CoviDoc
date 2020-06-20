using CoviDoc.Models.Interfaces;
using FileService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Mocks
{
    public class MockDiagnosisReportRepository : IDiagnosisReportRepository
    {
        private const string diagnosisReportFilePath = ".//Resources//MockDiagnosisReports.json";
        private List<DiagnosisReport> _diagnosisReports = new List<DiagnosisReport>();
        private readonly IFileUtility _fileUtility;

        public MockDiagnosisReportRepository(IFileUtility fileUtility)
        {
            _fileUtility = fileUtility;
            FetchDiagnosisReports().GetAwaiter().GetResult();
        }

        public async Task AddDiagnosisReport(DiagnosisReport diagnosisReport)
        {
            if (diagnosisReport == null)
            {
                throw new ArgumentNullException();
            }
            _diagnosisReports.Add(diagnosisReport);
            await PostDiagnosisReport();
        }

        public async Task<List<DiagnosisReport>> GetDiagnosisReports()
        {
            // Get the first instances of the patient results
            var diagnosisReports = _diagnosisReports.OrderByDescending(x => x.DateTested)
                                                    .GroupBy(x => x.PatientId)
                                                    .Select(x => x.First())
                                                    .ToList();
            return diagnosisReports;
        }

        public async Task<DiagnosisReport> GetDiagnosisReport(Patient patient)
        {
            if (patient == null)
            {
                return null;
            }
            return _diagnosisReports.LastOrDefault(x => x.PatientId == patient.ID);
        }

        public async Task<List<DiagnosisReport>> GetDiagnosisReports(List<Patient> patients)
        {
            if(patients == null || !patients.Any())
            {
                return null;
            }

            var diagnosisReports = new List<DiagnosisReport>();

            foreach(var patient in patients)
            {
                var item = _diagnosisReports.LastOrDefault(x => x.PatientId == patient.ID);
                if (item != null)
                {
                    diagnosisReports.Add(item);
                }
            }

            return diagnosisReports;
        }

        public async Task<DiagnosisReport> GetDiagnosisReport(Guid? reportId)
        {
            if (reportId == null)
            {
                return null;
            }

            return _diagnosisReports.FirstOrDefault(x => x.DiagnosisReportId == reportId);
        }

        private async Task FetchDiagnosisReports()
        {
            try
            {
                string jsonString = await _fileUtility.ReadFromFileAsync(diagnosisReportFilePath);
                _diagnosisReports = JsonConvert.DeserializeObject<List<DiagnosisReport>>(jsonString);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task PostDiagnosisReport()
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(_diagnosisReports, Formatting.Indented);
                await _fileUtility.WriteToFileAsync(jsonString, diagnosisReportFilePath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
