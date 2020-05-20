using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static CoviDoc.Models.Enums;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace CoviDoc.Models
{
    /// <summary>
    /// Cannot update the IdNumber/Names/DateRegistered of patient
    /// </summary>
    public class Patient
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Required]
        [Display(Name = "ID/Passport No.")]
        public string IdNumber { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; } = Gender.Uknown;

        public string Nationality { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        public string MobileNumber { get; set; }
        public string County { get; set; }
        public string Constituency { get; set; }
        public string Ward { get; set; }
        public bool IsAdult { get; set; }
        public bool IsActive { get; set; } = true;

        [DataType(DataType.DateTime)]
        [Display(Name = "Date Of Registration")]
        public DateTime DateRegistered { get; set; }

    }
}
