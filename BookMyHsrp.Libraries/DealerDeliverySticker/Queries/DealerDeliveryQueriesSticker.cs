using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.DealerDeliverySticker.Queries
{
    public class DealerDeliveryQueriesSticker
    {
        public static string GetDealersForRajasthan => "exec BMHSRP_GET_DEALERS_for_Rajasthan  @oemid,@StateId,'',@VehicleCat,'Scooter_Hero',@VehicleClass,@OrderType";
        public static string GetDealersForRajasthanElse => "exec BMHSRP_GET_DEALERS_for_Rajasthan  @oemid,@StateId,'',@VehicleCat,@VehicleType,@VehicleClass,@OrderType";

        public static string GetDealers => "exec BMHSRP_GET_DEALERS  @oemid,@StateId,'',@VehicleCat,'Scooter_Hero',@VehicleClass,@OrderType";

        public static string GetDealersElse => "exec BMHSRP_GET_DEALERS  @oemid,@StateId,'',@VehicleCat,@VehicleType,@VehicleClass,@OrderType";
        public static string CheckOemRate => "exec usp_GetTaxRate  @OrderType,@VehicleType,@HSRP_StateId";
        public static string CheckOemRateQuery => "exec CheckOrdersRates  @OemID,@OrderType,@VehicleClass,@VehicleType,@VehicleCategoryID,@FulType,@AppointmentType,@HSRP_StateID,@CustomerStateName,@PlateSticker";
        public static string GetAffixationId => "select DealeraffixationId from ExpressAffixatonCenter where DealeraffixationId=@Id";

    }
}
