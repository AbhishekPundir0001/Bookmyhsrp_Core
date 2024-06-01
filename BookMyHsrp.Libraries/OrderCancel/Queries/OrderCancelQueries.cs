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
        public static string  CancelOrderDetails = "exec usp_Appointment_BookingHist_CencelOrder @OrderNo, @VehicleregNo" ;
        public static string DealerAddress = "select DealerAffixationCenterAddress,DealerAffixationCenterName,DealerAffixationCenterCity,DealerAffixationCenterPinCode from [HSRPOEM].dbo.DealerAffixationCenter where DealerID = @DealerId ";
    }
}

