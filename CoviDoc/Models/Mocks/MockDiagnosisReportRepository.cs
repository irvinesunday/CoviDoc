using CoviDoc.Models.Interfaces;
using FileService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoviDoc.Models.Mocks
{
    public class MockDiagnosisReportRepository : IDiagnosisReportRepository
    {
        private const string diagnosisReportFilePath = ".//Resources//MockDiagnosisReports.json";
        private List<DiagnosisReport> _diagnosisReports;
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
            return _diagnosisReports;
        }

        public async Task<DiagnosisReport> GetDiagnosisReport(Guid? patientId)
        {
            if (patientId == null)
            {
                return null;
            }
            return _diagnosisReports.LastOrDefault(x => x.PatientId == patientId);
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
