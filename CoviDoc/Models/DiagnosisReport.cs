using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using static CoviDoc.Models.Enums;
namespace CoviDoc.Models
{
    public class DiagnosisReport
    {
        public Guid? DiagnosisReportId { get; set; }
        public Guid? PatientId { get; set; }
        public string TestCentre { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "Test Result")]
        public TestStatus TestStatus { get; set; } = TestStatus.AwaitingResults;
        [Display(Name = "Date Tested")]
        public DateTime DateTested { get; set; }
    }
}
