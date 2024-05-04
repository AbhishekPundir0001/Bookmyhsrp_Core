using BookMyHsrp.Libraries.GenerateOtp.Services;
using BookMyHsrp.Libraries.HsrpState.Services;
using Microsoft.AspNetCore.Mvc;

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

            var resultGot = await _generateOtpService.GenerateOtp();
            return resultGot;
        }

        [HttpGet]
        [Route("otpConfirmation/{OTP}")]
        public async Task<dynamic> ConfirmOTP(string OTP)
        {

            var resultGot = await _generateOtpService.ConfirmOTP(OTP);
            return resultGot;
        }
    }
    
}
