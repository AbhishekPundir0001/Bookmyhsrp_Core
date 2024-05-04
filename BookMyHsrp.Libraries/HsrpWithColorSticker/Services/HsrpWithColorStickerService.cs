using BookMyHsrp.Libraries.CustomValidationException;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Queries;
using BookMyHsrp.Libraries.ResponseWrapper.Models;
using BookMyHsrp.Utility;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BookMyHsrp.Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel.ResponseDto;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BookMyHsrp.Dapper;

namespace BookMyHsrp.Libraries.HsrpWithColorSticker.Services
{
    public class HsrpWithColorStickerService : IHsrpWithColorStickerService
    {
        private readonly FetchDataAndCache _fetchDataAndCache;
        //private readonly AppSettings _appSettings;
        private readonly DapperRepository _databaseHelper;
        private readonly DapperRepository _databaseHelperPrimary;
        private readonly DynamicDataDto _dynamicDataDto;
        private readonly string _connectionString;
        private readonly string _vehicleStatusAPI;
        private readonly string _oemId;
        string msg = string.Empty;
        // private readonly IHttpContextAccessor _httpContextAccessor;

        public HsrpWithColorStickerService(IOptionsSnapshot<ConnectionString> connectionStringOptions, FetchDataAndCache fetchDataAndCache, IOptionsSnapshot<DynamicDataDto> dynamicData )
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;

