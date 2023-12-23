using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportWebApi.Data;
using TransportWebApi.Model;

namespace TransportWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageManageController : ControllerBase
    {
        private readonly TransPortDbContext _dbContext;
        public PackageManageController(TransPortDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("GetVehicle&DriverId")]
        public IActionResult GetVehicleAndDriver(int capacity) 
        {
            if(capacity == 0)
            {
                return Ok("Please Enter Capacity");
            }
            var vid = _dbContext.Vehicle.Where(x=>x.Capacity>=capacity).OrderBy(e=>e.Capacity).Take(1).ToList();
            if (vid.Count > 0)
            {
                List<int> ides = new List<int>();
                var did = _dbContext.Driver.Where(x => x.LicenseLevel == vid[0].LicenseLevel && x.status==1).ToList();
                if (did.Count > 0)
                {
                    ides.Append(vid[0].VehicleID);
                    ides.Append(did[0].DriverID);
                    return Ok(ides);
                }
            }
            return Ok("Not any Vehicle");

        }
        
    }
}
