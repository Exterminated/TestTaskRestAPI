namespace UpdateDBConsoleApp.DataModel
{
    public class IPv4
    {
        //network,geoname_id,registered_country_geoname_id,represented_country_geoname_id,is_anonymous_proxy,is_satellite_provider,postal_code,latitude,longitude,accuracy_radius
        public string network { get; set; }
        public int? geoname_id { get; set; }
        public int? registered_country_geoname_id { get; set; }
        public int? represented_country_geoname_id { get; set; }
        public int? is_anonymous_proxy { get; set; }
        public int? is_satellite_provider { get; set; }
        public string postal_code { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public int? accuracy_radius { get; set; }
    }

}
