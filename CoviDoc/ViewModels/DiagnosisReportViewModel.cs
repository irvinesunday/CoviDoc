using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static CoviDoc.Models.Enums;

namespace CoviDoc.Models
{
    public class DiagnosisReportViewModel
    {
        public Guid PatientId { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Display(Name = "Id/Passport Number")]
        public string PatientIdNumber { get; set; }

        public string MobileNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "Gender")]
        public Gender PatientGender { get; set; }

        public bool IsAdult { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "Test Status")]
        public TestStatus TestStatus { get; set; } = TestStatus.AwaitingResults;

        [Required]
        [Display(Name = "Test Centre Name")]
        public string TestCentre { get; set; } = "N/A";

        [Display(Name = "Date Tested")]
        public DateTime DateTested { get; set; } = DateTime.UtcNow.AddHours(3);
    }
}
