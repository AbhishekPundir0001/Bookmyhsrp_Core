using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Receipt.Models
{
    public class ReceiptModels
    {
        public class Receipt
        {
            public string OrderNo { get; set; } = "";
            public string VehicleregNo { get; set; } = "";
        }

        public class ResponseSticker
        {
            public string Message { get; set; }

        }

    }
}
