using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ReAppointment.Queries
{
    public class ReAppointmentQueries
    {
        public static string GetOrderDetails = "exec GET_ORDER_Details @OrderNo, @VehicleregNo";
        public static string AuthorisedReschedule = "SELECT top 1 VehicleRegNo FROM hsrprecords WITH (NOLOCK) Where IsBookMyHsrpRecord = 'Y' and VehicleRegNo = @VehicleregNo and OrderNo = @OrderNo and isnull(OrderClosedDate ,'')!= ''  order by  HSRPRecordID desc";
        public static string AuthorisedRescheduleSticker = "SELECT top 1 vehRegNo FROM HSRPRecordsOnlyStricker WITH(NOLOCK) Where vehRegNo = @VehicleregNo and BookMyHSRPOrderNo = @OrderNo and isnull(StickerAffixedDateTime ,'')!= ''  order by ID";
        public static string  VerifyRescheduleSticker  = "select 1 from [BookMyHSRP].dbo.Appointment_BookingHist a inner join [BookMyHSRP].dbo.ExpressAffixatonCenter b on a.affix_id = b.DealeraffixationId where a.OrderNo = @OrderNo and a.VehicleRegNo = @VehicleregNo and a.PlateSticker='plate' and SlotTime = 'NULL'";
    
    }
}
