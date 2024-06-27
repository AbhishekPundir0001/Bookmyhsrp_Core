using BookMyHsrp.Libraries.BookingSummary.Services;
using BookMyHsrp.Libraries.VerifyPaymentDetail.Services;
using BookMyHsrp.ReportsLogics.VerifyPaymentDetails;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.AppointmentSlot.Model.AppointmentSlotModel;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static BookMyHsrp.Libraries.VerifyPaymentDetail.Models.VerifyPaymentDetailModel;

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
        public async Task<IActionResult> Payment([FromBody] Payment payment)
        {
            var vehicleDetail = HttpContext.Session.GetString("UserSession");
            var UserDetail = HttpContext.Session.GetString("UserDetail");
            var dealerAppointment = HttpContext.Session.GetString("AppointmentSlotId");
            var bookingDetails = HttpContext.Session.GetString("UserBookingDetails");
            var Vehicledetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var userdetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(UserDetail);
            var DealerAppointment = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerAppointment);
            var bookingDetail = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(bookingDetails);
            var ip = HttpContext.Request.Headers["REMOTE_ADDR"];
            var timeslot = HttpContext.Session.GetString("TimeSlotChecking");
            List<TimeSlotList> timeSlotChecking = System.Text.Json.JsonSerializer.Deserialize<List<TimeSlotList>>(timeslot);
            foreach (var slot in timeSlotChecking)
            {
                Console.WriteLine($"Slot ID: {slot.SlotID}, Slot Name: {slot.SlotName}");
            }
            var result = await _verifyPaymentDetailsConnector.Payment(Vehicledetails, userdetails, DealerAppointment, bookingDetail, ip, payment.PaymentStatus, timeSlotChecking);
           if(result!=null)
            {
                var paymentSession = new PaymentDetails();
                paymentSession.Address1= result.Address1;
                paymentSession.AppointmentType= result.AppointmentType;
                paymentSession.BMHConvenienceCharges = result.BMHConvenienceCharges;
                paymentSession.BMHHomeCharges= result.BMHHomeCharges;
                paymentSession.BasicAmount= result.BasicAmount;
                paymentSession.Basic_AmtFrm= result.Basic_AmtFrm;
                paymentSession.BharatStage= result.BharatStage;
                paymentSession.BookedFrom= result.BookedFrom;
               paymentSession.BookingType= result.BookingType;
                paymentSession.CGSTAmount= result.CGSTAmount;
                paymentSession.CGSTAmountFrm= result.CGSTAmountFrm;
                paymentSession.CGSTAmountST= result.CGSTAmountST;
                paymentSession.ChassisNo= result.ChassisNo;
                paymentSession.CustomerGSTNo= result.CustomerGSTNo;
                paymentSession.MobileNo = result.MobileNo;
                paymentSession.OwnerName = result.OwnerName;
                paymentSession.Order_No= result.Order_No;
               paymentSession.orderNo= result.orderNo;
                paymentSession.OEMID= result.OEMID;
                paymentSession.FitmentCharge= result.FitmentCharge;
                paymentSession.NetAmount= result.NetAmount;
                paymentSession.Message= result.Message;
                var jsonSerializer = System.Text.Json.JsonSerializer.Serialize(paymentSession);
                HttpContext.Session.SetString("PaymentDetails", jsonSerializer);
            }
            return Json(result);
        }

    }
}
