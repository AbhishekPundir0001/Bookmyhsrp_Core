using BookMyHsrp.Libraries.HsrpState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.GenerateOtp.Services
{
    public interface IGenerateOtpService
    {
        Task<dynamic> GenerateOtp(string mobile,dynamic data);
        Task<dynamic> ConfirmOTP(string otp);
        Task<string> OrdercancelSMSSend(string mobile, string SMSText, string TemplateID, string SenderIDHeader);
    }
}
