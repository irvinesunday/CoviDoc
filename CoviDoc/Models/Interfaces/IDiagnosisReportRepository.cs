using System;
using System.Collections.Generic;

namespace CoviDoc.Models.Interfaces
{
    public interface IDiagnosisReportRepository
    {
        void AddDiagnosisReport(DiagnosisReport diagnosisReport);
        List<DiagnosisReport> GetDiagnosisReports();
        DiagnosisReport GetDiagnosisReport(Guid? patientId);
    }
}
