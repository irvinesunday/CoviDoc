using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public class Location
    {
        public class Country
        {
            public string[] Countries { get; set; }
        }

        public class County
        {
            [JsonProperty("ID")]
            public int CountyId { get; set; }
            [JsonProperty("CountyName")]
            public string CountyName { get; set; }
            [JsonProperty("Constituencies")]
            public IEnumerable<Constituency> Constituencies { get; set; }
        }

        public class Constituency
        {
            [JsonIgnore]
            public string Constituencyid => ConstituencyName.Replace(" ", string.Empty);
            [JsonProperty(PropertyName = "ConstituencyName")]
            public string ConstituencyName { get; set; }
            [JsonProperty(PropertyName = "Wards")]
            public string[] Wards { get; set; }
        }
    }
}
