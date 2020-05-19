using CoviDoc.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Mocks
{
    public class MockLocationRepository :ILocationRepository
    {
        private const string countiesFilePath = ".//Resources//Counties.json";
        private const string countriesFilePath = ".//Resources//Countries.json";
        private List<Location.County> _counties;
        private string[] _countries;

        public MockLocationRepository()
        {
            FetchCounties();
            FetchCountries();
        }

        public IEnumerable<SelectListItem> GetCountries()
        {
            List<SelectListItem> counties = _countries.OrderBy(n => n)
                                                     .Select(n =>
                                                     new SelectListItem
                                                     {
                                                         Value = n,
                                                         Text = n
                                                     }).ToList();
            var countyTip = new SelectListItem()
            {
                Value = null,
                Text = "-- Select Country of Nationality --"
            };
            counties.Insert(0, countyTip);
            return new SelectList(counties, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetCounties()
        {
            List<SelectListItem> counties = _counties.OrderBy(n => n.CountyId)
                                                     .Select(n =>
                                                     new SelectListItem
                                                     {
                                                         Value = n.CountyId.ToString(),
                                                         Text = n.CountyName
                                                     }).ToList();
            var countyTip = new SelectListItem()
            {
                Value = null,
                Text = "-- Select County --"
            };
            counties.Insert(0, countyTip);
            return new SelectList(counties, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetConstituencies()
        {
            List<SelectListItem> constituencies = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = ""
                }
            };
            return constituencies;
        }

        public IEnumerable<SelectListItem> GetConstituencies(int countyId)
        {
            if (countyId > 0)
            {
                var county = _counties.FirstOrDefault(x => x.CountyId == countyId);

                if (county != null)
                {
                    IEnumerable<SelectListItem> constituencies = county.Constituencies
                                                                       .OrderBy(n => n.ConstituencyName)
                                                                       .Select(n =>
                                                                            new SelectListItem
                                                                            {
                                                                                Value = n.Constituencyid,
                                                                                Text = n.ConstituencyName
                                                                            }).ToList();
                    return new SelectList(constituencies, "Value", "Text");
                }
            }
            return null;
        }

        public IEnumerable<SelectListItem> GetWards()
        {
            List<SelectListItem> wards = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = ""
                }
            };
            return wards;
        }

        public IEnumerable<SelectListItem> GetWards(int countyId, string constituencyId)
        {
            if (countyId > 0 && !string.IsNullOrEmpty(constituencyId))
            {
                var county = _counties.FirstOrDefault(x => x.CountyId == countyId);

                if (county != null)
                {
                    var constituency = county.Constituencies.FirstOrDefault(x => x.Constituencyid == constituencyId);

                    IEnumerable<SelectListItem> wards = constituency.Wards.OrderBy(n => n)
                                                                          .Select(n =>
                                                                            new SelectListItem
                                                                            {
                                                                                Value = n,
                                                                                Text = n
                                                                            }).ToList();
                    return new SelectList(wards, "Value", "Text");
                }
            }

            return null;
        }

        private void FetchCounties()
        {
            string jsonString = ReadFromFile(countiesFilePath).GetAwaiter().GetResult();
            _counties = JsonConvert.DeserializeObject<List<Location.County>>(jsonString);
        }

        private void FetchCountries()
        {
            try
            {

                string jsonString = ReadFromFile(countriesFilePath).GetAwaiter().GetResult();
                _countries = JsonConvert.DeserializeObject<string[]>(jsonString);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<string> ReadFromFile(string filePathSource)
        {
            using StreamReader streamReader = new StreamReader(filePathSource);
            return await streamReader.ReadToEndAsync();
        }
    }
}
