using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<SelectListItem> GetCounties();
        IEnumerable<SelectListItem> GetConstituencies(int countyId);
        IEnumerable<SelectListItem> GetConstituencies();
        IEnumerable<SelectListItem> GetWards(int countyId, string constituencyId);
        IEnumerable<SelectListItem> GetWards();
    }
}
