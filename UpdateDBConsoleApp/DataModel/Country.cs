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
        public List<Subdivision> Subdivisions { get; set; } //множество регионов/областей/штатов
        public int GeoNameId { get; set; }
    }
}
