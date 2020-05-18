using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models
{
    public static class Enums
    {
        public enum Gender
        {
            Male,
            Female,
            Other,
            Uknown
        }

        public enum TestStatus
        {
            Pending,
            Positive,
            Negative
        }

    }
}
