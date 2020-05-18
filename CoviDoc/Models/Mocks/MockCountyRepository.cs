using CoviDoc.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Mocks
{
    public class MockCountyRepository :ICountyRepository
    {
        private const string countiesFilePath = ".//Resources//Counties.json";
        private List<County> _counties;

        public MockCountyRepository()
        {
            FetchCounties();
        }

        public List<County> GetCounties()
        {
            return _counties;
        }

        private void FetchCounties()
        {
            try
            {
                string jsonString = ReadFromFile(countiesFilePath).GetAwaiter().GetResult();
                _counties = JsonConvert.DeserializeObject<List<County>>(jsonString);
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
