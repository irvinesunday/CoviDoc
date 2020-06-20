using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoviDoc.Models.Location;

namespace CoviDoc.Models.Interfaces
{
    public interface ILocationRepository
    {
        // IEnumerable<SelectListItem> GetCountries();
        // IEnumerable<SelectListItem> GetCounties();
        string[] GetCountries();
        List<County> GetCounties();
        IEnumerable<SelectListItem> GetConstituencies(string countyName);
        IEnumerable<SelectListItem> GetConstituencies();
        IEnumerable<SelectListItem> GetWards(string countyName, string constituencyId);
        IEnumerable<SelectListItem> GetWards();
    }
}
