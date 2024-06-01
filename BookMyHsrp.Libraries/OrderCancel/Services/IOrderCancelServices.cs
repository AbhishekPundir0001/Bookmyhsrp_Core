using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OrderCancel.Services
{
    public interface IOrderCancelServices
    {
        Task<dynamic> DealerWalletdetail(dynamic dto);
        Task<dynamic> CancelOrderDetails(dynamic dto);
        Task<dynamic> CancelOrderGet(dynamic dto);
        Task<dynamic> DealerAddress(dynamic dto);
    }
}
