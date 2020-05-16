using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public class Patient
    {
        public enum GenderDefinitions
        {
            Male,
            Female
        }
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public DateTime DoB { get; set; }
        public GenderDefinitions Gender { get; set; }
        public string Nationality { get; set; }
        public bool IsAdult { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
