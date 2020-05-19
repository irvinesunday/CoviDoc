using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public static class Enums
    {
        public enum Gender
        {
            [Display(Name = "Male")]
            Male,
            [Display(Name = "Female")]
            Female,
            [Display(Name = "Other")]
            Other,
            [Display(Name = "Uknown")]
            Uknown,
        }

        public enum TestStatus
        {
            [Display(Name = "Pending")]
            Pending,
            [Display(Name = "Negative")]
            Negative,
            [Display(Name = "Positive")]
            Positive
        }

    }
}
