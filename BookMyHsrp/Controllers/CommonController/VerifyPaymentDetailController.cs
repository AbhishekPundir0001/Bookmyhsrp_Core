using BookMyHsrp.Libraries.BookingSummary.Services;
using BookMyHsrp.Libraries.VerifyPaymentDetail.Services;
using BookMyHsrp.ReportsLogics.VerifyPaymentDetails;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers.CommonController
{
    public class VerifyPaymentDetailController : Controller
    {
        private readonly ILogger<VerifyPaymentDetailController> _logger;
        private readonly IVerifyPaymentDetailService _verifyPaymentDetailService;
        private readonly VerifyPaymentDetailsConnector _verifyPaymentDetailsConnector;
        public VerifyPaymentDetailController(ILogger<VerifyPaymentDetailController> logger, VerifyPaymentDetailsConnector verifyPaymentDetailsConnector)
        {
            _verifyPaymentDetailsConnector = verifyPaymentDetailsConnector;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [Route("verifyPaymentDetail")]
        public IActionResult VerifyPaymentDetail()
        {
            return View();
        }

        [Route("getVerifyPaymentDetail")]
        public async Task<IActionResult> GetVerifyPaymentDetail()
        {
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var dealerAppointment = HttpContext.Session.GetString("AppointmentSlotId");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);
            var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);
            var customerName = userdetails.CustomerName;
            var billingAddress= userdetails.CustomerBillingAddress;
            var mobileNumber = userdetails.CustomerMobile;
            var customerEmail = userdetails.CustomerEmail;
            var result = await _verifyPaymentDetailsConnector.CheckSuperTagRate(vehicledetails, userdetails, DealerAppointment);
            return Json(result);
        }
        [HttpPost]
        [Route("payment")]
        public async Task<IActionResult> Payment([FromBody] string Payment)
        {
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var dealerAppointment = HttpContext.Session.GetString("AppointmentSlotId");
            var bookingDetails = HttpContext.Session.GetString("UserBookingDetails");
            var vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);
            var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);
            var bookingDetail = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(bookingDetails);
            var ip = HttpContext.Request.Headers["REMOTE_ADDR"];
            
            var result = await _verifyPaymentDetailsConnector.Payment(vehicledetails, userdetails, DealerAppointment, bookingDetail, ip);
            return Json(result);
        }

    }
}
