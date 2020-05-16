using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public class County
    {
        public string ID { get; set; }
        public string CountyName { get; set; }
        public List<Ward> Wards { get; set; }
    }
}
