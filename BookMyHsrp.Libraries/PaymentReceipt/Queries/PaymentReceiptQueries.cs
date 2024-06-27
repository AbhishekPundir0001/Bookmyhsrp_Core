using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.PaymentReceipt.Queries
{
    public class PaymentReceiptQueries
    {
        public static string UpdateStatus = "update Appointment_BookingHist set OrderStatus='success' where  OrderNo=@OrderNo";
    }
}
