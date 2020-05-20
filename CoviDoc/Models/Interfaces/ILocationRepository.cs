using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<SelectListItem> GetCountries();
        IEnumerable<SelectListItem> GetCounties();
        IEnumerable<SelectListItem> GetConstituencies(string countyName);
        IEnumerable<SelectListItem> GetConstituencies();
        IEnumerable<SelectListItem> GetWards(string countyName, string constituencyId);
        IEnumerable<SelectListItem> GetWards();
    }
}
