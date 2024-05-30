using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.TrackYoutOrder.Queries
{
    public class TrackYourOrderQueries
    {
        public static string TrackYourOrder =>  "select Emailid, OrderNo as 'ORDER_NUMBER',VehicleRegNo as 'REG_NUMBER',format(SlotBookingDate,'dd-MMM-yyyy')[SlotBookingDate]," +
                           "  ChassisNo as 'CHASSIS_NUMBER',EngineNo as 'ENGINE_NUMBER',OrderStatus,dealerid,ReceiptPath,OwnerName,AppointmentType,ShippingAddress1,ShippingAddress2,ShippingCity,ShippingState,ShippingPinCode " +
                " from Appointment_BookingHist where VehicleRegNo = @VehicleregNo and OrderNo = @OrderNo ";

        public static string SpTrackYourOrder => "exec BookMyHSRP_TrackYourOrder @OrderNo,@VehicleRegNo";

        public static string FitmentDate => "select 1 from [BookMyHSRP].dbo.Appointment_BookingHist a inner join[BookMyHSRP].dbo.ExpressAffixatonCenter b on a.affix_id = b.DealeraffixationId where a.OrderNo = @OrderNo and a.VehicleRegNo = @VehicleregNo ";
        
        public static string Dealername => " SELECT dealername , DealerAffixationCenterName ,DealerAffixationCenterAddress from" +
                              " DealerAffixationCenter where DealerID = @Dealerid ";
    }
}

