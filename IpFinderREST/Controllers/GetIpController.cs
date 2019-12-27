using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IpFinderREST.Models;
using System.Text.Json;
using System.Net;

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
            bool ValidateIP = IPAddress.TryParse(ip, out IPAddress ipadr);
            if (!ValidateIP) {
                return NotFound($"Not valid IP adress {ip}");
            }
            if (db == null)
            {
                return NotFound("Can't open DataBase");
            }
            else
            {
                var parseIp = ip.Split('.', ' ');
                var ipsFromDB = await db?.IPs?.AsNoTracking().Where(x => x.network.Contains(parseIp[0] + '.' + parseIp[1] + '.' + parseIp[2])).ToListAsync();
                foreach (var ipWithMask in ipsFromDB)
                {
                    if (ipadr.IsIpInSubnet(ipWithMask.network)) {
                        var cityInfo = await db?.Cities?.AsNoTracking().FirstOrDefaultAsync(c => c.geoname_id.Value == ipWithMask.geoname_id.Value && c.city_name != null && c.country_name != null && c.subdivision_1_name != null);

                        if (cityInfo != null)
                        {
                            return new ObjectResult(new { IP_Info = ipWithMask, Geo_Info = cityInfo });
                        }
                        else
                        {
                            return new ObjectResult(ipWithMask);
                        }
                    }
                }
                return NotFound($"Can't find ip {ip} in DB");
            }
            
        }
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(@"For using IpFinder call /api/GeoIP/{ip_adress}");
        }
    }
}
