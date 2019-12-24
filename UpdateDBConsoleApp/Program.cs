//This product includes GeoLite2 data created by MaxMind, available from
//<a href="https://www.maxmind.com"> https://www.maxmind.com</a>.
using System;
using MaxMind.GeoIP2;
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
        static void Main(string[] args)
        {
            Console.WriteLine("Try to open GeoIP2 DB");
            string db_path = "C:\\Repos\\Exterminated\\TestTaskRestAPI\\GeoLite2_BDs\\GeoLite2-City_20191217\\GeoLite2-City.mmdb";
            //string db_path = "C:\\Repos\\Exterminated\\TestTaskRestAPI\\GeoLite2_BDs\\GeoLite2-Country_20191217\\GeoLite2-Country.mmdb";
            string ipv4_csv = @"H:\repos\TestTaskRestAPI\GeoLite2_BDs\GeoLite2-City-CSV_20191217\GeoLite2-City-Blocks-IPv4.csv";
            string eng_loc = @"H:\repos\TestTaskRestAPI\GeoLite2_BDs\GeoLite2-City-CSV_20191217\GeoLite2-City-Locations-en.csv";
            try
            {
                var connectionString = GetConnectionString();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                    var options = optionsBuilder.UseNpgsql(connectionString).Options;

                    using (ApplicationContext db = new ApplicationContext(options))
                    {

                        List<IPv4> ipsv4 = new List<IPv4>();
                        List<CityLocations> cityLocations = new List<CityLocations>();
                        using (var reader = new StreamReader(ipv4_csv))
                        {
                            var csvReader = new CsvReader(reader);
                            csvReader = ConfigureCsvReader(csvReader);

                            ipsv4 = csvReader.GetRecords<IPv4>().ToList();
                            Console.WriteLine($"Readed {ipsv4.Count()}");
                        }
                        using (var reader = new StreamReader(eng_loc))
                        {
                            var csvReader = new CsvReader(reader);
                            csvReader = ConfigureCsvReader(csvReader);

                            cityLocations = csvReader.GetRecords<CityLocations>().ToList();
                            Console.WriteLine($"Readed {cityLocations.Count()}");
                        }

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
                Console.ReadLine();
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
    }
}
