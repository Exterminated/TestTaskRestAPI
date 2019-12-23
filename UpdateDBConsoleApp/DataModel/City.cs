using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateDBConsoleApp.DataModel
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public int CountryId { get; set; }
        public int LocalizedNamesId { get; set; }
    }
}
