using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Queries;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Services;
using BookMyHsrp.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Cryptography;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;
using static BookMyHsrp.Libraries.OemMaster.Models.OemMasterModel;

namespace BookMyHsrp.ReportsLogics.HsrpWithColorSticker
{
    public class HsrpWithColorStickerConnector
    {


        //private readonly FetchDataAndCache _fetchDataAndCache; // instance of ReportHelper
        // private readonly IHsrpWithColorStickerService _hsrpWithColorStickerService;
        //private readonly HttpContext _httpContext;
        private readonly HsrpWithColorStickerService _hsrpColorStickerService;
        private readonly string nonHomo;
        private readonly string _nonHomoOemId;
        private readonly string OemId;
        public HsrpWithColorStickerConnector(HsrpWithColorStickerService hsrpColorStickerService, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            // _fetchDataAndCache = fetchDataAndCache; // Dependency injection
            // _hsrpWithColorStickerService = hsrpWithColorStickerService ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerService));
            _hsrpColorStickerService = hsrpColorStickerService ?? throw new ArgumentNullException(nameof(hsrpColorStickerService));
            nonHomo = dynamicData.Value.NonHomo;
            _nonHomoOemId = dynamicData.Value.NonHomoOemId;
            OemId = dynamicData.Value.OemID;

        }
        public async Task<dynamic> VahanInformation(VahanDetailsDto requestDto)
        {

            ICollection<ValidationResult> results = null;
            var vehicleValidationResponse = new ResponseDto();
            vehicleValidationResponse.data = new VehicleValidation();
            if (!Validate(requestDto, out results))
            {
                vehicleValidationResponse.status = "false";
                vehicleValidationResponse.message = results.Select(x => x.ErrorMessage).FirstOrDefault();
                return vehicleValidationResponse;
            }
            var resultGot = await _hsrpColorStickerService.VahanInformation(requestDto);

            VehicleValidation vehicleValidationData = new VehicleValidation();
            var getStateId = Convert.ToInt32(requestDto.StateId);
            var getVehicleRegno = requestDto.RegistrationNo.Trim();
            var getChassisNo = requestDto.ChassisNo.Trim();
            var getEngineNo = requestDto.EngineNo.Trim();

            var statename = string.Empty;
            var stateshortname = string.Empty;
            var StateIdBackup = string.Empty;
            var oemImgPath = string.Empty;
            var oemid = string.Empty;
            int stateID = getStateId;
            foreach (var data in resultGot)
            {
                stateshortname = data.HSRPStateShortName;
                statename = data.HSRPStateName;
                StateIdBackup = requestDto.StateId;
            }
            var checkOrderExists = await _hsrpColorStickerService.CheckOrderExixts(getVehicleRegno, getChassisNo, getEngineNo);
            if (checkOrderExists.Count > 0 && requestDto.isReplacement == false)
            {
                vehicleValidationResponse.status = "false";
                vehicleValidationResponse.message =
                    "Order for this registration number already exists. For any query kindly mail to support@bookyourhsrp.com";
                return vehicleValidationResponse;
            }
            string responseJson = await _hsrpColorStickerService.RosmertaApi(getVehicleRegno, getChassisNo, getEngineNo, "5UwoklBqiW");
            if(responseJson== "Error While Calling Vahan Service - The remote server returned an error: (500) Internal Server Error.")
            {
                vehicleValidationResponse.message = "Error While Calling Vahan Service";
                return vehicleValidationResponse;
            }
            if(responseJson== "Vehicle Not Found")
            {
                vehicleValidationResponse.message = "Vehicle Not Found";
                return vehicleValidationResponse;

            }
            if (responseJson.Contains("The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters"))
            {
                vehicleValidationResponse.message = "Error While Calling Vahan Service";
                return vehicleValidationResponse;
            }

            VehicleDetails _vd = JsonConvert.DeserializeObject<VehicleDetails>(responseJson);
            if (_vd != null && _vd.stateCd != null && _vd.message != "Vehicle Not Found")
            {
                var hasError = false;
                if (_vd.stateCd != null)
                {


                    if (_vd.stateCd.ToLower().StartsWith(stateshortname.ToString().ToLower()) == false)
                    {
                        vehicleValidationResponse.status = "false";
                        vehicleValidationResponse.message =
                            "Please input Correct Registration Number of " + statename;
                        hasError = true;


                    }
                }
                //if (requestDto.isReplacement == false && !string.IsNullOrEmpty(_vd.hsrpRearLaserCode) &&
                //   !string.IsNullOrEmpty(_vd.hsrpFrontLaserCode))
                //{
                //    vehicleValidationResponse.status = "false";
                //    vehicleValidationResponse.message =
                //        "You have a valid HSRP laser code as per VAHAN data so the new' plate cannot be issued, however you can apply for a duplicate HSRP.";
                //    hasError = true;
                //}
                if (hasError)
                {
                    return vehicleValidationResponse;
                }
                if (_vd.message == "Vehicle details available in Vahan")
                {

                    var insertVahanLogQuery = _hsrpColorStickerService.InsertVaahanLog(getVehicleRegno, getChassisNo, getEngineNo, _vd);
                    var show_damage_both = true;
                    if (requestDto.isReplacement)
                    {
                        if (!string.IsNullOrEmpty(_vd.hsrpRearLaserCode) &&
                            !string.IsNullOrEmpty(_vd.hsrpFrontLaserCode))
                        {
                            if (!_vd.hsrpRearLaserCode.StartsWith("CC") && !_vd.hsrpFrontLaserCode.StartsWith("CC"))
                            {
                                show_damage_both = false;
                            }
                            var statusCheck = await CheckVehicleForDfdrdb(
                                new VahanDetailsDto()
                                {
                                    ChassisNo = getChassisNo.ToUpper(),
                                    EngineNo = getEngineNo.ToUpper(),
                                    RegistrationNo = getVehicleRegno.ToUpper(),
                                    StateId = getStateId.ToString()
                                });
                            if (statusCheck.status == "false")
                            {
                                vehicleValidationResponse.status = "false";
                                vehicleValidationResponse.message =
                                    "You are not authorized to book re-order. For any query kindly mail to support@bookyourhsrp.com";
                                vehicleValidationResponse.data = vehicleValidationData;
                                return vehicleValidationResponse;
                            }
                        }
                        else
                        {
                            vehicleValidationResponse.status = "false";
                            vehicleValidationResponse.message =
                                "You do not have a valid HSRP laser code as per VAHAN data so the duplicate plate cannot be issued, however you can apply for a new HSRP.";
                            vehicleValidationResponse.data = vehicleValidationData;
                            return vehicleValidationResponse;
                        }
                    }
                    var oemId = await _hsrpColorStickerService.GetOemId(_vd.maker);
                    if (oemId.Count > 0)
                    {
                        oemImgPath = oemId[0].oem_logo.ToString();
                        oemid = oemId[0].Oemid.ToString();
                        if (oemid == "20")
                        {
                            oemid = "272";
                        }
                        vehicleValidationData = new VehicleValidation();
                        vehicleValidationData.non_homo = "N";
                        vehicleValidationData.stateid = getStateId.ToString();
                        vehicleValidationData.statename = statename;
                        vehicleValidationData.StateIdBackup = StateIdBackup;
                        vehicleValidationData.stateshortname = stateshortname;
                        vehicleValidationData.oem_img_path = oemImgPath;
                        vehicleValidationData.oemid = oemid;
                        vehicleValidationData.fuel = _vd.fuel;
                        vehicleValidationData.maker = _vd.maker;
                        vehicleValidationData.vehicle_class = _vd.vchType;
                        vehicleValidationData.norms = _vd.norms;
                        vehicleValidationData.vehicle_category = _vd.vchCatg;
                        vehicleValidationData.veh_reg_date = _vd.regnDate;
                        vehicleValidationData.message = _vd.message;
                        vehicleValidationResponse.data = vehicleValidationData;
                        vehicleValidationData.engineno = getEngineNo.ToUpper();
                        vehicleValidationData.chassisno = getChassisNo.ToUpper();
                        vehicleValidationData.vehicleregno = getVehicleRegno.ToUpper();
                        vehicleValidationData.show_damage_both = show_damage_both;
                        vehicleValidationData.oemvehicletypelist = await GetOemVehicleTypes(_vd.vchType, oemid);
                        vehicleValidationResponse.status = "true";
                        vehicleValidationResponse.message = "success";
                    }
                    else
                    {
                        vehicleValidationResponse.status = "false";
                        vehicleValidationResponse.message =
                            "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                        vehicleValidationResponse.data = vehicleValidationData;
                        return vehicleValidationResponse;
                    }
                }
                else if (_vd.message.Contains(
                            "Vehicle details available in Vahan but Maker of this vehicle Not Present in HOMOLOGATION") ||
                        _vd.message ==
                        "Vehicle details available in Vahan but OEM/Manufacturer (Homologation) of this vehicle have not authorized you for the State/RTO of this vehicle, please contact respective OEM.")
                {
                    var insertVahanLogQuery = await _hsrpColorStickerService.InsertVaahanLog(getVehicleRegno, getChassisNo, getEngineNo, _vd);
                    var oemDetail = await _hsrpColorStickerService.GetOemId(_vd.maker);
                    if (oemDetail.Count > 0)
                    {
                        oemDetail[0].oem_logo.ToString();
                        oemid = oemDetail[0].Oemid.ToString();
                        vehicleValidationResponse.data.non_homo = "Y";
                        vehicleValidationResponse.data.oem_img_path = oemDetail[0].oem_logo.ToString();
                        var checkOrderExistsCHECK = await _hsrpColorStickerService.OemRtoMapping(getVehicleRegno, oemid);
                        if (oemid == "20")
                        {
                            oemid = "272";
                        }
                        if (checkOrderExistsCHECK.Count > 0)
                        {
                            if (checkOrderExistsCHECK[0].IsOemRtoMapped.ToString() == "Y")

                                {

                                    int NonHomoCount = 0;
                                    string NonHomo = nonHomo;
                                    string[] sp_NonHomo = NonHomo.Split(',');
                                    for (int i = 0; i < sp_NonHomo.Length; i++)
                                    {
                                        if (sp_NonHomo[i].ToString() == requestDto.StateId)
                                        {
                                            NonHomoCount++;
                                        }
                                    }
                                    string NonHomoOemId = _nonHomoOemId;
                                    string[] sp_NonHomoOemId = _nonHomoOemId.Split(',');
                                    if ((NonHomoCount > 0) && (_vd.message.Contains("Vehicle details available in Vahan but Maker of this vehicle Not Present in HOMOLOGATION") || (_vd.message.Contains("Vehicle details available in Vahan but OEM/Manufacturer (Homologation) of this vehicle have not authorized you for the State/RTO of this vehicle") && _nonHomoOemId.Contains(oemid))))
                                    {
                                        VehicleDetails details = new VehicleDetails();
                                        vehicleValidationResponse.data.vehicleregno = requestDto.RegistrationNo.ToUpper().Trim();
                                        vehicleValidationResponse.data.chassisno = requestDto.ChassisNo.ToUpper().Trim();
                                        vehicleValidationResponse.data.engineno = requestDto.EngineNo.ToUpper().Trim();
                                        vehicleValidationResponse.data.non_homo = "Y";
                                        vehicleValidationResponse.status = "true";
                                        vehicleValidationResponse.message = _vd.message;
                                        details = _vd;
                                        vehicleValidationResponse.data.fuel = details.fuel;
                                        vehicleValidationResponse.data.message = details.message;
                                        vehicleValidationResponse.data.offCd = details.offCd;
                                        vehicleValidationResponse.data.maker = details.maker;
                                        vehicleValidationResponse.data.hsrpFrontLaserCode = details.hsrpFrontLaserCode;
                                        vehicleValidationResponse.data.vchType = details.vchType;
                                        vehicleValidationResponse.data.vchCatg = details.vchCatg;
                                        vehicleValidationResponse.data.stateCd = details.stateCd;
                                        vehicleValidationResponse.data.regnDate = details.regnDate;
                                        vehicleValidationResponse.data.norms = details.norms;
                                        vehicleValidationResponse.data.hsrpRearLaserCode = details.hsrpRearLaserCode;
                                        return vehicleValidationResponse;
                                    }

                                    else
                                    {
                                        vehicleValidationResponse.data.non_homo = "N";
                                        vehicleValidationResponse.status = "false";
                                        vehicleValidationResponse.message =
                                            "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                                        vehicleValidationResponse.data = vehicleValidationData;
                                        return vehicleValidationResponse;

                                    }
                                }
                                else
                                {
                                    vehicleValidationResponse.data.non_homo = "N";
                                    vehicleValidationResponse.status = "false";
                                    vehicleValidationResponse.message =
                                        "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                                    vehicleValidationResponse.data = vehicleValidationData;
                                    return vehicleValidationResponse;
                                }
                            }
                            else
                            {
                                vehicleValidationResponse.data.non_homo = "N";
                                vehicleValidationResponse.status = "false";
                                vehicleValidationResponse.message =
                                    "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                                vehicleValidationResponse.data = vehicleValidationData;
                                return vehicleValidationResponse;
                            }
                        }
                        else
                        {
                            vehicleValidationResponse.data.non_homo = "N";
                            vehicleValidationResponse.status = "false";
                            vehicleValidationResponse.message =
                                "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                            vehicleValidationResponse.data = vehicleValidationData;
                            return vehicleValidationResponse;
                        }
                    }
                    else if (_vd.message == "Vehicle Not Found")
                    {
                        vehicleValidationResponse.data.non_homo = "N";
                        var insertVahanLogQuery = await _hsrpColorStickerService.InsertVaahanLogWithoutApi(getVehicleRegno, getChassisNo, getEngineNo, _vd);
                        vehicleValidationResponse.status = "false";
                        vehicleValidationResponse.message = "Your vehicle detail didn't match with vahan service..";
                        vehicleValidationResponse.data = vehicleValidationData;
                        return vehicleValidationResponse;
                    }

                }
                else
                {
                    vehicleValidationResponse.status = "false";
                    vehicleValidationResponse.message =
                        "Your Vehicle Data Not Pulled From Vahan Please Try After Some Time.";
                    vehicleValidationResponse.data = vehicleValidationData;
                    return vehicleValidationResponse;
                }
                return vehicleValidationResponse;
            }
            else
            {
                return vehicleValidationResponse;
            }
        }

