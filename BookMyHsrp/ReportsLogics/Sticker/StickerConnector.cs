using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.Libraries.Sticker.Models;
using BookMyHsrp.Libraries.Sticker.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;
using static BookMyHsrp.Libraries.Sticker.Models.StickerModel;

namespace BookMyHsrp.ReportsLogics.Sticker
{
    public class StickerConnector
    {
        private readonly IStickerService _StickerService;
        //private readonly string nonHomo;
        //private readonly string _nonHomoOemId;
        //private readonly string OemId;
        public StickerConnector(IStickerService StickerService)
        {
            _StickerService = StickerService ?? throw new ArgumentNullException(nameof(StickerService));
            //nonHomo = dynamicData.Value.NonHomo;
            //_nonHomoOemId = dynamicData.Value.NonHomoOemId;
            //OemId = dynamicData.Value.OemID;

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
            var resultGot = await _StickerService.GetStateDetails(requestDto);

            VehicleValidation vehicleValidationData = new VehicleValidation();
            var getStateId = Convert.ToInt32(requestDto.StateId);
            var getVehicleRegno = requestDto.RegistrationNo.Trim();
            var getChassisNo = requestDto.ChassisNo.Trim();
            var getEngineNo = requestDto.EngineNo.Trim();

            var statename = string.Empty;
            var stateshortname = string.Empty;
            var oemImgPath = string.Empty;
            var oemid = string.Empty;
            int stateID = getStateId;
            foreach (var data in resultGot)
            {
                stateshortname = data.HSRPStateShortName;
                statename = data.HSRPStateName;
            }
            var checkOrderExists = await _StickerService.isAbleToBook(getVehicleRegno, getChassisNo, getEngineNo);
            if (checkOrderExists.Count > 0 && requestDto.isReplacement == false)
            {
                if (checkOrderExists[0].ReBookingAllow != "Y" && getVehicleRegno != "DL10CG7191")
                {
                    vehicleValidationResponse.status = "false";
                    vehicleValidationResponse.message = "You are not authorized to book re-order. For any query kindly mail to online@bookmyhsrp.com";
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

            string responseJson = await _StickerService.RosmertaApi(getVehicleRegno, getChassisNo, getEngineNo, "5UwoklBqiW");
            if (responseJson.Contains("Vehicle details available in Vahan")) {
                VehicleDetails _vd = JsonConvert.DeserializeObject<VehicleDetails>(responseJson);
                if (_vd != null && _vd.stateCd != null && _vd.message != "Vehicle Not Found")
                {
                    var hasError = false;
                    if (_vd.stateCd != null)
                    {
                        if (_vd.stateCd.ToLower().StartsWith(stateshortname.ToString().ToLower()) == false)
                        {
                            vehicleValidationResponse.status = "false";
                            vehicleValidationResponse.message = "Please input Correct Registration Number of " + statename;
                            hasError = true;
                        }
                    }
                    if (hasError)
                    {
                        return vehicleValidationResponse;
                    }
                    if (_vd.message == "Vehicle details available in Vahan")
                    {
                        bool CheckedStatus = false;
                        var insertVahanLogQuery = await _StickerService.InsertVaahanLog(getVehicleRegno, getChassisNo, getEngineNo, _vd);
                        var res = await _StickerService.checkVehicleForStickerPr(requestDto.RegistrationNo, requestDto.HsrpFrontLaserCode, requestDto.HsrpRearLaserCode);
                        if (res.Count == 0)
                        {
                            res = await _StickerService.checkVehicleForStickerDL(requestDto.RegistrationNo, requestDto.HsrpFrontLaserCode, requestDto.HsrpRearLaserCode);
                            if (getVehicleRegno.StartsWith("hr"))
                            {
                                if (res.Count == 0)
                                {
                                    res = await _StickerService.checkVehicleForStickerHR(requestDto.RegistrationNo, requestDto.HsrpFrontLaserCode, requestDto.HsrpRearLaserCode);
                                }
                            }
                        }
                        if (res.Count > 0)
                        {
                            CheckedStatus = true;
                        }
                        if (_vd.vchCatg.ToUpper() == "2WN" || _vd.vchCatg.ToUpper() == "2WIC" || _vd.vchCatg.ToUpper() == "2WT")
                        {
                            vehicleValidationResponse.status = "false";
                            vehicleValidationResponse.message = "Your vehicle is not mapped for sticker";
                            vehicleValidationResponse.data = vehicleValidationData;
                            return vehicleValidationResponse;
                        }

                        var oemId = await _StickerService.GetOemId(_vd.maker);
                        if (oemId.Count > 0)
                        {
                            oemImgPath = oemId[0].oem_logo.ToString();
                            oemid = oemId[0].Oemid.ToString();
                        }
                        else
                        {
                            vehicleValidationResponse.status = "false";
                            vehicleValidationResponse.message = "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                            vehicleValidationResponse.data = vehicleValidationData;
                            return vehicleValidationResponse;
                        }

                        if (CheckedStatus == true)
                        {
                            VehicleDetails details = new VehicleDetails();
                            details = _vd;
                            vehicleValidationResponse.status = "true";
                            vehicleValidationResponse.message = _vd.message;
                            vehicleValidationResponse.UploadFlag = "N";
                            vehicleValidationResponse.data.upload_flag = "N";

                            vehicleValidationResponse.data.vehicleregno = requestDto.RegistrationNo.ToUpper().Trim();
                            vehicleValidationResponse.data.chassisno = requestDto.ChassisNo.ToUpper().Trim();
                            vehicleValidationResponse.data.engineno = requestDto.EngineNo.ToUpper().Trim();

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
                            vehicleValidationResponse.data.oemid = oemid;
                            vehicleValidationResponse.data.stateid = stateID.ToString();
                            vehicleValidationResponse.data.statename = statename;
                            return vehicleValidationResponse;

                        }
                        else
                        {

                            VehicleDetails details = new VehicleDetails();
                            details = _vd;
                            vehicleValidationResponse.status = "true";
                            vehicleValidationResponse.message = _vd.message;
                            vehicleValidationResponse.UploadFlag = "Y";
                            vehicleValidationResponse.data.upload_flag = "Y";

                            vehicleValidationResponse.data.vehicleregno = requestDto.RegistrationNo.ToUpper().Trim();
                            vehicleValidationResponse.data.chassisno = requestDto.ChassisNo.ToUpper().Trim();
                            vehicleValidationResponse.data.engineno = requestDto.EngineNo.ToUpper().Trim();

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
                            vehicleValidationResponse.data.oemid = oemid;
                            vehicleValidationResponse.data.stateid = stateID.ToString();
                            vehicleValidationResponse.data.statename = statename;
                            return vehicleValidationResponse;
                        }


                    }
                    else if (_vd.message.Contains("Vehicle details available in Vahan but"))
                    {
                        var insertVahanLogQuery = await _StickerService.InsertVaahanLog(getVehicleRegno, getChassisNo, getEngineNo, _vd);
                        bool CheckedStatus = false;
                        var res = await _StickerService.checkVehicleForStickerPr(requestDto.RegistrationNo, requestDto.HsrpFrontLaserCode, requestDto.HsrpRearLaserCode);
                        if (res.Count == 0)
                        {
                            res = await _StickerService.checkVehicleForStickerDL(requestDto.RegistrationNo, requestDto.HsrpFrontLaserCode, requestDto.HsrpRearLaserCode);
                            if (getVehicleRegno.StartsWith("hr"))
                            {
                                if (res.Count == 0)
                                {
                                    res = await _StickerService.checkVehicleForStickerHR(requestDto.RegistrationNo, requestDto.HsrpFrontLaserCode, requestDto.HsrpRearLaserCode);
                                }
                            }
                        }
                        vehicleValidationResponse.status = "false";
                        vehicleValidationResponse.message = "As a Vendor we are not authorised for this vehicle Please visit www.siam.in for respective HSRP Maker.";
                        vehicleValidationResponse.data = vehicleValidationData;
                        return vehicleValidationResponse;
                    }
                    else
                    {
                        vehicleValidationResponse.status = "false";
                        vehicleValidationResponse.message = "Your vehicle detail didn't match with vahan service";
                        vehicleValidationResponse.UploadFlag = "N";
                        return vehicleValidationResponse;
                    }
                }
            }
            else
            {
                vehicleValidationResponse.status = "false";
                vehicleValidationResponse.message = "Error While Calling Your Vehicle Details From Vahan Please Try After Some Time";
            }
               return vehicleValidationResponse;
        }

        public async Task<dynamic> CustomerInfo(CustomerInfoModelSticker customerInfo, dynamic sessionDetails)
        {
            var setCusmoterData = new SetCustomerData();
            var customerInformationresponseData = new CustomerInformationResponseSticker();
            customerInformationresponseData.data = new CustomerInformationData();
            //ICollection<ValidationResult> results = null;
            //if (!Validate(customerInfo, out results))
            //{
            //    customerInformationresponseData.Status = "false";
            //    customerInformationresponseData.Message = results.Select(x => x.ErrorMessage).FirstOrDefault();
            //    return customerInformationresponseData;
            //}
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
            var jsonDeSerializer = System.Text.Json.JsonSerializer.Deserialize<RootDtoSticker>(sessionDetails);

            try
            {

                string dateString = customerInfo.RegistrationDate;
                IFormatProvider theCultureInfo = new CultureInfo("en-GB", true);
                var resultDateTime = DateTime.TryParseExact(dateString, "dd/MM/yyyy", theCultureInfo, DateTimeStyles.None
                    , out var dt)
                    ? dt
                    : null as DateTime?;
                DateTime to = DateTime.ParseExact("01/04/2020", "dd/MM/yyyy", theCultureInfo);
                if (resultDateTime.HasValue)
                {
                    if (RunCheckIfOrderCanBeTakenAfter2019(stateId: getStateId))
                    {
                        var txtTotalDays = ((resultDateTime.Value - to).TotalDays);
                        if (txtTotalDays > 0)
                        {
                            customerInformationresponseData.Status = "false";
                            customerInformationresponseData.Message = "Check Registration date Format DD/MM/YYYY";
                            //   customerInformationresponseData.data = customerInformationData;
                            return customerInformationresponseData;
                        }

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            try
            {
                if (customerInfo.VehicleCatVahan.Trim().ToString().ToUpper() == "3WT" || customerInfo.VehicleCatVahan.Trim().ToString().ToUpper() == "3WN")
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
                    var vehicleSession = await _StickerService.VehicleSession(customerInfo.VehicleCatVahan);
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

                    var VehPlateEntryLog = await _StickerService.VehiclePlateEntryLog(customerInfo.BharatStage, customerInfo.RegistrationNo, customerInfo.RegistrationDate, customerInfo.ChassisNo, customerInfo.EngineNo, customerInfo.OwnerName, customerInfo.EmailId, customerInfo.MobileNo, customerInfo.BillingAddress, customerInfo.StateId, customerInfo.OrderType, customerInfo.VehicleCatVahan, customerInfo.VehicleTypeVahan, customerInfo.StateId, customerInfo.OemId, customerInfo.FrontLaserCode, customerInfo.RearLaserCode, customerInfo.FuelTypeVahan);


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


            }

            catch (Exception ex)
            {
                throw ex;
            }



            return customerInformationresponseData;
        }


        public async Task<dynamic> DateFormate()
        {
            var resultDateFormate = await _StickerService.DateFormate();
            return resultDateFormate;

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
