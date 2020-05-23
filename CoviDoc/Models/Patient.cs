using CoviDoc.Common;
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
    /// Cannot update the DateRegistered of patient
    /// </summary>
    public class Patient
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$",
            ErrorMessage = "Must start with a capital letter and cannot include special characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$",
            ErrorMessage ="Must start with a capital letter and cannot include special characters.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$",
            ErrorMessage = "Must start with a capital letter and cannot include special characters.")]
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

        private string _mobileNumber;
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
            ErrorMessage = "Not a valid mobile number.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber
        {
            get => _mobileNumber;
            set => _mobileNumber = Helpers.FormatMobileNumber(value);
        }
        public string County { get; set; }
        public string Constituency { get; set; }
        public string Ward { get; set; }
        public bool IsAdult { get; set; }
        public bool IsActive { get; set; } = true;

        [DataType(DataType.Date)]
        [Display(Name = "Date of Reg.")]
        public DateTime DateRegistered { get; set; }

    }
}
