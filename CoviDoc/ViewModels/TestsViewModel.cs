using CoviDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoviDoc.Models.Enums;

namespace CoviDoc.ViewModels
{
    public class TestsViewModel
    {
        public Patient Patient { get; set; }
        public TestCentre TestCentre { get; set; }
        public DiagnosisReport DiagnosisReport { get; set; }
    }
}
