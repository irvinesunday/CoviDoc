using CoviDoc.Models.Interfaces;
using FileService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            FetchDiagnosisReports();
        }

        public void AddDiagnosisReport(DiagnosisReport diagnosisReport)
        {
            if (diagnosisReport == null)
            {
                throw new ArgumentNullException();
            }
            _diagnosisReports.Add(diagnosisReport);
            PostDiagnosisReport();
        }

        public List<DiagnosisReport> GetDiagnosisReports()
        {
            return _diagnosisReports;
        }

        public DiagnosisReport GetDiagnosisReport(Guid? patientId)
        {
            if (patientId == null)
            {
                return null;
            }
            return _diagnosisReports.LastOrDefault(x => x.PatientId == patientId);
        }

        private void FetchDiagnosisReports()
        {
            try
            {
                string jsonString = _fileUtility.ReadFromFileAsync(diagnosisReportFilePath).GetAwaiter().GetResult();
                _diagnosisReports = JsonConvert.DeserializeObject<List<DiagnosisReport>>(jsonString);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void PostDiagnosisReport()
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(_diagnosisReports, Formatting.Indented);
                _fileUtility.WriteToFileAsync(jsonString, diagnosisReportFilePath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
