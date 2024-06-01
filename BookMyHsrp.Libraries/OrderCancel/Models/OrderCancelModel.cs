using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OrderCancel.Models
{
    public class OrderCancelModel
    {
        public class OrderCancel
        {
            public string OrderNo { get; set; } = "";
            public string VehicleregNo { get; set; } = "";

        }
        public class OtpModal
        {
            public string VehicleRegNo { get; set; } = "";
            public string MobileNo { get; set; } = "";
            public string OrderNo { get; set; } = "";

        }
    }
}