            _databaseHelper = new DapperRepository(_connectionString);
            //_appSettings = appSettings;
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelperPrimary = new DapperRepository(_connectionString);
            //_httpContextAccessor = httpContextAccessor;
            _fetchDataAndCache = fetchDataAndCache;
            _vehicleStatusAPI = dynamicData.Value.VehicleStatusAPI;
            _oemId = dynamicData.Value.OemID;
        }
        public async Task<dynamic> VahanInformation(dynamic requestDto)
        {

            ResponseDto responseDto = new ResponseDto();
            byte[] nonHomo;
            VehicleDetails details = new VehicleDetails();
            //ISession session = null;
            string hsrpStateShortName = string.Empty;
            int oemId = 0;
            string imagepath = string.Empty;
            string IsOemRtoMapped = string.Empty;

            var parameters = new DynamicParameters();
            parameters.Add("@StateId", requestDto.StateId);

            var result = await _databaseHelper.QueryAsync<dynamic>(
                  HsrpWithColorStickerQueries.GetStateDetails, parameters);
            // return result;

            foreach (var row in result)
            {
                hsrpStateShortName = row.HSRPStateShortName;


            }



            if (requestDto.RegistrationNo == "DL10CG7191")
            {

            }
            else
            {

                var parameters1 = new DynamicParameters();

                parameters1.Add("@RegistrationNo", requestDto.RegistrationNo);
                parameters1.Add("@ChassisNo", requestDto.ChassisNo);
                parameters1.Add("@EngineNo", requestDto.EngineNo);
                parameters1.Add("@StateId", requestDto.StateId);
                var data = await _databaseHelper.QueryAsync<dynamic>(
                    HsrpWithColorStickerQueries.GetBookingHistory, parameters1);
                if (data != null && data.Any())
                {

                    responseDto.Message = "Order for this registration number already exists";

                }


            }
            var newOrder = await RosmertaApi(requestDto.RegistrationNo.ToUpper().Trim(), requestDto.ChassisNo.ToUpper().Trim(), requestDto.EngineNo.ToUpper().Trim(), "5UwoklBqiW");
             details = JsonConvert.DeserializeObject<VehicleDetails>(newOrder);
            if (details != null)
            {

                if (details.stateCd != null)
                {


                    if (details.stateCd.ToLower().StartsWith(hsrpStateShortName.ToString().ToLower()) == false)
                    {
                        responseDto.Message = "Please input Correct Registration Number";

                    }
                }
                else
                {
                    responseDto.Message = "Your vehicle detail didn't match with vahan service";
                }

                if (details.message == "Vehicle details available in Vahan")
                {

                    nonHomo = Encoding.UTF8.GetBytes("N");

                    await _fetchDataAndCache.SetStringInCache("RegNo", requestDto.RegistrationNo.ToUpper().Trim());
                  
                    //session.Set("ChasisNo", requestDto.ChassisNo.ToUpper().Trim());
                    //session.Set("EngineNo", requestDto.EngineNo.ToUpper().Trim());

                    //session.Set("SessionRegNo", requestDto.RegistrationNo.ToUpper().Trim());
                    //session.Set("SessionChasisno", requestDto.ChassisNo.ToUpper().Trim());
                    //session.Set("SessionEngno", requestDto.EngineNo.ToUpper().Trim());
                    //session.Set("NonHomo", nonHomo);

                    var parameters2 = new DynamicParameters();
                    parameters2.Add("@RegistrationNo", requestDto.RegistrationNo.ToUpper());
                    parameters2.Add("@ChassisNo", requestDto.ChassisNo.ToUpper());
                    parameters2.Add("@EngineNo", requestDto.EngineNo.ToUpper());
                    parameters2.Add("@Fuel", details.fuel);
                    parameters2.Add("@Norms", details.norms);
                    parameters2.Add("@VehicleCategory", details.vchCatg);
                    parameters2.Add("@VehicleType", details.vchType);
                    parameters2.Add("@Maker", details.maker);
                    parameters2.Add("@ResponseJson", newOrder);
                    parameters2.Add("@RegistrationDate", details.regnDate);
                    parameters2.Add("@HsrpFrontLasserCode", details.hsrpFrontLaserCode);
                    parameters2.Add("@HsrpRearLasserCode", details.hsrpRearLaserCode);
                    var insertVahanLog = await _databaseHelper.QueryAsync<dynamic>(
                    HsrpWithColorStickerQueries.InsertVahanLog, parameters2);

                    string _maker = string.Empty;
                    if (details.maker.Trim() == "HONDA CARS INDIA LTD" && details.vchCatg == "2WN")
                    {
                        details.maker = "HONDA MOTORCYCLE AND SCOOTER INDIA (P) LTD";
                    }
                    var parameters3 = new DynamicParameters();
                    parameters3.Add("@MakerName", details.maker);
                    var GetOemId = await _databaseHelper.QueryAsync<dynamic>(HsrpWithColorStickerQueries.GetOemId, parameters3);
                    if (GetOemId.Any())
                    {
                        foreach (var row in GetOemId)
                        {
                            imagepath = row.oem_logo;

                            oemId = row.Oemid;
                            var oemid = oemId.ToString();
                            _fetchDataAndCache.SetStringInCache("ImagePath", imagepath);
                            _fetchDataAndCache.SetStringInCache("OemId", oemid);
                        }
                        if (oemId == 20)
                        {
                            oemId = 272;
                            _fetchDataAndCache.SetStringInCache("OemId", oemId.ToString());
                        }
                    }
                    else
                    {
                        responseDto.NonHomo = "N";
                        responseDto.Status = "0";
                        responseDto.ResponseData = details;
                        responseDto.Message = "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker";
                        _fetchDataAndCache.SetStringInCache("NonHomo", responseDto.NonHomo);
                        _fetchDataAndCache.SetStringInCache("Status", responseDto.Status);
                        Encoding.UTF8.GetBytes("N");

                        return responseDto;

                    }
                    responseDto.NonHomo = "N";
                    responseDto.Status = "1";
                    responseDto.ResponseData = details;
                    responseDto.Message = details.message;
                    _fetchDataAndCache.SetStringInCache("NonHomo", responseDto.NonHomo);
                    _fetchDataAndCache.SetStringInCache("Status", responseDto.Status);
                    return responseDto;

                }
                else if (details.message == "Vehicle details available in Vahan but Maker of this vehicle Not Present in HOMOLOGATION" || details.message == "Vehicle details available in Vahan but OEM/Manufacturer (Homologation) of this vehicle have not authorized you for the State/RTO of this vehicle, please contact respective OEM")
                {
                    var parameters2 = new DynamicParameters();
                    parameters2.Add("@RegistrationNo", requestDto.RegistrationNo.ToUpper());
                    parameters2.Add("@ChassisNo", requestDto.ChassisNo.ToUpper());
                    parameters2.Add("@EngineNo", requestDto.EngineNo.ToUpper());
                    parameters2.Add("@Fuel", details.fuel);
                    parameters2.Add("@Norms", details.norms);
                    parameters2.Add("@VehicleCategory", details.vchCatg);
                    parameters2.Add("@VehicleType", details.vchType);
                    parameters2.Add("@Maker", details.maker);
                    parameters2.Add("@ResponseJson", newOrder.ResponseJson);
                    parameters2.Add("@RegistrationDate", details.regnDate);
                    parameters2.Add("@HsrpFrontLasserCode", details.hsrpFrontLaserCode);
                    parameters2.Add("@HsrpRearLasserCode", details.hsrpRearLaserCode);
                    parameters2.Add("@MakerName", details.maker);
                    var insertVahanLogQuery = await _databaseHelper.QueryAsync<dynamic>(
                    HsrpWithColorStickerQueries.insertVahanLogQuery, parameters2);

                    var checkMappingInHsrpOem = await _databaseHelper.QueryAsync<dynamic>(
                   HsrpWithColorStickerQueries.checkMappingInHsrpOem, parameters2);

                    foreach (var row in checkMappingInHsrpOem)
                    {
                        nonHomo = Encoding.UTF8.GetBytes("Y");
                        imagepath = row.oem_logo;
                        oemId = row.Oemid;
                    }
                    if (checkMappingInHsrpOem != null && checkMappingInHsrpOem.Any())
                    {
                        var parameter = new DynamicParameters();
                        parameter.Add("@RegistrationNo", requestDto.RegistrationNo.ToUpper());
                        var oemRtoMapping = await _databaseHelper.QueryAsync<dynamic>(
                        HsrpWithColorStickerQueries.OemRtoMapping, parameter);

                        if (oemRtoMapping != null && oemRtoMapping.Any())
                        {
                            foreach (var row in oemRtoMapping)
                            {
                                IsOemRtoMapped = row.IsOemRtoMapped;
                            }
                            if (IsOemRtoMapped == "Y")
                            {
                                int NonHomoCount = 0;
                                string NonHomo = "5,18,1,9,15,27";
                                string[] sp_NonHomo = NonHomo.Split(',');
                                for (int i = 0; i < sp_NonHomo.Length; i++)
                                {
                                    if (sp_NonHomo[i].ToString() == requestDto.Stateid)
                                    {
                                        NonHomoCount++;
                                    }
                                }
                                string _NonHomoOemId = "1064,2";
                                string[] sp_NonHomoOemId = _NonHomoOemId.Split(',');
                                if (_NonHomoOemId != null && details.message == "Vehicle details available in Vahan but Maker of this vehicle Not Present in HOMOLOGATION" || details.message == "Vehicle details available in Vahan but OEM/Manufacturer (Homologation) of this vehicle have not authorized you for the State/RTO of this vehicle" && _NonHomoOemId.Contains(oemId.ToString()))
                                {
                                    responseDto.NonHomo = "Y";
                                    responseDto.Status = "1";
                                    responseDto.ResponseData = details;
                                    responseDto.Message = details.message;
                                    _fetchDataAndCache.SetStringInCache("NonHomo", responseDto.NonHomo);
                                    _fetchDataAndCache.SetStringInCache("Status", responseDto.Status);
                                    return responseDto;
                                }
                                else
                                {
                                    responseDto.NonHomo = "N";
                                    responseDto.Status = "O";
                                    responseDto.Message = "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                                    _fetchDataAndCache.SetStringInCache("NonHomo", responseDto.NonHomo);
                                    _fetchDataAndCache.SetStringInCache("Status", responseDto.Status);
                                    return responseDto;
                                }
                            }
                            else
                            {
                                responseDto.NonHomo = "N";
                                responseDto.Status = "O";
                                responseDto.Message = "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                                _fetchDataAndCache.SetStringInCache("NonHomo", responseDto.NonHomo);
                                _fetchDataAndCache.SetStringInCache("Status", responseDto.Status);
                                return responseDto;
                            }
                        }

                        else
                        {
                            responseDto.NonHomo = "N";
                            responseDto.Status = "O";
                            responseDto.Message = "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                            _fetchDataAndCache.SetStringInCache("NonHomo", responseDto.NonHomo);
                            _fetchDataAndCache.SetStringInCache("Status", responseDto.Status);
                            return responseDto;
                        }


                    }
                    else
                    {

                        responseDto.NonHomo = "N";
                        responseDto.Status = "O";
                        responseDto.Message = "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                        _fetchDataAndCache.SetStringInCache("NonHomo", responseDto.NonHomo);
                        _fetchDataAndCache.SetStringInCache("Status", responseDto.Status);
                        return responseDto;
                    }
                }
                else
                {
                    responseDto.NonHomo = "N";
                    _fetchDataAndCache.SetStringInCache("NonHomo", responseDto.NonHomo);

                    var parameter = new DynamicParameters();
                    parameter.Add("@RegistrationNo", requestDto.RegistrationNo.ToUpper());
                    parameter.Add("@ChassisNo", requestDto.ChassisNo.ToUpper());
                    parameter.Add("@EngineNo", requestDto.EngineNo.ToUpper());
                    parameter.Add("@Fuel", details.fuel);
                    parameter.Add("@Norms", details.norms);
                    parameter.Add("@VehicleCategory", details.vchCatg);
                    parameter.Add("@VehicleType", details.vchType);
                    parameter.Add("@Maker", details.maker);
                    parameter.Add("@ResponseJson", newOrder.ResponseJson);
                    parameter.Add("@RegistrationDate", details.regnDate);
                    parameter.Add("@HsrpFrontLasserCode", details.hsrpFrontLaserCode);
                    parameter.Add("@HsrpRearLasserCode", details.hsrpRearLaserCode);
                    var insertVahanLogQuery = await _databaseHelper.QueryAsync<dynamic>(
                   HsrpWithColorStickerQueries.insertVahanLogQuery, parameter);

                }



            }
            else
            {


                responseDto.Status = "O";
                responseDto.Message = "Your Vehicle Data Not Pulled From Vahan Please Try After Some Time";
                return responseDto;

            }

            return responseDto;
        }






