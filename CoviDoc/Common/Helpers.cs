using Newtonsoft.Json.Converters;
using System;
using System.Linq;

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

            string newMobileNumber;
            if (mobileNumber.StartsWith('+'))
            {
                // Remove country code if exists
                newMobileNumber = $"0{mobileNumber.Remove(0, countryCode.Length)}";
                return newMobileNumber;
            }
            else if(mobileNumber.StartsWith('0'))
            {
                // Add country code if none exists
                newMobileNumber = $"{countryCode}{mobileNumber.Remove(0, 1)}";
                return newMobileNumber;
            }
            else
            {
                // Add country code if none exists
                newMobileNumber = $"{countryCode}{mobileNumber}";
                return newMobileNumber;
            }
        }

        public static string GenerateCertificateId(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
