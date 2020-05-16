using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public class Encounter
    {
        public Guid ID { get; set; }
        public string Status { get; set; }
        public Patient Patient { get; set; }
        public TestCentre TestCentre { get; set; }
        public Guid PractitionerId { get; set; }
        public string PractictionerUPN { get; set; }
        public string PractitionerName { get; set; }
        public DateTime DateTested { get; set; }
    }
}
