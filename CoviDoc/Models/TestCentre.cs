using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public class TestCentre
    {
        public Guid ID { get; set; }
        [Display(Name = "Test Centre Name")]
        public string Name { get; set; }
        public string County { get; set; }
        public string Constituency { get; set; }
        [Display(Name = "Test Centre Location")]
        public string Ward { get; set; }
        public bool IsActive { get; set; }
    }
}
