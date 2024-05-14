using BookMyHsrp.Libraries.GenerateOtp.Services;
using BookMyHsrp.Libraries.HsrpState.Services;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.Libraries.ResponseWrapper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Controllers.CommonController
{
    [Route("api/v1/otp")]
    public class GenerateOTPController : ControllerBase
    {
        private readonly IGenerateOtpService _generateOtpService;
        public GenerateOTPController(IGenerateOtpService generateOtpService)
        {

            _generateOtpService = generateOtpService;
        }

        [HttpGet]
        public async Task<dynamic> GenerateOtp()
        {
           var vehicleDetail =  HttpContext.Session.GetString("UserSession");
           var details =  HttpContext.Session.GetString("UserDetail");
            var data= System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(details);
            var vehivledetailsData= System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(vehicleDetail);
            var mobile = data.CustomerMobile;
            var resultGot = await _generateOtpService.GenerateOtp(mobile, vehivledetailsData);
            return resultGot;
        }

        [HttpGet]
        [Route("otpConfirmation/{OTP}")]
        public async Task<IActionResult> ConfirmOTP(string OTP)
        {

            var resultGot = await _generateOtpService.ConfirmOTP(OTP);
            if (resultGot.Message == "Success")
            {
                return Ok(
                    new Response<dynamic>(resultGot, false,
                        "Data Received."));
            }
            else
            {
                return Ok(
                    new Response<dynamic>(resultGot, false,
                        "Wrong Otp"));
            }
        }
    }
    
}
