using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Grievance.Queries
{
    public class GreivanceQueries
    {
        public static string greivanceinsert = "exec usp_GrievanceInsert @VehicleRegNo,@OrderNo,@MobileNo,@EmailId,@Query,@TicketNo,@CustomerName";
        public static string getRecord = "select top 1 1 from Grievance where VehicleRegNo=@VehicleRegNo";
    }
}
