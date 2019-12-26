namespace TestTaskRestAPI.DataModel
{
    public class IP
    {
        public int IPId { get; set; }
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
