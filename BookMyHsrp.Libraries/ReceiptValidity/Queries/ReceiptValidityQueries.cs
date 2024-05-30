using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ReceiptValidity.Queries
{
    public class ReceiptValidityQueries
    {
        public static string ReceiptValidity = "SELECT DATEDIFF(day, SlotBookingDate, Getdate())" +
                                 " daycount,Replace(Convert(varchar,DATEADD(dd, 15, SlotBookingDate),106),' ','-') as 'ExpireDate'," +
                                 " VehicleRegNo from Appointment_BookingHist" +
                                 " where VehicleRegNo = @VehicleregNo and OrderStatus='Success' order by HSRPRecord_CreationDate desc ";
    }
}
