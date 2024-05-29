using BookMyHsrp.Libraries.AppointmentSlot.Services;
using BookMyHsrp.Libraries.DealerDelivery.Services;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.AppointmentSlot.Model.AppointmentSlotModel;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.ReportsLogics.AppointmentSlot
{
    public class AppointmentSlotConnector
    {

        private readonly IAppointmentSlotServices _appointmentSlotServices;
        public AppointmentSlotConnector(IAppointmentSlotServices appointmentSlotServices)
        {
            _appointmentSlotServices = appointmentSlotServices ?? throw new ArgumentNullException(nameof(appointmentSlotServices)); ;

        }
        public async Task<dynamic> CheckAppointmentSlot(dynamic jsonSerializer, string Id)
        {
            var setSession = new AppointmentSlotSessionResponse();
            setSession.DealerAffixationCenterId = Id;
            try
            {
                var getAffixationId = await _appointmentSlotServices.GetAffixationId(Id);
                if (getAffixationId.Count > 0)
                {
                    setSession.SelectedSlotID = "1";
                    setSession.SelectedSlotDate = "1900-01-01";
                    setSession.SelectedSlotTime = "NULL";
                    setSession.Affix = "Affixval";
                    setSession.Message = "Affix";
                }
                else
                {
                    setSession.Message = "Success";
                    setSession.Affix = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return setSession;
        }
        public async Task<dynamic> GetHolidays(dynamic vehicledetails, dynamic userdetails, dynamic DealerAppointment)
        {
            var dataOFDates = new Dates();
            var result = await _appointmentSlotServices.GetHolidays();
            if (result.Count > 0)
            {
                foreach (var data in result)
                {
                    dataOFDates.BlockDate = data.blockDate;

                }
            }
            return dataOFDates;
         }
        public async Task<dynamic> GetCheckAppointmentDate(dynamic vehicledetails, dynamic userdetails, dynamic DealerAppointment)
        {
            var dataOFDates = new Dates();
            string SQLAppointmentFromDate = string.Empty;

            string OrderType = userdetails.OrderType == null ? "" : userdetails.OrderType.ToString();
            var checkAppointmentDate = await _appointmentSlotServices.CheckAppointmentDate(OrderType,vehicledetails, userdetails, DealerAppointment);
            if(checkAppointmentDate.Count>0)
            {
                 dataOFDates.FromDate = checkAppointmentDate[0].FromDate;
                dataOFDates.EndDate = checkAppointmentDate[0].EndDate;
            }
            return dataOFDates;
        }
    }
}
