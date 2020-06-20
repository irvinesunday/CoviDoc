using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoviDoc.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CoviDoc.Models.Location;

namespace CoviDoc.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        readonly ILocationRepository _locationRepository;
        public LocationsController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet]
        [Route("api/counties")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCounties()
        {
            return Ok(_locationRepository.GetCounties());
        }

        [HttpGet]
        [Route("api/countries")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCountriesAsync()
        {
            return Ok(_locationRepository.GetCountries());
        }
    }
}