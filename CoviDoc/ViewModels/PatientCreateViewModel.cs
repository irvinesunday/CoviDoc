using CoviDoc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.ViewModels
{
    public class PatientCreateViewModel
    {
        public Patient Patient { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Counties { get; set; }
        //public string SelectedCounty { get; set; }

        public IEnumerable<SelectListItem> Constituencies { get; set; }
        //public string SelectedConstituency { get; set; }

        public IEnumerable<SelectListItem> Wards { get; set; }
    }
}
