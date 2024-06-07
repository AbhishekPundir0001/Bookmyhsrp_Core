using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Grievance.Services
{
    public interface IGrievanceServices
    {
        Task<dynamic> greivanceinsert(string VehicleRegNo, string OrderNo, string MobileNo, string EmailId, string Query, string CustomerName);
        Task<dynamic> getRecord(dynamic VehicleRegNo);
    }
}