        public async Task<dynamic> RosmertaApi(string VehicleRegNo, string ChassisNo, string EngineNo, string Key)
        {
            string html = string.Empty;
            string decryptedString = string.Empty;
            try
            {
                string vehicleapi = _vehicleStatusAPI;
                string url = @"" + vehicleapi + "?VehRegNo=" + VehicleRegNo + "&ChassisNo=" + ChassisNo + "&EngineNo=" + EngineNo + "&X=" + Key + "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = await reader.ReadToEndAsync();
                }


            }
            catch (Exception ex)
            {
                throw new ValidationException("Vahan is not working");

            }
            return html;

        }

        public async Task<dynamic> SessionBookingDetails(dynamic requestDto)
        {
            ResponseDto response = new ResponseDto();
            string nonHomo = string.Empty;

           await _fetchDataAndCache.SetStringInCache("IsOTPVerify", "N");
             _fetchDataAndCache.SetStringInCache("OTPno", null);
            await _fetchDataAndCache.SetStringInCache("VehicleType_imgPath", "www");
            await _fetchDataAndCache.SetStringInCache("OEMImgPath", "www");
            await _fetchDataAndCache.SetStringInCache("OrderType", "OB");
            try
            {
                if (requestDto.BharatStage == "" || requestDto.BharatStage == null)
                {
                    throw new ArgumentException("Please select Bharat Stage value");

                }
                else if (requestDto.RegistrationDate == "" || requestDto.RegistrationDate == null)
                {
                    throw new ArgumentException("Please provide Registration date");

                }
                else if (requestDto.RegistrationNo == "" || requestDto.RegistrationNo == null)
                {
                    throw new ArgumentException("Please provide Registration Number");

                }
                else if (requestDto.ChassisNo == "" || requestDto.ChassisNo == null)
                {
                    throw new ArgumentException("Please provide Chassis Number");

                }
                else if (requestDto.EngineNo == "" || requestDto.EngineNo == null)
                {
                    throw new ArgumentException("Please provide Engine Number");

                }
                else if (requestDto.OwnerName == "" || requestDto.OwnerName == null)
                {
                    throw new ArgumentException("Please provide Owner Name");

                }
                else if (requestDto.EmailId == "" || requestDto.EmailId == null)
                {
                    throw new ArgumentException("Please provide Email Id");

                }
                else if (requestDto.MobileNo == "" || requestDto.MobileNo == null)
                {
                    throw new ArgumentException("Please provide Mobile Number");

                }
                else if (requestDto.BillingAddress == "" || requestDto.BillingAddress == null)
                {
                    throw new ArgumentException("Please provide Billing Address");

                }
                else if (requestDto.MakerVahan == "" || requestDto.MakerVahan == null)
                {
                    throw new ArgumentException("Please provide Maker Vahan");

                }
                else if (requestDto.VehicleTypeVahan == "" || requestDto.VehicleTypeVahan == null)
                {
                    throw new ArgumentException("Please provide Vehicle Type");

                }
                else if (requestDto.FuelTypeVahan == "" || requestDto.FuelTypeVahan == null)
                {
                    throw new ArgumentException("Please provide Fuel Type");

                }
                else if (requestDto.VehicleCatVahan == "" || requestDto.VehicleCatVahan == null)
                {
                    throw new ArgumentException("Please provide Vehicle Category");

                }
                if (requestDto.MobileNo.Length == 10)
                {

                }
                else
                {
                    throw new ArgumentException("Please enter valid mobile no");


                }
                if (!string.IsNullOrEmpty(requestDto.BillingAddress))
                {
                    if (requestDto.BillingAddress.Contains("'"))
                    {
                        throw new ArgumentException("Apostrophe (') is not allowed in address");

                    }

                }
                nonHomo = await _fetchDataAndCache.GetStringFromCache("NonHomo");
                if (nonHomo == "Y")
                {
                    var vehicleCategory = requestDto.VehicleCatVahan;
                    _fetchDataAndCache.SetStringInCache("NonHomoVehicleType", vehicleCategory);
                }
                string IsVehicletypeEnable = "N";
                var oemId = await _fetchDataAndCache.GetStringFromCache("OemId");
                var parameters = new DynamicParameters();
                parameters.Add("@OemId", oemId);
                var taxInvoiceSummary = await _databaseHelper.QueryAsync<dynamic>(HsrpWithColorStickerQueries.GetTaxInvoiceSummaryReport, parameters);
                if (taxInvoiceSummary != null && taxInvoiceSummary.Any())
                {
                    foreach (var taxInvoice in taxInvoiceSummary)
                    {
                        var IsVehicleTypeEnable = taxInvoice.IsVehicleTypeEnable;
                        if (IsVehicleTypeEnable == "Y")
                        {
                            IsVehicletypeEnable = "Y";
                        }
                    }

                }
                if (IsVehicletypeEnable == "Y")
                {
                    if (requestDto.OemVehicleType == "" || requestDto.OemVehicleType == null)
                    {
                        throw new ArgumentException("Please Select Oem VehicleType Type");

                    }
                    if (requestDto.OemVehicleType == "0")
                    {
                        throw new ArgumentException("Please Select Oem VehicleType Type");

                    }


                }
                try
                {
                    string dateString = requestDto.RegistrationDate;
                    DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    string formattedDate ="13/06/2017";

                    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                    DateTime from_date = DateTime.ParseExact(formattedDate, "dd/MM/yyyy", theCultureInfo);
                    DateTime to = DateTime.ParseExact("25/11/2019", "dd/MM/yyyy", theCultureInfo);
                    string txt_total_days = ((from_date - to).TotalDays).ToString();
                    int diffResult = int.Parse(txt_total_days.ToString());
                    if (requestDto.StateId != "25")
                    {
                        if (diffResult >= 0)
                        {
                            throw new ArgumentException("Vehicle owner's with vehicles manufactured after 1st April 2019, should contact their respective Automobile Dealers for HSRP affixation.");

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Check Registration date Format DD/MM/YYYY");
                }
                var OrderType = _fetchDataAndCache.GetStringFromCache("OrderType");
                var OemId = _fetchDataAndCache.GetStringFromCache("OemId");
                var NonHomo = _fetchDataAndCache.GetStringFromCache("NonHomo");
                var billingaddress = requestDto.BillingAddress.Replace("'", "''");
                var parameter = new DynamicParameters();
                parameter.Add("@RegistrationNo", requestDto.RegistrationNo);
                parameter.Add("@ChassisNo", requestDto.ChassisNo);
                parameter.Add("@EngineNo", requestDto.EngineNo);
                var bookingHistoryId = await _databaseHelper.QueryAsync<dynamic>(HsrpWithColorStickerQueries.GetBookingHistoryId, parameter);
                if (bookingHistoryId != null && bookingHistoryId.Any())
                {
                    var parameter1 = new DynamicParameters();
                    parameter1.Add("@BharatStage", requestDto.BharatStage);
                    parameter1.Add("@RegistrationDate", requestDto.RegistrationDate);
                    parameter1.Add("@RegistrationNo", requestDto.RegistrationNo);
                    parameter1.Add("@ChassisNo", requestDto.ChassisNo);
                    parameter1.Add("@EngineNo", requestDto.EngineNo);
                    parameter1.Add("@OwnerName", requestDto.OwnerName);
                    parameter1.Add("@EmailId", requestDto.EmailId);
                    parameter1.Add("@MakerName", requestDto.MakerVahan);
                    parameter1.Add("@MobileNo", requestDto.MobileNo);
                    parameter1.Add("@StateName", requestDto.StateName);
                    parameter1.Add("@OrderType", OrderType);
                    parameter1.Add("@VehicleType", requestDto.VehicleTypeVahan);
                    parameter1.Add("@StateId", requestDto.StateId);
                    parameter1.Add("@BillingAddress", billingaddress);
                    parameter1.Add("@VehicleCategory", requestDto.VehicleCatVahan);
                    parameter1.Add("@OemId", OemId);
                    parameter1.Add("@NonHomo", NonHomo);
                    var InsertVahanLogQuery = await _databaseHelper.QueryAsync<dynamic>(HsrpWithColorStickerQueries.InsertVahanLogQuery, parameter1);
                    if (requestDto.RegistrationNo.Trim().ToUpper() == "DL10CG7191")
                    {

                    }
                    else
                    {
                        throw new ArgumentException("Order for this registration number already exists. For any query kindly mail to online@bookmyhsrp.com");
                    }


                }
                else
                {
                    var parameter1 = new DynamicParameters();
                    parameter1.Add("@BharatStage", requestDto.BharatStage);
                    parameter1.Add("@RegistrationDate", requestDto.RegistrationDate);
                    parameter1.Add("@RegistrationNo", requestDto.RegistrationNo);
                    parameter1.Add("@ChassisNo", requestDto.ChassisNo);
                    parameter1.Add("@EngineNo", requestDto.EngineNo);
                    parameter1.Add("@OwnerName", requestDto.OwnerName);
                    parameter1.Add("@EmailId", requestDto.EmailId);
                    parameter1.Add("@MakerName", requestDto.MakerVahan);
                    parameter1.Add("@MobileNo", requestDto.MobileNo);
                    parameter1.Add("@StateName", requestDto.StateName);
                    parameter1.Add("@OrderType", OrderType.Result);
                    parameter1.Add("@VehicleType", requestDto.VehicleTypeVahan);
                    parameter1.Add("@StateId", requestDto.StateId);
                    parameter1.Add("@BillingAddress", requestDto.BillingAddress);
                    parameter1.Add("@VehicleCategory", requestDto.VehicleCatVahan);
                    parameter1.Add("@OemId", OemId.Result);
                    parameter1.Add("@NonHomo", NonHomo.Result);
                    var InsertVahanLogQuery = await _databaseHelper.QueryAsync<dynamic>(HsrpWithColorStickerQueries.InsertVahanLogQuery, parameter1);

                }
                
                var oemid = _oemId;
                string[] oemarray;
                int flag = 0;
                string nonhomo = (NonHomo).ToString();
                if (nonhomo == "Y")
                {
                    if (requestDto.RcFileName == "" || requestDto.RcFileName == null)
                    {
                        flag = 0;
                        throw new ArgumentException("Please Upload Rc File");

                    }
                    else
                    {
                        flag = 1;

                    }
                }
                try
                {
                    if (requestDto.VehicleCatVahan.Trim().ToString().ToUpper() == "3WT" && requestDto.FuelTypeVahan)
                    {
                       await _fetchDataAndCache.SetStringInCache("VehicleType", "E-RICKSHAW");
                        await _fetchDataAndCache.SetStringInCache("VehicleClass_imgPath", "www");
                        await _fetchDataAndCache.SetStringInCache("VehicleCat", "3W");
                        await _fetchDataAndCache.SetStringInCache("VehicleTypeId", "2");
                        await _fetchDataAndCache.SetStringInCache("Vehiclecategoryid", "3");

                    }
                    else
                    {
                        var vehicleType = "";
                        var vehiclecategory = "";
                        int vehicletypeid;
                        var vehicletypeidIntoString = "";
                        var parameters2 = new DynamicParameters();
                        parameters2.Add("@VehicleCatType", requestDto.VehicleCatVahan);
                        var vehicleSesion = await _databaseHelper.QueryAsync<dynamic>(HsrpWithColorStickerQueries.VehicleSession, parameters2);
                        if (vehicleSesion != null && vehicleSesion.Any())
                        {
                            string HSRPHRVehicleType = "";
                            foreach (var vehicle in vehicleSesion)
                            {
                                HSRPHRVehicleType = vehicle.HSRPHRVehicleType;
                                vehicletypeid = vehicle.VehicleTypeid;
                                vehicletypeidIntoString = vehicletypeid.ToString();
                                vehiclecategory = vehicle.VehicleCategory;
                            }
                            if (IsVehicletypeEnable == "N")
                            {
                                var OEMID =Convert.ToInt32(await _fetchDataAndCache.GetStringFromCache("OemId"));
                                var parameter6 = new DynamicParameters();
                                parameter6.Add("@VahanVehicleType", HSRPHRVehicleType);
                                parameter6.Add("@OrderType", "OB");
                                parameter6.Add("@vehicleclass", requestDto.VehicleTypeVahan);
                                parameter6.Add("@oemid", OEMID);
                                parameter6.Add("@FuelType", requestDto.FuelTypeVahan);
                                var getOemVehicleType = await _databaseHelperPrimary.QueryAsync<dynamic>(HsrpWithColorStickerQueries.GetOemVehicleType, parameter6);
                                if (getOemVehicleType != null && getOemVehicleType.Any())
                                {

                                    foreach (var vehicle in getOemVehicleType)
                                    {
                                        vehicleType = vehicle.vehicleType;
                                       
                                    }
                                   await _fetchDataAndCache.SetStringInCache("VehicleType", vehicleType);
                                }
                                else
                                {
                                    var parameter4 = new DynamicParameters();
                                    parameter4.Add("@Maker", requestDto.Maker);
                                    parameter4.Add("@RegistrationNo", requestDto.RegistrationNo);
                                    parameter4.Add("@ChassisNo", requestDto.ChassisNo);
                                    parameter4.Add("@EngineNo", requestDto.EngineNo);
                                    parameter4.Add("@MobileNo", requestDto.MobileNo);
                                    parameter4.Add("@EmailId", requestDto.EmailId);
                                    parameter4.Add("@VehicleCatVahan", requestDto.VehicleCatVahan);
                                    parameter4.Add("@OrderType", "OB");
                                    parameter4.Add("@VehicleType", requestDto.VehicleTypeVahan);
                                    parameter4.Add("@OemId", OEMID);
                                    var insertMissMatchDataLog = await _databaseHelper.QueryAsync<dynamic>(HsrpWithColorStickerQueries.InsertMissMatchDataLog, parameter4);
                                    throw new ArgumentException("Vehicle Type Details didn't match");
                                }
                            }
                            else
                            {
                                _fetchDataAndCache.SetStringInCache("VehicleType", requestDto.VehicleTypeVahan);
                            }
                            
                           await _fetchDataAndCache.SetStringInCache("VehicleClass_imgPath", "www");
                           await _fetchDataAndCache.SetStringInCache("VehicleTypeid", vehicletypeidIntoString);
                            await _fetchDataAndCache.SetStringInCache("Vehiclecategoryid", "3");
                           await _fetchDataAndCache.SetStringInCache("VehicleCategory", vehiclecategory);

                        }
                        else
                        {
                            throw new ArgumentException("Vehicle Details didn't match");

                        }


                    }
                    await _fetchDataAndCache.SetStringInCache("VehicleFuelType", requestDto.FuelTypeVahan);
                    await _fetchDataAndCache.SetStringInCache("VehicleClass", requestDto.VehicleTypeVahan);
                    await _fetchDataAndCache.SetStringInCache("SessionBharatStage", requestDto.BharatStage);
                    await _fetchDataAndCache.SetStringInCache("RegDate", requestDto.RegistrationDate);
                    await _fetchDataAndCache.SetStringInCache("OwnerName", requestDto.OwnerName);
                    await _fetchDataAndCache.SetStringInCache("EmailID", requestDto.EmailId);
                    await _fetchDataAndCache.SetStringInCache("MobileNo", requestDto.MobileNo);
                    await _fetchDataAndCache.SetStringInCache("FilePath", requestDto.FilePath);
                    await _fetchDataAndCache.SetStringInCache("BillingAddress", requestDto.BillingAddress);
                    await _fetchDataAndCache.SetStringInCache("SessionState", requestDto.StateName);
                    await _fetchDataAndCache.SetStringInCache("SessionCity", "");
                    await _fetchDataAndCache.SetStringInCache("SessionGST", "");

                    response.Message = "Success";
                }
                catch (Exception ex)
                {

                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
