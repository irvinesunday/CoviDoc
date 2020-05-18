using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoviDoc.Models.Enums;

namespace CoviDoc.Models
{
    public class PatientTestViewModel
    {
        public string PatientName { get; set; }
        public string IdNumber { get; set; }
        public DateTime DoB { get; set; }
        public bool IsAdult { get; set; }
        public TestStatus TestStatus { get; set; } = TestStatus.Pending;
        public List<TestCentre> TestCentres { get; set; }
    }
}
