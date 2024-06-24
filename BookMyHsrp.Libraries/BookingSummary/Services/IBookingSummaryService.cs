using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.BookingSummary.Services
{
    public interface IBookingSummaryService
    {
        Task<dynamic> BookingSummaryConfirmation(dynamic DealerAppointment);
    }
}
