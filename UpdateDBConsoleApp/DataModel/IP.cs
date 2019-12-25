using CsvHelper.Configuration;

namespace UpdateDBConsoleApp.DataModel
{
    public class IP : IPv4
    {
        public int IPId { get; set; }
    }
    public sealed class IPMap : ClassMap<IP>
    {
        public IPMap()
        {
            AutoMap();
            Map(m => m.IPId).Ignore();
        }
    }
}
