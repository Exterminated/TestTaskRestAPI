//This product includes GeoLite2 data created by MaxMind, available from
//<a href="https://www.maxmind.com"> https://www.maxmind.com</a>.
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using UpdateDBConsoleApp.DataModel;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;

namespace UpdateDBConsoleApp
{
    class Program
    {
        readonly static int _rowPageLimit = 2000;
        static void Main(string[] args)
        {
            Console.WriteLine("Open and load new enteries to DB?(Y/N)");
            string answ = Console.ReadLine();
            if (answ.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Try to open GeoIP2 DB");

                Dictionary<string, string> csvs = GetImportedData();
                try
                {
                    var connectionString = GetConnectionString();
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                        var options = optionsBuilder.UseNpgsql(connectionString).Options;

                        using (ApplicationContext db = new ApplicationContext(options))
                        {
                            List<IP> ipsv4 = new List<IP>();
                            List<City> cityLocations = new List<City>();
                            using (var reader = new StreamReader(csvs["ipv4_csv"]))
                            {
                                var csvReader = new CsvReader(reader);
                                csvReader = ConfigureCsvReader(csvReader);
                                csvReader.Configuration.RegisterClassMap<IPMap>();

                                ipsv4 = csvReader.GetRecords<IP>().Distinct().ToList();
                                Console.WriteLine($"Readed IPv4 {ipsv4.Count()}");

                                do
                                {
                                    db.IPs.AddRange(ipsv4.Take(_rowPageLimit));
                                    db.SaveChanges();
                                    ipsv4.RemoveRange(0, _rowPageLimit);
                                }
                                while (ipsv4.Count != 0);

                            }
                            using (var reader = new StreamReader(csvs["loc_csv"]))
                            {
                                var csvReader = new CsvReader(reader);
                                csvReader = ConfigureCsvReader(csvReader);
                                csvReader.Configuration.RegisterClassMap<CityMap>();

                                cityLocations = csvReader.GetRecords<City>().Distinct().ToList();
                                Console.WriteLine($"Readed cityLocations {cityLocations.Count()}");

                                do
                                {
                                    db.Cities.AddRange(cityLocations.Take(_rowPageLimit));
                                    db.SaveChanges();
                                    ipsv4.RemoveRange(0, _rowPageLimit);
                                }
                                while (cityLocations.Count != 0);
                            }
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Can't open DB");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in getting data\n{ex.ToString()}");
                }
                finally
                {
                    Console.WriteLine("All readed and saved to DB");
                    Console.ReadLine();
                }
            }
            else {
                Console.WriteLine("Leaving app");                
            }

        }
        private static CsvReader ConfigureCsvReader(CsvReader reader)
        {
            reader.Configuration.Delimiter = ",";
            reader.Configuration.CultureInfo = CultureInfo.InvariantCulture;
            return reader;
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
            catch (Exception ex)
            {
                Console.WriteLine("Error in openning appsettings.json");
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }
        private static Dictionary<string, string> GetImportedData() {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            try
            {
                builder.AddJsonFile("appsettings.json");
                var config = builder.Build();
                return config.GetSection("ImportingData").GetChildren().Select(i => new KeyValuePair<string, string>(i.Key, i.Value)).ToDictionary(x => x.Key, x => x.Value);                 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in openning appsettings.json");
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
