using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ReAppointment.Services
{
    public interface IReAppointmentServices
    {
        Task<dynamic> GetOrderDetails(dynamic dto);
        Task<dynamic> AuthorisedReschedule(dynamic dto);
        Task<dynamic> AuthorisedRescheduleSticker(dynamic dto);
        Task<dynamic> VerifyRescheduleSticker(dynamic dto);
    }
}
