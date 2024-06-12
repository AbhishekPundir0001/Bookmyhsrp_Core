using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.TrackYoutOrder.Services
{
    public interface ITrackYourOrderService
    {
        Task<dynamic> GetTrackYourOrderStatus(dynamic dto);
        Task<dynamic> GetTrackYourOrderStatusSp(dynamic dto);
        Task<dynamic> GetFitmentDate(dynamic dto);
        Task<dynamic> GetDealerName(dynamic dto);
    }
}
