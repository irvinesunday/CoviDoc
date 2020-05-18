using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Interfaces
{
    public interface ICountyRepository
    {
        List<County> GetCounties();
    }
}
