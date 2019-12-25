using CsvHelper.Configuration;

namespace UpdateDBConsoleApp.DataModel
{
    public class City : CityLocations
    {
        public int CityId { get; set; }
    }
    public sealed class CityMap : ClassMap<City> {
        public CityMap() {
            AutoMap();
            Map(m => m.CityId).Ignore();
        }
    }
}
