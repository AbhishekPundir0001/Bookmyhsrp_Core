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
           var details =  HttpContext.Session.GetString("SessionDetail");
            var data= System.Text.Json.JsonSerializer.Deserialize<GetSessionBookingDetails>(details);
            var mobile = data.MobileNo;
            var resultGot = await _generateOtpService.GenerateOtp(mobile);
            return resultGot;
        }

        [HttpGet]
        [Route("otpConfirmation/{OTP}")]
        public async Task<IActionResult> ConfirmOTP(string OTP)
        {

            var resultGot = await _generateOtpService.ConfirmOTP(OTP);
            return Ok(
                new Response<dynamic>(resultGot, false,
                    "Data Received."));
        }
    }
    
}
