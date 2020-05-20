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
    public class PatientTestViewModel
    {
        public Guid PatientId { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Display(Name = "Id/Passport Number")]
        public string PatientIdNumber { get; set; }

        [Display(Name = "Gender")]
        public Gender PatientGender { get; set; }

        [Required]
        [Display(Name = "Test Status")]
        public TestStatus TestStatus { get; set; }

        [Required]
        [Display(Name = "Test Centre Name")]
        public Guid TestCentreId { get; set; }

        public IEnumerable<SelectListItem> TestCentres { get; set; }
    }
}
