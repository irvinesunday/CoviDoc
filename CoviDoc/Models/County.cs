using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public class County
    {
        public int ID { get; set; }
        public string CountyName { get; set; }
        public List<string> Constituencies { get; set; }
        public List<string> Wards { get; set; }
    }
}
