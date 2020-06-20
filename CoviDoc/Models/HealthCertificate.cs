using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoviDoc.Models.Enums;

namespace CoviDoc.Models
{
    public class HealthCertificate
    {
        public string CertificateId { get; set; }
        public Guid? DiagnosisReportId { get; set; }
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public string MobileNumber { get; set; }
        public bool IsAdult { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TestStatus TestStatus { get; set; } = TestStatus.Pending;
        public string TestCentre { get; set; } = "Pending";
        public DateTime TestDate { get; set; }
        public string BaseUrlLocation { get; set; }
        public List<Guid> ChildrenIds { get; set; }
    }
}
