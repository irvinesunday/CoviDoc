using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoviDoc.Models.Interfaces
{
    public interface IDiagnosisReportRepository
    {
        Task AddDiagnosisReport(DiagnosisReport diagnosisReport);
        Task<List<DiagnosisReport>> GetDiagnosisReports();
        Task<DiagnosisReport> GetDiagnosisReport(Guid? patientId);
    }
}
