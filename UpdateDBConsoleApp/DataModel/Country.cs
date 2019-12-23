using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateDBConsoleApp.DataModel
{
    public class Country
    {
        public int CountryID { get; set; }
        public string IsoCode { get; set; }
        public string Name { get; set; }
        public string LocalizedNamesId { get; set; }
    }
}
