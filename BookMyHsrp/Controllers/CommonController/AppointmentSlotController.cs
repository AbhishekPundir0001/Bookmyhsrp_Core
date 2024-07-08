using BookMyHsrp.Libraries.AppointmentSlot.Model;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.ReportsLogics.AppointmentSlot;
using BookMyHsrp.ReportsLogics.DealerDelivery;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static BookMyHsrp.Libraries.AppointmentSlot.Model.AppointmentSlotModel;
using static BookMyHsrp.Libraries.BookingSummary.Model.BookingSummaryModel;
using static BookMyHsrp.Libraries.DealerDelivery.Models.DealerDeliveryModel;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers.CommonController
{
    public class AppointmentSlotController : Controller
    {
        private readonly ILogger<AppointmentSlotController> _logger;
        private readonly AppointmentSlotConnector _appointmentconnector;
        public AppointmentSlotController(ILogger<AppointmentSlotController> logger, AppointmentSlotConnector appointmentconnector)
        {
            _appointmentconnector = appointmentconnector;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("appointmentSlot")]
        public IActionResult AppointmentSlot()
        {
            return View();
        }
        [HttpGet]
        [Route("holiday")]
        public async Task<IActionResult> GetHolidays()
        {
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var dealerAppointment = HttpContext.Session.GetString("AppointmentSlotId");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);
            var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);

            var result = await _appointmentconnector.GetHolidays(vehicledetails, userdetails, DealerAppointment);
            return Json(result);
        }
        [HttpGet]
        [Route("chek-appointment-date")]
        public async Task<IActionResult> GetCheckAppointmentDate()
        {
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var dealerAppointment = HttpContext.Session.GetString("AppointmentSlotId");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);
            if (dealerAppointment == null)
            {
                var DeliveryPoint = "Home";
                var result = await _appointmentconnector.GetCheckAppointmentDate(vehicledetails, userdetails, DeliveryPoint);
                return Json(result);
            }
            else
            {
                var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);
                var result = await _appointmentconnector.GetCheckAppointmentDate(vehicledetails, userdetails, DealerAppointment);
                return Json(result);
            }
          
        }
        [HttpPost]
        [Route("chek-ec-block-date")]
        public async Task<IActionResult> CheckECBlockedDates([FromBody] AppointmentSlotCheckEc request)
        {
            var dealerAppointment = HttpContext.Session.GetString("AppointmentSlotId");
            var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);
           
            var result =await _appointmentconnector.CheckECBlockedDates(request.StartDate, request.EndDate, DealerAppointment);
            
            return Json(result);
        }
        [HttpPost]
        [Route("check-time-slot")]
        public async Task<IActionResult> CheckTimeSlot([FromBody] CheckTimeSlot response)
        {
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);
            var dealerDetails= HttpContext.Session.GetString("DealerDetails");
            var DealerDetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerDetails);

            var dealerAppointment = HttpContext.Session.GetString("AppointmentSlotId");
            var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);

            List<TimeSlotList> result = await _appointmentconnector.CheckTimeSlot(response.Date, DealerAppointment, vehicledetails, userdetails, DealerDetails);
            if(result.Count>0)
            {
                List<TimeSlotList> datalist = new List<TimeSlotList>();
                var data = new TimeSlotList();
                for (var i =0;i<result.Count;i++) {
                    
                        data.SlotName = result[i].SlotName;
                        data.SlotID = result[i].SlotID;
                        data.TimeSlotID = result[i].TimeSlotID;
                        data.AvaiableStatus = result[i].AvaiableStatus;
                        data.AvaiableCount = result[i].AvaiableCount;
                        data.RTOCodeID = result[i].RTOCodeID;
                        data.BookedCount = result[i].BookedCount;
                        data.VehicleTypeID = result[i].VehicleTypeID;
                        datalist.Add(data);
                    
                }
                var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(datalist);
                var jsonSerializer1 = System.Text.Json.JsonSerializer.Serialize(result);
                HttpContext.Session.SetString("TimeSlot", jsonSerializer);
                HttpContext.Session.SetString("TimeSlotChecking", jsonSerializer1);
            }
            return Json(result);
        }

        [HttpPost]
        [Route("check-time-slot-re")]
        public async Task<IActionResult> CheckTimeSlotReAppointment([FromBody] CheckTimeSlot response)
        {
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);
          

            var dealerAppointment = HttpContext.Session.GetString("AppointmentSlotId");
            var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);

            List<TimeSlotList> result = await _appointmentconnector.CheckTimeSlotReAppointment(response.Date, DealerAppointment, vehicledetails, userdetails);
            if (result.Count > 0)
            {
                List<TimeSlotList> datalist = new List<TimeSlotList>();
                var data = new TimeSlotList();
                for (var i = 0; i < result.Count; i++)
                {

                    data.SlotName = result[i].SlotName;
                    data.SlotID = result[i].SlotID;
                    data.TimeSlotID = result[i].TimeSlotID;
                    data.AvaiableStatus = result[i].AvaiableStatus;
                    data.AvaiableCount = result[i].AvaiableCount;
                    data.RTOCodeID = result[i].RTOCodeID;
                    data.BookedCount = result[i].BookedCount;
                    data.VehicleTypeID = result[i].VehicleTypeID;
                    datalist.Add(data);

                }
                var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(datalist);
                var jsonSerializer1 = System.Text.Json.JsonSerializer.Serialize(result);
                HttpContext.Session.SetString("TimeSlot", jsonSerializer);
                HttpContext.Session.SetString("TimeSlotChecking", jsonSerializer1);
            }
            return Json(result);
        }
    }
}
