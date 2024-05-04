using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Dapper
{
    public class ConnectionString
    {
        public string PrimaryDatabaseHO { get; set; }
        public string SecondaryDatabaseHO { get; set; }
    }
    public class DynamicDataDto
    {
        public string VehicleStatusAPI { get; set; }
        public string VehicleStatusAP2 { get; set; }
        public string OemID { get; set; }
    }
}
