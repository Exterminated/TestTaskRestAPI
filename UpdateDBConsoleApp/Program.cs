//This product includes GeoLite2 data created by MaxMind, available from
//<a href="https://www.maxmind.com"> https://www.maxmind.com</a>.
using System;
using MaxMind.GeoIP2;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using UpdateDBConsoleApp.DataModel;

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

                    var readCity = reader.City("128.101.101.101");

                    Console.WriteLine(readCity.Country.IsoCode); // 'US'
                    Console.WriteLine(readCity.Country.Name); // 'United States'
                    Console.WriteLine(readCity.Country.GeoNameId);
                    //Console.WriteLine(city.Country.Names["zh-CN"]); // '美国'

                    Console.WriteLine(readCity.MostSpecificSubdivision.Name); // 'Minnesota'
                    Console.WriteLine(readCity.MostSpecificSubdivision.IsoCode); // 'MN'

                    Console.WriteLine(readCity.City.Name); // 'Minneapolis'

                    Console.WriteLine(readCity.Postal.Code); // '55455'

                    Console.WriteLine(readCity.Location.Latitude); // 44.9733
                    Console.WriteLine(readCity.Location.Longitude); // -93.2323

                    var connectionString = GetConnectionString();
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                        var options = optionsBuilder.UseNpgsql(connectionString).Options;                                                

                        using (ApplicationContext db = new ApplicationContext(options))
                        {
                            //foreach (var country in reader.)
                            //{

                            //}

                        }
                    }
                    else
                    {
                        Console.WriteLine("Can't open DB");
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in getting data\n{ex.ToString()}");
            }
            finally {
                Console.ReadLine();
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
