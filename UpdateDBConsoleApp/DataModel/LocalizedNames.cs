using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateDBConsoleApp.DataModel
{
    public class LocalizedNames
    {
        public int LocalizedNamesId { get; set; }
        public int GeoNameId { get; set; }
        public string Local { get; set; }
        public string Name { get; set; }
    }
}
