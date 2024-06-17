using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.GenerateOtp.Models;
using BookMyHsrp.Libraries.GenerateOtp.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Web;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static System.Net.WebRequestMethods;

namespace BookMyHsrp.Libraries.GenerateOtp.Services
{
    public class GenerateOtpService : IGenerateOtpService
    {
        private readonly FetchDataAndCache _fetchDataAndCache;
        private readonly DapperRepository _databaseHelper;
        private readonly DapperRepository _databaseHelperPrimary;
        private readonly string _connectionString;
        string msg = string.Empty;
        string OTPVerifyStatus = string.Empty;
        ResponseDto response = new ResponseDto();
        public GenerateOtpService(IOptionsSnapshot<ConnectionString> connectionStringOptions, FetchDataAndCache fetchDataAndCache)
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;

            _databaseHelper = new DapperRepository(_connectionString);
            //_appSettings = appSettings;
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelperPrimary = new DapperRepository(_connectionString);
            //_httpContextAccessor = httpContextAccessor;
            _fetchDataAndCache = fetchDataAndCache;
        }

        public async Task<dynamic> GenerateOtp(string mobile, dynamic data)
        {
            var generateOtp = new GenerateOtpResponse();
            generateOtp.Otp="";
            try
            {
                if (mobile == null)
                {
                    throw new ArgumentException("Vehicle Details didn't match");

                }
                else
                {
                    string otpMobile = "";
                    var otp = generateOtp.Otp;
                    if (otp =="")
                    {
                       generateOtp.OTPno= "N";
                        otpMobile = generateOtp.OTPno;
                    }
                    if (otpMobile == "N")
                    {
                        SendOtp(mobile,data);
                        response.Message = "Success";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        private async void SendOtp(string mob,dynamic data)
        {
            int Rno = GenerateRandomNo();
            string OTPno = Rno.ToString();
            string OrderNo = string.Empty;
            string VehicleRegNo = data.VehicleRegNo;
            string MobileNo = mob;
            //string sms = "Your OTP code is: " + OTPno + "\n(Team Rosmerta)";
            string sms = "Your One Time Password (OTP) is " + OTPno + ". Don't share it with anyone.This is for verification of your mobile no. for your transaction in BookMyHSRP.Team Rosmerta";
            //string sms = "Your One Time Password (OTP) is " + OTPno + ". Don't share it with anyone.\nThis is for verification of your mobile no. for your transaction in BookMyHSRP.";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string response =SMSSend(MobileNo, sms, "1007823291653661531", "BMHSRP");
            //string response = Utils.SendOtp(MobileNo, "1007823291653661531", OTPno);
            sms = sms.Replace("'", "");

            var parameter = new DynamicParameters();
            parameter.Add("@OrderNo", OrderNo);
            parameter.Add("@VehicleRegNo", VehicleRegNo);
            parameter.Add("@MobileNo", MobileNo);
            parameter.Add("@Sms", sms);
            parameter.Add("@Response", response);
            parameter.Add("@OTPno", OTPno);
            parameter.Add("@OTPVerifyStatus", OTPVerifyStatus);
           
            var result = _databaseHelperPrimary.QueryAsync<dynamic>(GenerateOtpQueries.SMSLogSave, parameter);
            var OtpNo = OTPno;
            await _fetchDataAndCache.SetStringInCache("OtpNo", OtpNo);
            var result1 = _databaseHelperPrimary.QueryAsync<dynamic>(GenerateOtpQueries.InsertSMSLog2, parameter);

           #region  WhatsApp Sending
            string[] Params = { OTPno };
            string Templateid = "bmhsrp_otp";


            #endregion

            #region Sending Mail
            string _IsEmail = "True";
            var emailId=await _fetchDataAndCache.GetStringFromCache("EmailId");
          
            #endregion
        }

        public static String SMSSend(string mobile, string SMSText, string TemplateID, string SenderIDHeader)
        {
            //txtAuthKey.Text="343817AaX3yb5BY4rI5f967427P1";
            //txtSenderId.Text ="BMHSRP;    // "dlhsrp";
            string result;
            //Your authentication key
            string authKey = "343817AaX3yb5BY4rI5f967427P1";
            //Multiple mobiles numbers separated by comma
            string mobileNumber = mobile;
            //Sender ID,While using route4 sender id should be 6 characters long.
            string senderId = SenderIDHeader;// "BMHSRP";
            //Your message to send, Add URL encoding here.
            string message = HttpUtility.UrlEncode(SMSText);
            string country = "91";
            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
            sbPostData.AppendFormat("&message={0}", message);
            sbPostData.AppendFormat("&sender={0}", senderId);
            sbPostData.AppendFormat("&country={0}", country);
            sbPostData.AppendFormat("&route={0}", "4");
            sbPostData.AppendFormat("&DLT_TE_ID={0}", TemplateID);


            try
            {
                //Call Send SMS API
                string sendSMSUri = "http://api.msg91.com/api/sendhttp.php";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response
                reader.Close();
                response.Close();
                result = responseString;
            }
            catch (SystemException ex)
            {
                result = ex.Message.ToString();
            }

            return result;
        }


        public async Task<string> OrdercancelSMSSend(string mobile, string SMSText, string TemplateID, string SenderIDHeader)
        {
            ResponseDto response = new ResponseDto();
            string MobileNo = mobile;
            string sms = SMSText;
            string id = TemplateID;
            string idHeader = SenderIDHeader;
            string message = SMSSend(MobileNo, sms, id, idHeader);
            return message;
        }
        public async Task<dynamic> ConfirmOTP(string otp)
        {
            var otpno = await _fetchDataAndCache.GetStringFromCache("OtpNo");
            if(otpno==null)
            {
                throw new ValidationException("Something Wrong");
            }
            OTPVerifyStatus = "N";
            try
            {
                if (otp == null)
                {
                    throw new ValidationException("Please Enter OTP ..!!");
                }
                else
                {
                    if (otp == otpno)
                    {
                        string OrderNo = string.Empty;
                        string VehicleRegNo = await _fetchDataAndCache.GetStringFromCache("RegNo");
                        string MobileNo = await _fetchDataAndCache.GetStringFromCache("MobileNo");
                        var parameter = new DynamicParameters();
                        parameter.Add("@MobileNo", MobileNo);
                        parameter.Add("@OTP", otpno);
                        var result = _databaseHelperPrimary.QueryAsync<dynamic>(GenerateOtpQueries.SaveOtp, parameter);

                        if (result != null)
                        {
                            int status = 0;
                            foreach (var data in result.Result)
                            {
                                status = data.status;
                            }
                            if (status==0)
                            {
                                throw new ValidationException("Something Wrong ..!!");
                            }
                        }

                        var parameter2 = new DynamicParameters();
                        parameter2.Add("@MobileNo", MobileNo);
                        parameter2.Add("@OTP", otp);
                        var updatesmsLog = _databaseHelperPrimary.QueryAsync<dynamic>(GenerateOtpQueries.UpdateSMSLog, parameter2);



                        OTPVerifyStatus = "Y";
                        await _fetchDataAndCache.SetStringInCache("IsOTPVerify", "Y");

                    }
                    else
                    {

                        response.Message = "Wrong Otp";
                    }
                    if(OTPVerifyStatus == "Y")
                    {
                        response.Message = "Success";
                    }
                }

            }
            catch (Exception ex) 
            {
                throw new ArgumentException(ex.Message);

            }
            return response;

        }

    }
  
}
