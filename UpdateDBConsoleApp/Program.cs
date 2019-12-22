using System;
using MaxMind;
using MaxMind.GeoIP2;

namespace UpdateDBConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Try to open GeoIP2 DB");
            try
            {
                using (var reader = new DatabaseReader("H:\\repos\\TestTaskRestAPI\\GeoLite2_BDs\\GeoLite2-Country_20191217"))
                {
                    var response = reader.AnonymousIP("85.25.43.84");
                    Console.WriteLine(response.IsAnonymous);       // true
                    Console.WriteLine(response.IsAnonymousVpn);    // false
                    Console.WriteLine(response.IsHostingProvider); // false
                    Console.WriteLine(response.IsPublicProxy);      // false
                    Console.WriteLine(response.IsTorExitNode);     // true
                    Console.WriteLine(response.IPAddress);         // '85.25.43.84'
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
    }
}
