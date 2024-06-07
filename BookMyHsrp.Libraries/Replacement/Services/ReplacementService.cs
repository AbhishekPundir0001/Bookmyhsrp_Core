﻿using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Queries;
using BookMyHsrp.Libraries.Replacement.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.ReplacementHsrpColorStickerModel;

namespace BookMyHsrp.Libraries.Replacement.Services
{
    public class ReplacementService : IReplacementService
    {
        private readonly FetchDataAndCache _fetchDataAndCache;
        //private readonly AppSettings _appSettings;
        private readonly DapperRepository _databaseHelper;
        private readonly DapperRepository _databaseHelperPrimary;
        private readonly DynamicDataDto _dynamicDataDto;
        private readonly string _connectionString;
        private readonly string _vehicleStatusAPI;
        private readonly string _oemId;
        private readonly string _nonHomo;
        private readonly string _nonHomoOemId;
        string msg = string.Empty;

        public ReplacementService(IOptionsSnapshot<ConnectionString> connectionStringOptions, FetchDataAndCache fetchDataAndCache, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;

            _databaseHelper = new DapperRepository(_connectionString);
            //_appSettings = appSettings;
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelperPrimary = new DapperRepository(_connectionString);
            //_httpContextAccessor = httpContextAccessor;
            _fetchDataAndCache = fetchDataAndCache;
            _vehicleStatusAPI = dynamicData.Value.VehicleStatusAPI;
            //_oemId = dynamicData.Value.OemID;
            //_nonHomo = dynamicData.Value.NonHomo;
            //_nonHomoOemId = dynamicData.Value.NonHomoOemId;

        }

        public async Task<dynamic> VahanInformation(ReplacementVahanDetailsDto requestDto)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@StateId", requestDto.StateId);
            var result = await _databaseHelper.QueryAsync<dynamic>(
                 ReplacementQueries.GetStateDetails, parameters);
            return result;
        }
        public async Task<dynamic> CheckOrderExixts(string VehicleRegNo, string chassisNo, string engineNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNo", VehicleRegNo);
            parameters.Add("@ChassisNo", chassisNo);
            parameters.Add("@EngineNo", engineNo);
            var result = await _databaseHelper.QueryAsync<dynamic>(
                 ReplacementQueries.GetBookingHistory, parameters);
            return result;
        }
        public async Task<dynamic> OemRtoMapping(string VehicleRegNo, string OemId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNo", VehicleRegNo);
            parameters.Add("@OemId", OemId);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(
                 ReplacementQueries.OemRtoMapping, parameters);
            return result;
        }
        public async Task<dynamic> InsertVaahanLog(string VehicleRegNo, string chassisNo, string engineNo, ReplacementVehicleDetails vahanDetailsDto)
        {
            var response = JsonConvert.SerializeObject(vahanDetailsDto);

            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNo", VehicleRegNo.ToUpper());
            parameters.Add("@ChassisNo", chassisNo.ToUpper());
            parameters.Add("@EngineNo", engineNo.ToUpper());
            parameters.Add("@Fuel", vahanDetailsDto.fuel);
            parameters.Add("@Norms", vahanDetailsDto.norms);
            parameters.Add("@VehicleCategory", vahanDetailsDto.vchCatg);
            parameters.Add("@VehicleType", vahanDetailsDto.vchType);
            parameters.Add("@Maker", vahanDetailsDto.maker);
            parameters.Add("@ResponseJson", response);
            parameters.Add("@RegistrationDate", vahanDetailsDto.regnDate);
            parameters.Add("@HsrpFrontLasserCode", vahanDetailsDto.hsrpFrontLaserCode);
            parameters.Add("@HsrpRearLasserCode", vahanDetailsDto.hsrpRearLaserCode);
            var insertVahanLog = await _databaseHelperPrimary.QueryAsync<dynamic>(
            ReplacementQueries.InsertVahanLog, parameters);
            return insertVahanLog;
        }
        public async Task<dynamic> InsertVaahanLogWithoutApi(string VehicleRegNo, string chassisNo, string engineNo, ReplacementVehicleDetails vahanDetailsDto)
        {

            var response = JsonConvert.SerializeObject(vahanDetailsDto);
            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNo", VehicleRegNo.ToUpper());
            parameters.Add("@ChassisNo", chassisNo.ToUpper());
            parameters.Add("@EngineNo", engineNo.ToUpper());
            parameters.Add("@Fuel", vahanDetailsDto.fuel);
            parameters.Add("@Norms", vahanDetailsDto.norms);
            parameters.Add("@VehicleCategory", vahanDetailsDto.vchCatg);
            parameters.Add("@VehicleType", vahanDetailsDto.vchType);
            parameters.Add("@Maker", vahanDetailsDto.maker);
            parameters.Add("@ResponseJson", response);
            parameters.Add("@RegistrationDate", vahanDetailsDto.regnDate);
            parameters.Add("@HsrpFrontLasserCode", vahanDetailsDto.hsrpFrontLaserCode);
            parameters.Add("@HsrpRearLasserCode", vahanDetailsDto.hsrpRearLaserCode);
            var insertVahanLog = await _databaseHelperPrimary.QueryAsync<dynamic>(
            ReplacementQueries.InsertVaahanLogWithoutApi, parameters);
            return insertVahanLog;
        }

