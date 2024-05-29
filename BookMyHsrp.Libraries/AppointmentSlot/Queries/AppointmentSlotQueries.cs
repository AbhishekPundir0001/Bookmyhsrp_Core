using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.AppointmentSlot.Queries
{
    public class AppointmentSlotQueries
    {
        public static string GetAffixationId => "select DealeraffixationId from ExpressAffixatonCenter where DealeraffixationId=@Id";
        public static string GetHolidays => "select ''''+STRING_AGG(blockDate, ''',''')+''''   blockDate from ( " +
            "select distinct CONVERT(VARCHAR(20), cast(blockDate as date), 120) blockDate from [HSRPOEM].[dbo].[HolidayDateTime] " +
            "where cast(blockDate as date) between getdate() and cast(DATEADD(MONTH, +5, GETDATE()) as date) and [Desc] = 'Holiday' )t";
        public static string CheckAppointmentDate => "exec CheckAppointmentFromDate @OemId,@DealerId,@VehicleTypeId,@DeliveryPoint,@StateId,'Plate',@NonHomo,@OrderType";

    }
}
