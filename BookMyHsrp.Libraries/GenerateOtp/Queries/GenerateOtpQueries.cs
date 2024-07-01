using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.GenerateOtp.Queries
{
    public class GenerateOtpQueries
    {
        public static readonly string SMSLogSave = "insert into [BookMyHSRP].dbo.Appointment_SMSDetails (OwnerName,VehicleRegNo,MobileNo,SMSText,SentResponseCode,SentDateTime) values(@OrderNo,@VehicleRegNo,@MobileNo,@Sms,@Response,GetDate())";
        public static readonly string InsertSMSLog2 = "exec sp_SMSLogInsert @OrderNo ,@VehicleRegNo,@MobileNo,@OTPno, @OTPVerifyStatus,'Plate'";
        public static readonly string SaveOtp = "exec ValidateOTP10min @MobileNo,@OTP";
        public static readonly string UpdateSMSLog = "exec sp_SMSLogUpdate @MobileNo,@OTP";
    }
}
