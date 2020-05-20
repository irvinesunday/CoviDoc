using CoviDoc.Models.Interfaces;
using FileService;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IFileUtility _fileUtility;

        public MockTestCentreRepository(IFileUtility fileUtility)
        {
            _fileUtility = fileUtility;
            FetchTestCentres();
        }

        public IEnumerable<SelectListItem> GetTestCentres()
        {
            List<SelectListItem> testCentres =_testCentres.OrderBy(x => x.Name)
                                                          .Select(n =>
                                                           new SelectListItem
                                                           {
                                                               Value = n.ID.ToString(),
                                                               Text = n.Name
                                                           }).ToList();

            var testCentreTip = new SelectListItem()
            {
                Value = null,
                Text = "-- Select Test Centre --"
            };
            testCentres.Insert(0, testCentreTip);
            return new SelectList(testCentres, "Value", "Text");
        }

        public TestCentre GetTestCentre(Guid? testCentreId)
        {
            return testCentreId == null ? null : _testCentres.FirstOrDefault(x => x.ID == testCentreId);
        }

        private void FetchTestCentres()
        {
            try
            {
                string jsonString = _fileUtility.ReadFromFileAsync(testCentresFilePath).GetAwaiter().GetResult();
                _testCentres = JsonConvert.DeserializeObject<List<TestCentre>>(jsonString);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
