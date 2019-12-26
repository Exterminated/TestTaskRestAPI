using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TestTaskRestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeoIPController : ControllerBase
    {
        private ApplicationContext GetContext()
        {
            var connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                var options = optionsBuilder.UseNpgsql(connectionString).Options;
                return new ApplicationContext(options);
            }
            else
            {
                return null;
            }
        }
        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory()+ @"\Properties");
            try
            {
                builder.AddJsonFile("launchSettings.json");
                var config = builder.Build();
                return config.GetConnectionString("DefaultConnection");
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        [HttpGet("{ip}")]
        public ActionResult<string> GetTodoItem(string ip)
        {
            ApplicationContext ctx = GetContext();
            if (ctx == null)
            {
                return NotFound();
            }
            return Ok(ip + ":" + ctx.Database.ProviderName.ToString());
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<GeoIPController> _logger;

        public GeoIPController(ILogger<GeoIPController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
