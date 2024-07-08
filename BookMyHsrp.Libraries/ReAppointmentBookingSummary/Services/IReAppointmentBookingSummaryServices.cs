using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ReAppointmentBookingSummary.Services
{
    public interface IReAppointmentBookingSummaryServices
    {

        Task<dynamic> ReScheduleFreeCheck(dynamic dto);
    }
}
