using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public static class Enums
    {
        public enum Gender
        {
            [EnumMember(Value = "Male")]
            [Display(Name = "Male")]
            Male,

            [EnumMember(Value = "Female")]
            [Display(Name = "Female")]
            Female,

            [EnumMember(Value = "Other")]
            [Display(Name = "Other")]
            Other,

            [EnumMember(Value = "Uknown")]
            [Display(Name = "Uknown")]
            Uknown
        }

        public enum TestStatus
        {
            [Display(Name = "Awaiting Results")]
            [Description("Awaiting Results")]
            AwaitingResults,
            [Display(Name = "Negative")]
            Negative,
            [Display(Name = "Positive")]
            Positive
        }

    }
}