        public async Task<List<OemVehicleTypeList>> GetOemVehicleTypes(string VehicleClass, string OemId)
        {
            List<OemVehicleTypeList> lst = new List<OemVehicleTypeList>();

            var vehicleDetails = await _hsrpColorStickerService.GetVehicleDetails(OemId, VehicleClass);



            if (vehicleDetails.Count > 0)
            {
                for (int i = 0; i < vehicleDetails.Count; i++)
                {
                    var vehType = new OemVehicleTypeList();
                    vehType.VehicleType = vehicleDetails[0].VehicleType.ToString().Trim();
                    vehType.DisplayVehicleType = vehicleDetails[0].Vehicletypenew.ToString().Trim();
                    vehType.IsEnabled = vehicleDetails[0].IsVehicleTypeEnable.ToString().Trim().ToUpper();
                    lst.Add(vehType);
                }
            }

            return lst;
        }


        public async Task<TrackOrderResponse> CheckVehicleForDfdrdb(VahanDetailsDto info)
        {
            var trackOrderResponse = new TrackOrderResponse();

            ICollection<ValidationResult> results = null;
            if (!Validate(info, out results))
            {
                trackOrderResponse.status = "false";
                trackOrderResponse.message = results.Select(x => x.ErrorMessage).FirstOrDefault();
                return trackOrderResponse;
            }
            var getOrderNumber = await _hsrpColorStickerService.GetOrderNumber(info);
            if (getOrderNumber)
            {

                var data = await _hsrpColorStickerService.SelectByOrderNumber(getOrderNumber[0].Orderno);
                if (data.Count > 0)
                {
                    if (data[0].ReBookingAllow.ToString() == "Y")
                    {
                        trackOrderResponse.status = "true";
                        trackOrderResponse.message = "Order found";
                        return trackOrderResponse;
                    }
                }

                trackOrderResponse.status = "false";
                trackOrderResponse.message = "Vehicle not found.";
                return trackOrderResponse;
            }
            else
            {
                var result = await _hsrpColorStickerService.CheckDateBetweenCloseDate(info);

                if (result.Count > 0)
                {
                    if (result.ReBookingAllow.ToString() == "Y")
                    {
                        trackOrderResponse.status = "true";
                        trackOrderResponse.message = "Order found";
                        return trackOrderResponse;
                    }
                }
                else
                {
                    trackOrderResponse.status = "true";
                    trackOrderResponse.message = "New Order.";
                    return trackOrderResponse;
                }

                trackOrderResponse.status = "false";
                trackOrderResponse.message = "Vehicle not found.";
                return trackOrderResponse;
            }
        }
        public async Task<dynamic> SessionBookingDetails(dynamic requestDto)
        {
            var data = _hsrpColorStickerService.SessionBookingDetails(requestDto);
            return data;


        }
        public async Task<dynamic> CustomerInfo(CustomerInfoModel customerInfo, dynamic sessionDetails)
        {

            var setCusmoterData = new SetCustomerData();
            var customerInformationresponseData = new CustomerInformationResponse();
            customerInformationresponseData.data = new CustomerInformationData();
            ICollection<ValidationResult> results = null;
            if (!Validate(customerInfo, out results))
            {
                customerInformationresponseData.Status = "false";
                customerInformationresponseData.Message = results.Select(x => x.ErrorMessage).FirstOrDefault();
                return customerInformationresponseData;
            }
            string getStateId = customerInfo.StateId;
            string getOemId = customerInfo.OemId;
            string getNonHomo = customerInfo.NonHomo;
            string getOrderType = customerInfo.OrderType;
            string getVehicleRegno = customerInfo.RegistrationDate;
            string getChassisNo = customerInfo.ChassisNo;
            string getEngineNo = customerInfo.EngineNo;
            string getBhartStage = customerInfo.BharatStage;
            string getVehRegDate = customerInfo.RegistrationDate;
            string getCustomerName = customerInfo.OwnerName;
            string getCustomerEmail = customerInfo.EmailId;
            string getCustomerMobile = customerInfo.MobileNo;
            string getCustomerBillingAddress = customerInfo.BillingAddress;
            string getRCFile = customerInfo.RcFileName;
            string getVehMaker = customerInfo.MakerVahan;
            string getVehicleType = customerInfo.VehicleTypeVahan;
            string getFuelType = customerInfo.FuelTypeVahan;
            string getVehicleCategory = customerInfo.VehicleCatVahan;
            string getOrderPlateSticker = customerInfo.PlateSticker;
            string IsVehicletypeEnable = "N";
            string realOrdertype = "OB";
            string ReplacementType = customerInfo.ReplacementType;
            var qstr = "";
            string insertVahanLogQuery = "";
            var session = new Session();
            session.IsOTPVerify = "N";
            session.OTPno = null;
            session.VehicleType_imgPath = "www";
            session.OEMImgPath = "www";
            session.OrderType = "OB";
            var jsonDeSerializer = System.Text.Json.JsonSerializer.Deserialize<RootDto>(sessionDetails);
            try
            {
                var nonHomo = jsonDeSerializer.NonHomo;
                if (nonHomo == "Y")
                {
                    var vehicleCategory = customerInfo.VehicleCatVahan;
                    customerInformationresponseData.data.NonHomoVehicleType = vehicleCategory;
                }
                IsVehicletypeEnable = "N";
                var oemId = jsonDeSerializer.OemId;
                var taxInvoiceSummary = await _hsrpColorStickerService.TaxInvoiceSummary(oemId);
                if (taxInvoiceSummary.Count > 0)
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
                    //if (requestDto.OemVehicleType == "" || requestDto.OemVehicleType == null)
                    //{
                    //    throw new ArgumentException("Please Select Oem VehicleType Type");

                    //}
                    //if (requestDto.OemVehicleType == "0")
                    //{
                    //    throw new ArgumentException("Please Select Oem VehicleType Type");

                    //}


                }
                try
                {
                    string dateString = customerInfo.RegistrationDate;
                    IFormatProvider theCultureInfo = new CultureInfo("en-GB", true);
                    var resultDateTime = DateTime.TryParseExact(dateString, "dd/MM/yyyy", theCultureInfo, DateTimeStyles.None
                        , out var dt)
                        ? dt
                        : null as DateTime?;
                    DateTime to = DateTime.ParseExact("25/11/2019", "dd/MM/yyyy", theCultureInfo);
                    if (resultDateTime.HasValue)
                    {

                        //if State Orissa then check if order can be taken after 2019

                        if (RunCheckIfOrderCanBeTakenAfter2019(stateId: getStateId))
                        {
                            var txtTotalDays = ((resultDateTime.Value - to).TotalDays);
                            if (txtTotalDays > 0)
                            {
                                customerInformationresponseData.Status = "false";
                                customerInformationresponseData.Message = "Vehicle owner's with vehicles manufactured after 25th November 2019, should contact their respective Automobile Dealers for HSRP affixation.";
                                //   customerInformationresponseData.data = customerInformationData;
                                return customerInformationresponseData;
                            }

                        }

                    }
                   
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                var OrderType = jsonDeSerializer.OrderType;
                var OemId = jsonDeSerializer.OemId;
                var NonHomo = jsonDeSerializer.NonHomo;
                var billingaddress = "";
                if (jsonDeSerializer.CustomerBillingAddress != null)
                {
                    billingaddress = jsonDeSerializer.CustomerBillingAddress.Replace("'", "''");
                }
                var bookingHistoryId = await _hsrpColorStickerService.BookingHistoryId(customerInfo.RegistrationNo, customerInfo.ChassisNo, customerInfo.EngineNo);
                if (bookingHistoryId.Count > 0)
                {
                    var insertVahanLogQueryCustomer = await _hsrpColorStickerService.InsertVahanLogQueryCustomer(customerInfo, OrderType, billingaddress, NonHomo, OemId);
                    if (customerInfo.RegistrationNo.Trim().ToUpper() == "DL10CG7191")
                    {

                    }
                    else
                    {
                        customerInformationresponseData.Message = "Order for this registration number already exists. For any query kindly mail to online@bookmyhsrp.com";
                    }
                }
                else
                {
                    var insertVahanLogQueryCustomer = await _hsrpColorStickerService.InsertVahanLogQueryCustomer(customerInfo, OrderType, billingaddress, NonHomo, OemId);

                }
                var oemid = OemId;
                string[] oemarray;
                int flag = 0;
                string nonhomo = (NonHomo).ToString();
                if (nonhomo == "Y")
                {
                    if (customerInfo.RcFileName == "" || customerInfo.RcFileName == null)
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
                    if (customerInfo.VehicleCatVahan.Trim().ToString().ToUpper() == "3WT" && customerInfo.FuelTypeVahan != null && customerInfo.FuelTypeVahan != "")
                    {
                        customerInformationresponseData.data.VehicleType = "E-RICKSHAW";
                        customerInformationresponseData.data.Vehiclecategoryid = "3";
                        customerInformationresponseData.data.VehicleClass_imgPath = "www";
                        customerInformationresponseData.data.VehicleCat = "3w";
                        customerInformationresponseData.data.VehicleTypeId = "2";
                    }
                    else
                    {
                        var vehicleType = "";
                        var vehiclecategory = "";
                        int vehicletypeid;
                        var vehicletypeidIntoString = "";
                        var vehicleSession = await _hsrpColorStickerService.VehicleSession(customerInfo.VehicleCatVahan);
                        if (vehicleSession.Count > 0)
                        {
                            string HSRPHRVehicleType = "";
                            foreach (var vehicle in vehicleSession)
                            {
                                HSRPHRVehicleType = vehicle.HSRPHRVehicleType;
                                vehicletypeid = vehicle.VehicleTypeid;
                                vehicletypeidIntoString = vehicletypeid.ToString();
                                vehiclecategory = vehicle.VehicleCategory;
                            }
                            if (IsVehicletypeEnable == "N")
                            {
                                var oemIdNew = await _hsrpColorStickerService.GetOemId(customerInfo.MakerVahan);
                                int newId = 0;
                                foreach (var Id in oemIdNew)
                                {
                                    newId = Id.Oemid;
                                }
                                var getOemVehicleType = await _hsrpColorStickerService.OemVehicleType(HSRPHRVehicleType, customerInfo.VehicleTypeVahan, newId, customerInfo.FuelTypeVahan);
                                if (getOemVehicleType.Count > 0)
                                {

                                    foreach (var vehicle in getOemVehicleType)
                                    {
                                        vehicleType = vehicle.vehicleType;
                                        customerInformationresponseData.Message = "Success";

                                    }
                                    customerInformationresponseData.data.VehicleType = vehicleType;
                                }
                                else
                                {
                                    var insertMissmatchDataLog = await _hsrpColorStickerService.InsertMisMatchDataLog(customerInfo, oemIdNew);
                                    customerInformationresponseData.Message = "Vehicle Details didn't match";
                                    return customerInformationresponseData;
                                }
                            }
                            else
                            {
                                customerInformationresponseData.data.VehicleType = customerInfo.VehicleTypeVahan;
                            }
                            customerInformationresponseData.data.VehicleClass_imgPath = "www";
                            customerInformationresponseData.data.VehicleTypeId = vehicletypeidIntoString;
                            customerInformationresponseData.data.Vehiclecategoryid = "3";
                            customerInformationresponseData.data.Vehiclecategory = vehiclecategory;
                        }
                        else
                        {
                            customerInformationresponseData.Message = "Vehicle Details didn't match";
                            return customerInformationresponseData;

                        }
                    }
                    customerInformationresponseData.data.VehicleFuelType = customerInfo.FuelTypeVahan;
                    customerInformationresponseData.data.VehicleClass = customerInfo.VehicleTypeVahan;
                    customerInformationresponseData.data.SessionBharatStage = customerInfo.BharatStage;
                    customerInformationresponseData.data.RegDate = customerInfo.RegistrationDate;
                    customerInformationresponseData.data.OwnerName = customerInfo.OwnerName;
                    customerInformationresponseData.data.EmailID = customerInfo.EmailId;
                    customerInformationresponseData.data.MobileNo = customerInfo.MobileNo;
                    customerInformationresponseData.data.BillingAddress = customerInfo.BillingAddress;
                    customerInformationresponseData.data.SessionCity = "";
                    customerInformationresponseData.data.SessionGST = "";
                    customerInformationresponseData.Message = "Success";
                    customerInformationresponseData.data.IsOTPVerify = session.IsOTPVerify;
                    customerInformationresponseData.data.OTPno = session.OTPno;
                    customerInformationresponseData.data.OrderType = session.OrderType;
                    customerInformationresponseData.data.OEMImgPath = session.OEMImgPath;
                    customerInformationresponseData.data.VehicleType_imgPath = session.VehicleType_imgPath;

                }

                catch (Exception ex)
                {
                    throw ex;
                }

                return customerInformationresponseData;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        }
        public static bool RunCheckIfOrderCanBeTakenAfter2019(string stateId)
        {

            //Orissa

            var dateValidationCheckFor2019 = false;
            if (stateId == "25")
            {

                dateValidationCheckFor2019 = false;

            }
            else
            {

                dateValidationCheckFor2019 = true;
            }
            return dateValidationCheckFor2019;

        }

    }
}
