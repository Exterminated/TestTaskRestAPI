using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateDBConsoleApp.DataModel
{
    public class Subdivision
    {
        public int SubdivisionId { get; set; }        
        public string IsoCode { get; set; }
        public string Name { get; set; }
        public int CountryID { get; set; }
        public Country Country { get; set; }
        public List<City> Cities { get; set; }//множество городов в области/регионе/штате
        public int GeoNameId { get; set; }
    }
}
