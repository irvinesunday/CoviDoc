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
        public Guid TestCentreId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "Test Result")]
        public TestStatus TestStatus { get; set; } = Enums.TestStatus.Pending;
        [Display(Name = "Date Tested")]
        public DateTime? DateTested { get; set; }
    }
}
