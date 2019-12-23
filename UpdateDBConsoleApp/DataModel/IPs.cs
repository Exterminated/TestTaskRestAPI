using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateDBConsoleApp.DataModel
{
    public class IPs
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public int CountryID { get; set; }
        public int LocationId { get; set; }
        public int SubdivisionId { get; set; }
    }
}
