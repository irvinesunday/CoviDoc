using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public class TestCentre
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int WardId { get; set; }
        public Ward Ward { get; set; }
        public bool IsActive { get; set; }
    }
}
