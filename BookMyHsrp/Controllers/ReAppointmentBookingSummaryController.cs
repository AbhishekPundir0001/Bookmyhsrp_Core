using BookMyHsrp.Libraries.BookingSummary.Services;
using BookMyHsrp.Libraries.ReAppointmentBookingSummary.Services;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.BookingSummary.Model.BookingSummaryModel;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
namespace BookMyHsrp.Controllers
{
    public class ReAppointmentBookingSummaryController : Controller
    {
        private readonly IBookingSummaryService _bookingSummaryService;
        private readonly IReAppointmentBookingSummaryServices _reappointmentBookingSummaryService;
        public ReAppointmentBookingSummaryController(ILogger<ReAppointmentBookingSummaryController> logger, IReAppointmentBookingSummaryServices reappointmentBookingSummaryService, IBookingSummaryService bookingSummaryService)
        {
            _reappointmentBookingSummaryService = reappointmentBookingSummaryService;
            _bookingSummaryService = bookingSummaryService;

        }   
        [Route("/reappointmentbookingsummary")]
        public IActionResult ReAppointmentBookingSummary()
        {
            return View();
        }

        [HttpPost]
        [Route("/reappointment-bookingSummary-confirmation")]

        public async Task<IActionResult> BookingSummaryConfirmation([FromBody] BookingDate date)
        {
            var bookingDetails = new BookingDetails();
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var dealerAppointment = HttpContext.Session.GetString("AppointmentSlotId");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);
            var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);
            var result = await _bookingSummaryService.BookingSummaryConfirmation(DealerAppointment);
            if (result.Count > 0)
            {

                bookingDetails.BharatStage = userdetails.BhartStage;
                bookingDetails.DealerAffixationCenterId = DealerAppointment.DealerAffixationCenterId;
                bookingDetails.ChassisNo = vehicledetails.ChassisNo;
                bookingDetails.EngineNo = vehicledetails.EngineNo;
                bookingDetails.FuelType = vehicledetails.FuelType;
                bookingDetails.PlateSticker = vehicledetails.PlateSticker;
                bookingDetails.VehicleCategory = vehicledetails.VehicleCategory;
                bookingDetails.VehicleClass = vehicledetails.VehicleClass;
                bookingDetails.VehicleRegNo = vehicledetails.VehicleRegNo;
                bookingDetails.CustomerMobile = userdetails.CustomerMobile;
                bookingDetails.VehicleType = userdetails.VehicleType;
                bookingDetails.CustomerBillingAddress = userdetails.CustomerBillingAddress;
                bookingDetails.DealerAffixationCenterAddress = result[0].DealerAffixationCenterAddress;
                bookingDetails.DealerAffixationCenterName = result[0].DealerAffixationCenterName;
                bookingDetails.RTOLocationName = result[0].RTOLocationName;
                bookingDetails.RTOLocationID = result[0].RTOLocationID;
                bookingDetails.StateID = result[0].StateID;
                bookingDetails.DealerID = result[0].DealerID;
                bookingDetails.OemName = result[0].OemName;
                bookingDetails.OemID = result[0].OemID;
                bookingDetails.SlotDate = userdetails.SlotDate;
                bookingDetails.SlotTime = userdetails.SlotTime;
                var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(bookingDetails);
                HttpContext.Session.SetString("UserBookingDetails", jsonSerializer);


            }
            return Json(bookingDetails);
        }
    }
}
