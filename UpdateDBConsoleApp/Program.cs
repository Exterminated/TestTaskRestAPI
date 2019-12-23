using System;
using MaxMind.GeoIP2;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace UpdateDBConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Try to open GeoIP2 DB");
            string db_path = "C:\\Repos\\Exterminated\\TestTaskRestAPI\\GeoLite2_BDs\\GeoLite2-City_20191217\\GeoLite2-City.mmdb";
            //string db_path = "C:\\Repos\\Exterminated\\TestTaskRestAPI\\GeoLite2_BDs\\GeoLite2-Country_20191217\\GeoLite2-Country.mmdb";
            try
            {
                using (var reader = new DatabaseReader(db_path))
                {
                    //var response = reader.Country("85.25.43.84");
                    //Console.WriteLine(response.Country.IsoCode);        // 'US'
                    //Console.WriteLine(response.Country.Name);           // 'United States'
                    //Console.WriteLine(response.Country.Names["zh-CN"]); // '美国'
                    // Replace "City" with the appropriate method for your database, e.g.,
                    // "Country".
                    var city = reader.City("128.101.101.101");

                    Console.WriteLine(city.Country.IsoCode); // 'US'
                    Console.WriteLine(city.Country.Name); // 'United States'
                    Console.WriteLine(city.Country.GeoNameId);
                    //Console.WriteLine(city.Country.Names["zh-CN"]); // '美国'

                    Console.WriteLine(city.MostSpecificSubdivision.Name); // 'Minnesota'
                    Console.WriteLine(city.MostSpecificSubdivision.IsoCode); // 'MN'

                    Console.WriteLine(city.City.Name); // 'Minneapolis'

                    Console.WriteLine(city.Postal.Code); // '55455'

                    Console.WriteLine(city.Location.Latitude); // 44.9733
                    Console.WriteLine(city.Location.Longitude); // -93.2323
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in getting data\n{ex.ToString()}");
            }
            finally {
                Console.ReadLine();
            }
            var connectionString = GetConnectionString();
            if (!string.IsNullOrEmpty(connectionString))
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                var options = optionsBuilder
                    .UseNpgsql(connectionString)
                    .Options;

                using (ApplicationContext db = new ApplicationContext(options))
                {

                }
            }
            else {
                Console.WriteLine("Can't open DB");
            }

        }
        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());
            try
            {
                builder.AddJsonFile("appsettings.json");
                var config = builder.Build();
                return config.GetConnectionString("DefaultConnection");
            }
            catch (Exception ex) {
                Console.WriteLine("Error in openning appsettings.json");
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }

        }
    }
}
