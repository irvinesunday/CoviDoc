using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using static CoviDoc.Models.Enums;
namespace CoviDoc.Models
{
    public class DiagnosisReport
    {
        public Guid? DiagnosisReportId { get; set; }
        public Guid? PatientId { get; set; }
        public Guid TestCentreId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TestStatus TestStatus { get; set; } = Enums.TestStatus.Pending;
        public DateTime? DateTested { get; set; }
    }
}
