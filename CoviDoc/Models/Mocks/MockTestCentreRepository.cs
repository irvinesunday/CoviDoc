using CoviDoc.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Mocks
{
    public class MockTestCentreRepository : ITestCentreRepository
    {
        private const string testCentresFilePath = ".//Resources//MockTestCentres.json";
        private List<TestCentre> _testCentres;

        public MockTestCentreRepository()
        {
            FetchTestCentres();
        }

        public List<TestCentre> GetTestCentres()
        {
            return _testCentres;
        }

        private void FetchTestCentres()
        {
            try
            {
                string jsonString = ReadFromFile(testCentresFilePath).GetAwaiter().GetResult();
                _testCentres = JsonConvert.DeserializeObject<List<TestCentre>>(jsonString);
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
