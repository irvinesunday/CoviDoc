using Newtonsoft.Json.Converters;
using System;

namespace CoviDoc.Common
{
    public static class Helpers
    {
        public static bool IsAdult(DateTime dateOfBirth)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month ||
               (now.Month == dateOfBirth.Month &&
                now.Day < dateOfBirth.Day))
            {
                age--;
            }
            return age >= 18;
        }

        public static string GetConstituencyId(string constituencyName)
        {
            return constituencyName.Replace(" ", string.Empty);
        }
    }
}
