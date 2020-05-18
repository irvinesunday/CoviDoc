using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoviDoc.Models.Enums;

namespace CoviDoc.Models
{
    public class CovidTestReport
    {
        public Guid ID { get; set; }
        public Guid PatientId { get; set; }
        public TestStatus Status { get; set; } = TestStatus.Pending;
        public TestCentre TestCentre { get; set; }
        public DateTime TestDate { get; set; }
    }
}
