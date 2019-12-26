namespace IpFinderREST.Models
{
    public class City
    {
        public int CityId { get; set; }
        public int? geoname_id { get; set; }
        public string locale_code { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; }
        public string country_iso_code { get; set; }
        public string country_name { get; set; }
        public string subdivision_1_iso_code { get; set; }
        public string subdivision_1_name { get; set; }
        public string subdivision_2_iso_code { get; set; }
        public string subdivision_2_name { get; set; }
        public string city_name { get; set; }
        public string metro_code { get; set; }
        public string time_zone { get; set; }
        public int? is_in_european_union { get; set; }
    }
}
