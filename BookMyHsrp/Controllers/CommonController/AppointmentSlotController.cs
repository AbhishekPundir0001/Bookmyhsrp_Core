﻿using BookMyHsrp.Libraries.AppointmentSlot.Model;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.ReportsLogics.AppointmentSlot;
using BookMyHsrp.ReportsLogics.DealerDelivery;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.AppointmentSlot.Model.AppointmentSlotModel;
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
            var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);

            var result = await _appointmentconnector.GetCheckAppointmentDate(vehicledetails, userdetails, DealerAppointment);
            return Json(result);
        }

    }
}
