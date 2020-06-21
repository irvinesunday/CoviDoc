using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CoviDoc.Models.Enums;

namespace CoviDoc.Models
{
    public class HealthCertificate
    {
        [Display(Name = "Certificate ID")]
        public string CertificateId { get; set; }
        public Guid? DiagnosisReportId { get; set; }
        public Guid PatientId { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "ID/Passport No.")]
        public string IdNumber { get; set; }
        public string MobileNumber { get; set; }
        public bool IsAdult { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]

        [Display(Name = "Test Status")]
        public TestStatus TestStatus { get; set; } = TestStatus.AwaitingResults;

        [Display(Name = "Test Centre")]
        public string TestCentre { get; set; } = "Pending";

        public DateTime TestDate { get; set; } = DateTime.UtcNow.AddHours(3);

        [Display(Name = "Date Tested")]
        public string TestDateToString
        { get => TestDate.ToString("dddd, dd MMMM yyyy hh:mm tt"); }

        public string BaseUrlLocation { get; set; }
        public List<Guid> ChildrenIds { get; set; }
    }
}
