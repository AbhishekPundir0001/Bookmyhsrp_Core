using BookMyHsrp.Libraries.GenerateOtp.Services;
using BookMyHsrp.Libraries.OrderCancel.Models;
using BookMyHsrp.Libraries.OrderCancel.Services;
using BookMyHsrp.Libraries.ReAppointment.Model;
using BookMyHsrp.Libraries.ReAppointment.Services;
using BookMyHsrp.ReportsLogics.TrackYourOrder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static BookMyHsrp.Libraries.ReAppointment.Model.ReAppointmentModel;
using static BookMyHsrp.Libraries.TrackYoutOrder.Models.TrackYourOrderModel;


namespace BookMyHsrp.Controllers.CommonController
{
    public class ReAppointmentController : Controller
    {
        [Route("/reappointment")]
        public IActionResult ReAppointment()
        {
            return View();
        }

        private readonly IReAppointmentServices _reAppointmentServices;
        private readonly IGenerateOtpService _generateOtpService;
        public ReAppointmentController(IReAppointmentServices reappointmentservices, IGenerateOtpService generateOtpService)
        {
            _reAppointmentServices = reappointmentservices;

            _generateOtpService = generateOtpService;
        }


        [Route("/reappointmentOTP")]
        [HttpPost]
        public async Task<IActionResult> GenerateOtpOrderCancel([FromBody] OrderCancelModel.OtpModal requestdto)
        {
            var userdetails = HttpContext.Session.GetString("UserDetail");
            var userdetail = System.Text.Json.JsonSerializer.Deserialize<RootDto>(userdetails);
            var mobile = userdetail.CustomerMobile;
            var resultGot = await _generateOtpService.GenerateOtp(mobile, requestdto);
            return Json("Success");
        }


        [Route("/reappointment-details")]
        [HttpPost]
        public async Task<IActionResult> ReAppointmentDetails([FromBody] ReAppointmentModel.ReAppointment requestdto)
        {
            var jsonSerializer = "";
            var result = await _reAppointmentServices.GetOrderDetails(requestdto);
            // var result1 = await _trackYourOrderService.GetTrackYourOrderStatusSp(requestdto)
            Message res = new Message();
            if (result.Count > 0)
            {
                var firstItem = result[0];
                if (firstItem.Columns == 2)
                {
                    res.Status = "0";
                    res.message = result.Columns[0].Msg;
                    return Ok(res);
                }
                if (result[0].PlateSticker.ToUpper() == "PLATE")
                {
                    var authCheck = await _reAppointmentServices.AuthorisedReschedule(requestdto);
                    if (authCheck.Count > 0)
                    {
                        res.Status = "0";
                        res.message = "You are not Authorised to Reschedule your appointment";
                    }
                }
                else
                {
                    var authCheck = await _reAppointmentServices.AuthorisedRescheduleSticker(requestdto);
                    if (authCheck.Count > 0)
                    {
                        res.Status = "0";
                        res.message = "You are not Authorised to Reschedule your appointment";
                    }
                }
                var vehiclettype = "";

                if (result[0].BookingType.ToString().Replace("-", "").ToUpper() == "4W")
                {
                    vehiclettype = "1";
                }
                else if (result[0].BookingType.ToString().Replace("-", "").ToUpper() == "2W")
                {
                    vehiclettype = "2";
                }
                else if (result[0].BookingType.ToString().Replace("-", "").ToUpper() == "3W")
                {
                    vehiclettype = "2";
                }
                else
                {
                    vehiclettype = "";
                }




                res.Status = "1";
                res.message = "";
                var rootDto = new RootDto();
                rootDto.StateId = firstItem.HSRP_StateID.ToString();
                rootDto.PlateSticker = result[0].PlateSticker;
                rootDto.NonHomo = result[0].NonHomologVehicle;
                jsonSerializer = System.Text.Json.JsonSerializer.Serialize(rootDto);
                HttpContext.Session.SetString("UserSession", jsonSerializer);

                var rootDtoUser = new RootDto();
                rootDtoUser.OemId = firstItem.OEMID.ToString();
                rootDtoUser.VehicleTypeId = vehiclettype;
                rootDtoUser.OrderType = result[0].OrderType;
                rootDtoUser.CustomerMobile = result[0].MobileNo;

                jsonSerializer = System.Text.Json.JsonSerializer.Serialize(rootDtoUser);
                HttpContext.Session.SetString("UserDetail", jsonSerializer);

                var dealerappointment = new AppointmentSlotSessionResponse();

                dealerappointment.DealerAffixationCenterId = firstItem.affix_id.ToString();
                dealerappointment.SelectedSlotID = firstItem.SlotId.ToString();
                dealerappointment.SelectedSlotDate = firstItem.SlotBookingDate.ToString() ;
                dealerappointment.SelectedSlotTime = firstItem.SlotTime.ToString(); 
                dealerappointment.Affix = firstItem.affix_id.ToString();
                dealerappointment.DeliveryPoint = firstItem.AppointmentType;

                jsonSerializer = System.Text.Json.JsonSerializer.Serialize(dealerappointment);
                HttpContext.Session.SetString("AppointmentSlotId", jsonSerializer);

              
                var reappointmentdetails = new RootDtoReAppointment();
                reappointmentdetails.ReOrderNo = firstItem.orderno;
                reappointmentdetails.PlateSticker = firstItem.plateSticker;
                reappointmentdetails.VehicleRegNo = firstItem.VehicleRegNo;
                reappointmentdetails.OldAppointmentDate = firstItem.oldDate;
                reappointmentdetails.OldAppointmentSlot = firstItem.SlotTime;
                reappointmentdetails.ReOEMId = firstItem.oemid;
                reappointmentdetails.ReDealerAffixationCenterid = firstItem.affix_id;
                reappointmentdetails.ReSessionOwnerName = firstItem.OwnerName;
                reappointmentdetails.ReSessionMobileNo = firstItem.MobileNo;
                reappointmentdetails.ReSessionBillingAddress = firstItem.Address1;
                reappointmentdetails.ReSessionEmailID = firstItem.EmailID;
                reappointmentdetails.ReStateName = firstItem.State;
                reappointmentdetails.ReVehicleTypeid = vehiclettype;
                reappointmentdetails.ReDeliveryPoint = firstItem.AppointmentType;
                reappointmentdetails.ReStateId = firstItem.HSRP_StateID.ToString();
                jsonSerializer = System.Text.Json.JsonSerializer.Serialize(reappointmentdetails);
                HttpContext.Session.SetString("reappointmentdetails", jsonSerializer);
                return Json(res);

            }
            return Json("OK");
        }
    }
}
    

