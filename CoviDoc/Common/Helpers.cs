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

        /// <summary>
        /// Replaces country code with '0' if exists and replaces 0 with country code, if exists.
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public static string FormatMobileNumber(string mobileNumber, string countryCode = Constants.CountryCodes.KENYA)
        {
            if (string.IsNullOrEmpty(mobileNumber))
            {
                return null;
            }

            if (mobileNumber.StartsWith('+'))
            {
                // Remove country code if exists
                return $"0{mobileNumber.Remove(0, countryCode.Length)}";
            }
            // Add country code if none exists
            return $"{countryCode}{mobileNumber.Remove(0, 1)}";
        }
    }
}
