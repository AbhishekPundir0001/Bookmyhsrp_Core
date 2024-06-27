using BookMyHsrp.Libraries.PaymentReceipt.Services;
using BookMyHsrp.ReportsLogics.AppointmentSlot;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static BookMyHsrp.Libraries.Receipt.Models.ReceiptModels;
using static BookMyHsrp.Libraries.VerifyPaymentDetail.Models.VerifyPaymentDetailModel;
using static BookMyHsrp.Libraries.VerifyPaymentDetail.Models.VerifyPaymentDetailModel.PaymentDetails;

namespace BookMyHsrp.Controllers.CommonController
{
    public class PaymentReceiptController : Controller
    {
        private readonly ILogger<PaymentReceiptController> _logger;
        private readonly IPaymentReceiptService _paymentReceiptService;
        public PaymentReceiptController(ILogger<PaymentReceiptController> logger, IPaymentReceiptService paymentReceiptService)
        {
            _paymentReceiptService = paymentReceiptService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [Route("/paymentRecipt")]
        public IActionResult PaymentReceipt()
        {
            return View();
        }
        [HttpGet]
        [Route("/paymentDetails")]
        public async Task<IActionResult> PaymentDetail()
        {
            var detailsPayment = new PaymentDetails();
           var bookingDetails = HttpContext.Session.GetString("UserBookingDetails");
           var dealerDetails =   HttpContext.Session.GetString("DealerDetails");
           var userDetails = HttpContext.Session.GetString("UserSession");
           var details =  HttpContext.Session.GetString("UserDetail");
            //var timeSlot = HttpContext.Session.GetString("TimeSlot");
           //var timeslotchecking=  HttpContext.Session.GetString("TimeSlotChecking");
          var paymentDetails    =  HttpContext.Session.GetString("PaymentDetails");
          var appointmentSlot    =  HttpContext.Session.GetString("AppointmentSlotId");
            
            var BookingDetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(bookingDetails);
            var UserDetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(userDetails);
            var Details = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(details);
            //var  TimeSlot = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(timeSlot);
            //var  TimeSlotChecking = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(timeslotchecking);
            var PaymentDetails1 = System.Text.Json.JsonSerializer.Deserialize<PaymentDetails>(paymentDetails);
            var DealerDetails = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(dealerDetails);
            var AppointmentSlot = System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(appointmentSlot);
            detailsPayment.CustomerAddress1 = PaymentDetails1.Address1;
            detailsPayment.BharatStage = PaymentDetails1.BharatStage;
            detailsPayment.ChassisNo= UserDetails.ChassisNo;
            detailsPayment.VehicleRegNo = UserDetails.VehicleRegNo;
            detailsPayment.EngineNo= UserDetails.EngineNo;
            detailsPayment.NetAmount= PaymentDetails1.NetAmount;
            detailsPayment.FuelType= PaymentDetails1.FuelType;
            detailsPayment.Message  = PaymentDetails1.Message;
            detailsPayment.MobileNo = PaymentDetails1.MobileNo;
            detailsPayment.DealerAffixationCenterName = BookingDetails.DealerAffixationCenterName;
            detailsPayment.DealerAffixationAddress= BookingDetails.DealerAffixationCenterAddress;
            detailsPayment.EmailID= Details.CustomerEmail;
            detailsPayment.Order_No = PaymentDetails1.Order_No;
            detailsPayment.CustomerName = Details.CustomerName;
            detailsPayment.DealerAffixationCenterContactPerson = AppointmentSlot.DealerAffixationCenterContactPerson;
            detailsPayment.DealerAffixationCenterContactNo = AppointmentSlot.DealerAffixationCenterContactNo;
            detailsPayment.orderNo = PaymentDetails1.orderNo;
            var update =await _paymentReceiptService.UpdateStatusOfPayment(detailsPayment.orderNo);
            return Json(detailsPayment);
        }
        //[Route("printReceipt")]
        //[HttpPost]
        //public  async Task<IActionResult> PaymentReceipt([FromBody] PaymentReceipt paymentReceipt)
        //{

        //}

    }
}
