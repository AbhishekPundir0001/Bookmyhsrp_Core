using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Dapper
{
    public class ConnectionString
    {
        public string PrimaryDatabaseHO { get; set; } = "";
        public string SecondaryDatabaseHO { get; set; } = "";
        public string DLConnectionString { get; set; } = "";
        public string HRConnectionString { get; set; } = "";
    }
    public class DynamicDataDto
    {
        public string VehicleStatusAPI { get; set; } = "";
        public string VehicleStatusAP2 { get; set; } = "";
        public string OemID { get; set; } = "";
        public string ReceiptPath { get; set; } = "";
        public string QRPath { get; set; } = "";
        public string NonHomo { get; set; } = "";
        public string NonHomoOemId { get; set; } = "";
        public string ImgoemRosmertaUrl { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNamePostfix { get; set; }
        public string RCFilePath { get; set; }
        public string key { get; set; }
        public string secret { get; set; }
        public string Host { get; set; }

    }
}
