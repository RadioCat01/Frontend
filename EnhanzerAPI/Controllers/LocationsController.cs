using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnhanzerAPI.Data;
using EnhanzerAPI.DTOs;
using EnhanzerAPI.Models;

namespace EnhanzerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public LocationsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetLocations()
        {
            var locations = _dbContext.LocationDetails
                .Select(l => new LocationDetailDto
                {
                    LocationCode = l.LocationCode,
                    LocationName = l.LocationName
                })
                .ToList();

            return Ok(locations);
        }
    }
}
