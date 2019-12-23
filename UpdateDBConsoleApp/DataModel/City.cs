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
        public int SubdivisionId { get; set; }
        public Subdivision Subdivision { get; set; }
        public List<Location> Locations { get; set; }//множесто локаций
        public int GeoNameId { get; set; }
    }
}
