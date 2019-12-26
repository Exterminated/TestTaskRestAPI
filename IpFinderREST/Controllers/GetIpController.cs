using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IpFinderREST.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IpFinderREST.Controllers
{
    [ApiController]
    [Route("api/GeoIp")]
    public class GetIpController : ControllerBase
    {
        private ApplicationContext db;
        public GetIpController(ApplicationContext context) {
            db = context;
        }
        // GET api/<controller>/5
        [HttpGet("{ip}")]
        public async Task<ActionResult<string>> GetInfoByIP(string ip)
        {
            if (db == null)
            {
                return NotFound("Can't open DataBase");
            }
            else
            {
                var ipFromDB = await db?.IPs?.AsNoTracking().FirstOrDefaultAsync(x => x.network.Contains(ip));
                if (ipFromDB != null)
                {
                    var cityInfo = await db?.Cities?.AsNoTracking().Where(c => c.geoname_id.Value == ipFromDB.geoname_id.Value).Distinct().ToListAsync();
                    //var options = new JsonSerializerOptions
                    //{
                    //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    //    WriteIndented = true
                    //};
                    //var modelJson = JsonSerializer.Serialize(new { IP_Info = ipFromDB, Geo_Info = cityInfo }, options);
                    //return new ObjectResult(modelJson);
                    if (cityInfo != null) {
                        return new ObjectResult(new { IP_Info = ipFromDB, Geo_Info = cityInfo.ToArray() });
                    }
                    else {
                        return new ObjectResult(ipFromDB);
                    }
                }
                else
                {
                    return NotFound($"Can't find record with ip {ip}");
                }
            }
            
        }
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(@"For using IpFinder call /api/GeoIP/{ip_adress}");
        }
    }
}
