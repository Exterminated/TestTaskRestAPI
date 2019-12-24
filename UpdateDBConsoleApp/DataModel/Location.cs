using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateDBConsoleApp.DataModel
{
    public class Location
    {
        public int LocationId { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int AccuracyRadius { get; set; }
        public bool HasCoordinates { get; set; }
        public int MetroCode { get; set; }
        public int PopulationDensity { get; set; }
        public string TimeZone { get; set; }
        public List<IP> IPs { get; set; }
    }
}
