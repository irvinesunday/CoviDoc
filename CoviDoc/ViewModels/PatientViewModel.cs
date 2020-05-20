using CoviDoc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.ViewModels
{
    public class PatientViewModel
    {
        public Patient Patient { get; set; }
        public DiagnosisReport DiagnosisReport { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Counties { get; set; }
        public IEnumerable<SelectListItem> Constituencies { get; set; }
        public IEnumerable<SelectListItem> Wards { get; set; }
        public TestCentre TestCentre { get; set; }
    }
}
