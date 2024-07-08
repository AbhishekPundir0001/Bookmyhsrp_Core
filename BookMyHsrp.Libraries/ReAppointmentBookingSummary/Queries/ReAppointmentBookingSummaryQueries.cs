using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ReAppointmentBookingSummary.Queries
{
    public class ReAppointmentBookingSummaryQueries
    {
        public static string checking = " select top 1 HSRPRecord_CreationDate,case when getdate() Between HSRPRecord_CreationDate And DATEADD(HOUR, 24, HSRPRecord_CreationDate) then 'Y' else 'N' end isRescheduleFree  from Appointment_BookingHist where OrderStatus='Success' and OrderNo = @OrderNo";
    }
}