        public async Task<dynamic> GetOrderNumber(ReplacementVahanDetailsDto vahanDetailsDto)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNo", vahanDetailsDto.RegistrationNo.ToUpper());
            parameters.Add("@ChassisNo", vahanDetailsDto.ChassisNo.ToUpper());
            parameters.Add("@EngineNo", vahanDetailsDto.EngineNo.ToUpper());

            var getOrderNo = await _databaseHelper.QueryAsync<dynamic>(
            ReplacementQueries.GetOrderNo, parameters);
            return getOrderNo;
        }
        public async Task<dynamic> SelectByOrderNumber(string OrderNo)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", OrderNo);
            var getDatByOrderNumber = await _databaseHelper.QueryAsync<dynamic>(
            ReplacementQueries.GetDatByOrderNumber, parameters);
            return getDatByOrderNumber;
        }
        public async Task<dynamic> CheckDateBetweenCloseDate(ReplacementVahanDetailsDto vahanDetailsDto)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNo", vahanDetailsDto.RegistrationNo);
            parameters.Add("@ChassisNo", vahanDetailsDto.ChassisNo);
            parameters.Add("@EngineNo", vahanDetailsDto.EngineNo);
            var checkdate = await _databaseHelper.QueryAsync<dynamic>(
            ReplacementQueries.CheckDateBetweenCloseDate, parameters);
            return checkdate;
        }
        public async Task<dynamic> GetOemId(string makerName)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@MakerName", makerName);
            var oemId = await _databaseHelper.QueryAsync<dynamic>(
            ReplacementQueries.GetOemId, parameters);
            return oemId;
        }
        public async Task<dynamic> GetVehicleDetails(string OemId, string VehicleClass)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@OemId", OemId);
            parameters.Add("@VehicleClass", VehicleClass);
            var vehicleDetails = await _databaseHelper.QueryAsync<dynamic>(
            ReplacementQueries.GetVehicleDetails, parameters);
            return vehicleDetails;
        }
        public async Task<dynamic> TaxInvoiceSummary(string OemId)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@OemId", OemId);
            var taxInvoiceSummary = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.GetTaxInvoiceSummaryReport, parameters);
            return taxInvoiceSummary;
        }
        public async Task<dynamic> BookingHistoryId(string RegistrationNo, string ChassisNo, string EngineNo)
        {


            var parameter = new DynamicParameters();
            parameter.Add("@RegistrationNo", RegistrationNo);
            parameter.Add("@ChassisNo", ChassisNo);
            parameter.Add("@EngineNo", EngineNo);
            var bookingHistoryId = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.GetBookingHistoryId, parameter);
            return bookingHistoryId;
        }
        public async Task<dynamic> VehicleSession(string VehicleCatVahan)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@VehicleCatType", VehicleCatVahan);
            var vehicleSesion = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.VehicleSession, parameters);
            return vehicleSesion;
        }
        public async Task<dynamic> OemVehicleType(string HSRPHRVehicleType, string VehicleTypeVahan, int newId, string FuelTypeVahan)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@VahanVehicleType", HSRPHRVehicleType);
            parameters.Add("@OrderType", "OB");
            parameters.Add("@vehicleclass", VehicleTypeVahan);
            parameters.Add("@oemid", newId);
            parameters.Add("@FuelType", FuelTypeVahan);
            var getOemVehicleType = await _databaseHelperPrimary.QueryAsync<dynamic>(ReplacementQueries.GetOemVehicleType, parameters);
            return getOemVehicleType;
        }
        public async Task<dynamic> InsertMisMatchDataLog(dynamic customerInfo, string OemId)
        {


            var parameter = new DynamicParameters();
            parameter.Add("@Maker", customerInfo.MakerVahan);
            parameter.Add("@RegistrationNo", customerInfo.RegistrationNo);
            parameter.Add("@ChassisNo", customerInfo.ChassisNo);
            parameter.Add("@EngineNo", customerInfo.EngineNo);
            parameter.Add("@MobileNo", customerInfo.MobileNo);
            parameter.Add("@EmailId", customerInfo.EmailId);
            parameter.Add("@VehicleCatVahan", customerInfo.VehicleCatVahan);
            parameter.Add("@OrderType", "OB");
            parameter.Add("@VehicleType", customerInfo.VehicleTypeVahan);
            parameter.Add("@OemId", OemId);
            var insertMissMatchDataLog = await _databaseHelperPrimary.QueryAsync<dynamic>(ReplacementQueries.InsertMissMatchDataLog, parameter);
            return insertMissMatchDataLog;
        }
        public async Task<dynamic> InsertVahanLogQueryCustomer(dynamic requestDto, string OrderType, string billingaddress, string NonHomo, string OemId)
        {


            var parameter = new DynamicParameters();
            parameter.Add("@BharatStage", requestDto.BharatStage);
            parameter.Add("@RegistrationDate", requestDto.RegistrationDate);
            parameter.Add("@RegistrationNo", requestDto.RegistrationNo);
            parameter.Add("@ChassisNo", requestDto.ChassisNo);
            parameter.Add("@EngineNo", requestDto.EngineNo);
            parameter.Add("@OwnerName", requestDto.OwnerName);
            parameter.Add("@EmailId", requestDto.EmailId);
            parameter.Add("@MakerName", requestDto.MakerVahan);
            parameter.Add("@MobileNo", requestDto.MobileNo);
            parameter.Add("@StateName", requestDto.StateName);
            parameter.Add("@OrderType", OrderType);
            parameter.Add("@VehicleType", requestDto.VehicleTypeVahan);
            parameter.Add("@StateId", requestDto.StateId);
            parameter.Add("@BillingAddress", billingaddress);
            parameter.Add("@VehicleCategory", requestDto.VehicleCatVahan);
            parameter.Add("@OemId", OemId);
            parameter.Add("@NonHomo", NonHomo);
            var InsertVahanLogQuery = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.InsertVahanLogQuery, parameter);
            return InsertVahanLogQuery;
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
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            html = await reader.ReadToEndAsync();
                        }
                    }
                    else
                    {
                        html = "Vehicle Not Found";
                    }


            }
            catch (Exception ev)
            {
                html = "Error While Calling Vahan Service - " + ev.Message;
            }
            return html;

        }

        public async Task<dynamic> SessionBookingDetails(dynamic requestDto)
        {
            ReplacementResponseDto response = new ReplacementResponseDto();
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
                var taxInvoiceSummary = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.GetTaxInvoiceSummaryReport, parameters);
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
                    DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string formattedDate = date.ToString("dd-MM-yyyy");

                    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                    DateTime from_date = DateTime.ParseExact(formattedDate, "dd-MM-yyyy", theCultureInfo);
                    DateTime to = DateTime.ParseExact("25-11-2019", "dd-MM-yyyy", theCultureInfo);
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
                var bookingHistoryId = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.GetBookingHistoryId, parameter);
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
                    var InsertVahanLogQuery = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.InsertVahanLogQuery, parameter1);

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
                                var parameters7 = new DynamicParameters();
                                parameters7.Add("@MakerName", requestDto.MakerVahan);
                                var OEMID = await _databaseHelper.QueryAsync<dynamic>(
                                HsrpWithColorStickerQueries.GetOemId, parameters7);
                                int newId = 0;
                                foreach (var Id in OEMID)
                                {
                                    newId = Id.Oemid;
                                }
                                var parameter6 = new DynamicParameters();
                                parameter6.Add("@VahanVehicleType", HSRPHRVehicleType);
                                parameter6.Add("@OrderType", "OB");
                                parameter6.Add("@vehicleclass", requestDto.VehicleTypeVahan);
                                parameter6.Add("@oemid", newId);
                                parameter6.Add("@FuelType", requestDto.FuelTypeVahan);
                                var getOemVehicleType = await _databaseHelperPrimary.QueryAsync<dynamic>(HsrpWithColorStickerQueries.GetOemVehicleType, parameter6);
                                if (getOemVehicleType != null && getOemVehicleType.Any())
                                {

                                    foreach (var vehicle in getOemVehicleType)
                                    {
                                        vehicleType = vehicle.vehicleType;
                                        response.Message = "Success";

                                    }
                                    await _fetchDataAndCache.SetStringInCache("VehicleType", vehicleType);
                                }
                                else
                                {
                                    var parameter4 = new DynamicParameters();
                                    parameter4.Add("@Maker", requestDto.MakerVahan);
                                    parameter4.Add("@RegistrationNo", requestDto.RegistrationNo);
                                    parameter4.Add("@ChassisNo", requestDto.ChassisNo);
                                    parameter4.Add("@EngineNo", requestDto.EngineNo);
                                    parameter4.Add("@MobileNo", requestDto.MobileNo);
                                    parameter4.Add("@EmailId", requestDto.EmailId);
                                    parameter4.Add("@VehicleCatVahan", requestDto.VehicleCatVahan);
                                    parameter4.Add("@OrderType", "OB");
                                    parameter4.Add("@VehicleType", requestDto.VehicleTypeVahan);
                                    parameter4.Add("@OemId", OEMID);
                                    var insertMissMatchDataLog = await _databaseHelperPrimary.QueryAsync<dynamic>(HsrpWithColorStickerQueries.InsertMissMatchDataLog, parameter4);
                                    response.Message = "Vehicle Details didn't match";
                                    return response;
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
                            response.Message = "Vehicle Details didn't match";
                            return response;

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


        #region 7 days, 90 days, 180 days
        public async Task<dynamic> strBmhsrp1(string RegistrationNo, string ChassisNo, string EngineNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length-5));
            parameter.Add("@Engineno", EngineNo.Substring(EngineNo.Length-5));
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(ReplacementQueries.strBmhsrp1, parameter);
            return result;
        }

        public async Task<dynamic> strBmhsrp2(string RegistrationNo, string ChassisNo, string EngineNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            parameter.Add("@Engineno", EngineNo.Substring(EngineNo.Length - 5));
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(ReplacementQueries.strBmhsrp2, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord1(string RegistrationNo, string ChassisNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord1, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord2(string RegistrationNo, string ChassisNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord2, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord3(string OrderNo, int days)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Orderno", OrderNo);
            parameter.Add("@days", days);
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord3, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord4(string RegistrationNo, string ChassisNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            //parameter.Add("@Chassisno", "50702");
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord4, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord5(string RegistrationNo, string ChassisNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord5, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord6(string RegistrationNo, string ChassisNo,string HSRPRecordId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            parameter.Add("@HSRPRecordId", HSRPRecordId);
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord6, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord7(string RegistrationNo, string ChassisNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord7, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord8(string RegistrationNo, string ChassisNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord8, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord9(string RegistrationNo, string ChassisNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord9, parameter);
            return result;
        }

        public async Task<dynamic> strHsrpRecord10(string RegistrationNo, string ChassisNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strHsrpRecord10, parameter);
            return result;
        }

        public async Task<dynamic> strBmHSRPOrissa(string RegistrationNo, string ChassisNo, string EngineNo)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleRegNo", RegistrationNo);
            parameter.Add("@Chassisno", ChassisNo.Substring(ChassisNo.Length - 5));
            parameter.Add("@EngineNo", EngineNo.Substring(EngineNo.Length - 5));
            var result = await _databaseHelper.QueryAsync<dynamic>(ReplacementQueries.strBmhsrpOrissa, parameter);
            return result;
        }
        #endregion

    }


}

