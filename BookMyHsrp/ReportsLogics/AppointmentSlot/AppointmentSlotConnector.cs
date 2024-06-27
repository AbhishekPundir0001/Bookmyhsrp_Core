using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.AppointmentSlot.Services;
using BookMyHsrp.Libraries.DealerDelivery.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Xml.Linq;
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
        public async Task<dynamic> CheckECBlockedDates(string checkdate,string EndDate,dynamic DealerAppointment)
        {
            
            List<string> blockedDates = new List<string>();
            var dealerId = DealerAppointment.DealerAffixationCenterId;
            var dealiveryPoint = DealerAppointment.DeliveryPoint;
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                DateTime checkDateTime = DateTime.ParseExact(checkdate, "yyyy-MM-dd", provider);
                DateTime avaiableSlotFromDateTime = DateTime.ParseExact(checkdate, "yyyy-MM-dd", provider);
                //DateTime startDate = new DateTime(checkDateTime.Year, checkDateTime.Month, 1);
                //if (checkDateTime.Month == avaiableSlotFromDateTime.Month)
                //{
                //    checkDateTime = avaiableSlotFromDateTime;
                //}
                //else if (checkDateTime.Month < avaiableSlotFromDateTime.Month)
                //{
                //    return blockedDates;
                //}
                //DateTime endDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                DateTime endDate = Convert.ToDateTime(EndDate);
                DateTime tempDate = checkDateTime;
                while (tempDate <= endDate)
                {
                    var tempdate = tempDate.ToString("yyyy-MM-dd");

                    string AppointmentBlockedDatesQuery = string.Empty;
                     var result = await _appointmentSlotServices.AppointmentBlockedDates(tempdate, dealerId, dealiveryPoint);
                    if(DealerAppointment.DeliveryPoint=="Home" || DealerAppointment.DeliveryPoint == "")
                    {
                         var result1 = await _appointmentSlotServices.AppointmentBlockedDatesForHomes(tempdate, dealerId, dealiveryPoint);
                        if (result.Count > 0)
                        {
                            string Status = result[0].status.ToString();
                            if (Status == "1")
                            {
                                blockedDates.Add(tempDate.ToString("yyyy-MM-dd"));
                            }
                        }

                    }

                    if (result.Count > 0)
                    {
                        string Status = result[0].status.ToString();
                        if (Status == "1")
                        {
                            blockedDates.Add(tempDate.ToString("yyyy-MM-dd"));
                        }
                    }

                    tempDate = tempDate.AddDays(1);
                }
               // return blockedDates;

            }

            catch
            {
                return blockedDates;
            }
            
            return blockedDates;
        }
        public async Task<dynamic> CheckTimeSlot(string selectedDate,dynamic DealerAppointment,dynamic vehicledetails,dynamic userdetails,dynamic DealerDetails)
        {

             var dealerId = DealerAppointment.DealerAffixationCenterId;
            var dealiveryPoint = DealerAppointment.DeliveryPoint;
            var vehicleTypeId = userdetails.VehicleTypeId;
            var stateId = vehicledetails.StateId;
            var dealerAffexationCentreName = DealerDetails.DealerAffixationCenterName;
            string responseHtml = string.Empty;
            CultureInfo provider = CultureInfo.InvariantCulture;
            var datalist = new List<TimeSlotList>();
            try
            {
                DateTime checkDateTime = DateTime.ParseExact(selectedDate, "yyyy-MM-dd", provider);
                string DayOfWeekTemp = checkDateTime.DayOfWeek.ToString();
                var appointmentBlockDate =await _appointmentSlotServices.AppointmentBlockedDatesForSelectedDate(selectedDate, dealerId, dealiveryPoint);
              if(appointmentBlockDate.Count>0)
                {
                    string Status = appointmentBlockDate[0].status.ToString();
                    if(Status=="0" && checkDateTime.DayOfWeek != DayOfWeek.Sunday && dealiveryPoint=="Dealer")
                    {
                       var checkAppointmentSlotTime = await _appointmentSlotServices.CheckAppointmentSlotTime(selectedDate, vehicleTypeId, dealerId, dealiveryPoint, stateId);
                        foreach(var res in checkAppointmentSlotTime)
                        {
                            var data = new TimeSlotList();
                            data.SlotName = res.SlotName;
                            data.SlotID = res.SlotID;
                            data.TimeSlotID=res.TimeSlotID;
                            data.AvaiableStatus = res.AvaiableStatus;
                            data.AvaiableCount= res.AvaiableCount;
                            data.RTOCodeID= res.RTOCodeID;
                            data.BookedCount= res.BookedCount;
                            data.VehicleTypeID= res.VehicleTypeID;
                            datalist.Add(data);
                        
                    }
                    }
                    //if(dealerAffexationCentreName.toUpperCase().includes("FITMENT") && Status == "0" && dealiveryPoint=="Dealer")
                    //{
                    //   var  checkAppointmentSlotTime = await _appointmentSlotServices.CheckAppointmentSlotTime(selectedDate, vehicleTypeId, dealerId, dealiveryPoint, stateId);
                    //    foreach (var res in checkAppointmentSlotTime)
                    //    {
                    //        data.SlotName = res.SlotName;
                    //        data.SlotID = res.SlotID;
                    //        data.TimeSlotID = res.TimeSlotID;
                    //        data.AvaiableStatus = res.AvaiableStatus;
                    //        data.AvaiableCount = res.AvaiableCount;
                    //        data.RTOCodeID = res.RTOCodeID;
                    //        data.BookedCount = res.BookedCount;
                    //        data.VehicleTypeID = res.VehicleTypeID;
                    //    }
                    //}
                    if (Status == "0" && dealiveryPoint == "Home")
                    {
                       var  checkAppointmentSlotTime = await _appointmentSlotServices.CheckAppointmentSlotTimeHome(selectedDate, vehicleTypeId, dealerId, dealiveryPoint, stateId);
                        foreach (var res in checkAppointmentSlotTime)
                        {
                            var data = new TimeSlotList();
                            data.SlotName = res.SlotName;
                            data.SlotID = res.SlotID;
                            data.TimeSlotID = res.TimeSlotID;
                            data.AvaiableStatus = res.AvaiableStatus;
                            data.AvaiableCount = res.AvaiableCount;
                            data.RTOCodeID = res.RTOCodeID;
                            data.BookedCount = res.BookedCount;
                            data.VehicleTypeID = res.VehicleTypeID;
                            datalist.Add(data);
                        }
                    }
                  

                }
                

            }

            catch(Exception ex)
            {
                return ex.Message;
            }

            return datalist;
        }
    }
}
