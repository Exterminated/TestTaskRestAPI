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
        readonly static int _rowPageLimit = 5000;
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

                        Console.WriteLine("Do you want clear and reload full DB? This is long time operation (Y/N)");
                        string reloadAnsw = Console.ReadLine();

                        using (ApplicationContext db = new ApplicationContext(options))
                        {
                            db.ChangeTracker.AutoDetectChangesEnabled = false;
                            List<IP> ipsv4 = new List<IP>();
                            List<City> cityLocations = new List<City>();

                            Console.WriteLine("Starting clearing IP table");
                            foreach (var id in db.IPs.Select(e => e.IPId))
                            {
                                var entity = new IP { IPId = id };
                                db.IPs.Attach(entity);
                                db.IPs.Remove(entity);
                                db.SaveChanges();
                            }
                            
                            Console.WriteLine("Cleared IP table");
                            Console.WriteLine("Starting clearing Cities table");
                            foreach (var id in db.Cities.Select(e => e.CityId))
                            {
                                var entity = new City { CityId = id };
                                db.Cities.Attach(entity);
                                db.Cities.Remove(entity);
                                db.SaveChanges();
                            }
                            
                            Console.WriteLine("Cleared Cities table");

                            Console.WriteLine("Starting add to IP table");
                            using (var reader = new StreamReader(csvs["ipv4_csv"]))
                            {
                                var csvReader = new CsvReader(reader);
                                csvReader = ConfigureCsvReader(csvReader);
                                csvReader.Configuration.RegisterClassMap<IPMap>();

                                ipsv4 = csvReader.GetRecords<IP>().Distinct().ToList();
                                Console.WriteLine($"Readed IPv4 {ipsv4.Count()}");

                                if (reloadAnsw.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    try
                                    {
                                        do
                                        {
                                            db.IPs.AddRange(ipsv4.Take(_rowPageLimit));
                                            db.SaveChanges();
                                            ipsv4.RemoveRange(0, _rowPageLimit <= ipsv4.Count() ? _rowPageLimit : ipsv4.Count());
                                        }
                                        while (ipsv4.Count != 0);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Problem wtih IP table\n{ex.ToString()}");
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Updating existing IPs");
                                    try
                                    {
                                        var updItem = db?.IPs?.Join(ipsv4,
                                            i => i.network,
                                            e => e.network,
                                            (i, e) => new
                                            {
                                                i.IPId,
                                                e.accuracy_radius,
                                                e.geoname_id,
                                                e.is_anonymous_proxy,
                                                e.is_satellite_provider,
                                                e.latitude,
                                                e.longitude,
                                                e.network,
                                                e.postal_code,
                                                e.registered_country_geoname_id,
                                                e.represented_country_geoname_id
                                            }
                                            ).ToList();
                                        do
                                        {
                                            db.IPs.UpdateRange((IP)updItem.Take(_rowPageLimit));
                                            db.SaveChanges();
                                            updItem.RemoveRange(0, _rowPageLimit <= updItem.Count() ? _rowPageLimit : updItem.Count());
                                        } while (updItem.Count() != 0);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error while updating IP table\n" + ex.ToString());
                                    }
                                }
                            }
                            Console.WriteLine("End add to IP table");
                            Console.WriteLine("Starting add to City table");
                            using (var reader = new StreamReader(csvs["loc_csv"]))
                            {
                                var csvReader = new CsvReader(reader);
                                csvReader = ConfigureCsvReader(csvReader);
                                csvReader.Configuration.RegisterClassMap<CityMap>();

                                cityLocations = csvReader.GetRecords<City>().Distinct().ToList();
                                Console.WriteLine($"Readed cityLocations {cityLocations.Count()}");

                                if (reloadAnsw.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    try
                                    {
                                        do
                                        {
                                            db.Cities.AddRange(cityLocations.Take(_rowPageLimit));
                                            db.SaveChanges();
                                            cityLocations.RemoveRange(0, _rowPageLimit <= cityLocations.Count() ? _rowPageLimit : cityLocations.Count());
                                        }
                                        while (cityLocations.Count != 0);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Problem wtih Cities table\n"+ex.ToString());
                                    }
                                }
                                else {
                                    try {
                                        var updItem = db?.Cities?.Join(cityLocations,
                                            i => i.geoname_id,
                                            e => e.geoname_id,
                                            (i, e) => new
                                            {
                                                i.CityId,
                                                e.city_name,
                                                e.continent_code,
                                                e.continent_name,
                                                e.country_iso_code,
                                                e.country_name,
                                                e.geoname_id,
                                                e.is_in_european_union,
                                                e.locale_code,
                                                e.metro_code,
                                                e.subdivision_1_iso_code,
                                                e.subdivision_1_name,
                                                e.subdivision_2_iso_code,
                                                e.subdivision_2_name,
                                                e.time_zone                                                
                                            }
                                            ).ToList();
                                        do
                                        {
                                            db.Cities.UpdateRange((City)updItem.Take(_rowPageLimit));
                                            db.SaveChanges();
                                            updItem.RemoveRange(0, _rowPageLimit <= updItem.Count() ? _rowPageLimit : updItem.Count());
                                        } while (updItem.Count() != 0);
                                    }
                                    catch (Exception ex) {
                                        Console.WriteLine("Error while updating Cities table\n" + ex.ToString());
                                    }
                                }
                            }
                            db.SaveChanges();
                            db.ChangeTracker.AutoDetectChangesEnabled = true;
                            Console.WriteLine("Starting add to City table");
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
            else
            {
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
            Console.WriteLine("Starting to read appsettings.json for connection string");
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
            finally {
                Console.WriteLine("End to read appsettings.json for connection string");
            }
        }
        private static Dictionary<string, string> GetImportedData()
        {
            Console.WriteLine("Starting to read appsettings.json");
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
            finally {
                Console.WriteLine("End to read appsettings.json");
            }
        }
    }
}
