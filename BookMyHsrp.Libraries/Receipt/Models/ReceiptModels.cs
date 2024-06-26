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
        public class PaymentReceipt
        {
            public string OrderDate { get; set; }
            public string AppointmentDateTime { get; set; }
            public string TransactionId { get; set; }

        }

    }
}
