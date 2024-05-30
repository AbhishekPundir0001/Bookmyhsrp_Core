using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OrderCancel.Queries
{
    public class OrderCancelQueries
    {
        public static string DealerWalletdetail = "select * from[HSRPOEM].dbo.dealerwalletorder where OrderNo = @OrderNo and VehicleRegNo = @VehicleregNo";
        public static string CancelOrderGet = "exec usp_CancelOrderGet @OrderNo,@VehicleregNo";
    }
}

