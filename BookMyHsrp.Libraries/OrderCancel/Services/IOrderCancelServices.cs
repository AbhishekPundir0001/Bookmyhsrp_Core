using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OrderCancel.Services
{
    public interface IOrderCancelServices
    {
        Task<dynamic> DealerWalletdetail(dynamic dto);
        Task<dynamic> CancelOrderDetails(dynamic dto);
        Task<dynamic> CancelOrderGet(dynamic dto);
        Task<dynamic> DealerAddress(dynamic Dealerid);
        Task<dynamic> OrderStatusCancel(dynamic dto);
            Task<dynamic> voidOrder(dynamic dto);
            Task<dynamic> OrderStatusUpdate(dynamic dto);
            Task<dynamic> checkBookApp(dynamic dto);
            Task<dynamic> updateBookApp(dynamic dto);
            Task<dynamic> Smsqry(dynamic dto);
        Task<dynamic> SMSLogSaveQuery(dynamic dto, string MobileNo, string sms, string responsecode);
        Task<dynamic> checkcancelrecord(dynamic dto);
        Task<dynamic> updatecancelledlog(dynamic dto);
        Task<dynamic> updatecancelledlog2(dynamic dto);
        Task<dynamic> cancelledlogerror(dynamic dto);
        Task<dynamic> cancellationfinalpage(dynamic dto);
    }
}





