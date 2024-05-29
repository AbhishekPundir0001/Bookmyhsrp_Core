using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Libraries.AppointmentSlot.Services
{
    public interface IAppointmentSlotServices
    {
        Task<dynamic> GetAffixationId(string Id);
        Task<dynamic> GetHolidays();
        Task<dynamic> CheckAppointmentDate(string OrderType,dynamic vehicledetails,dynamic userdetails,dynamic DealerAppointment);
    }
}
