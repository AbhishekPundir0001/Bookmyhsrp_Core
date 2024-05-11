using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Services;
using BookMyHsrp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
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
        public HsrpWithColorStickerConnector(HsrpWithColorStickerService hsrpColorStickerService, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            // _fetchDataAndCache = fetchDataAndCache; // Dependency injection
           // _hsrpWithColorStickerService = hsrpWithColorStickerService ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerService));
            _hsrpColorStickerService = hsrpColorStickerService ?? throw new ArgumentNullException(nameof(hsrpColorStickerService));
            nonHomo = dynamicData.Value.NonHomo;
            _nonHomoOemId = dynamicData.Value.NonHomoOemId;

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
            var oemImgPath = string.Empty;
            var oemid = string.Empty;
            int stateID = getStateId;
            foreach (var data in resultGot)
            {
                stateshortname = data.HSRPStateShortName;
                statename= data.HSRPStateName;
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
        
        public  async Task<List<OemVehicleTypeList>> GetOemVehicleTypes(string VehicleClass, string OemId)
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


        public  async Task<TrackOrderResponse> CheckVehicleForDfdrdb(VahanDetailsDto info)
                {
                    var trackOrderResponse = new TrackOrderResponse();

                    ICollection<ValidationResult> results = null;
                    if (!Validate(info, out results))
                    {
                        trackOrderResponse.status = "false";
                        trackOrderResponse.message = results.Select(x => x.ErrorMessage).FirstOrDefault();
                        return trackOrderResponse;
                    }
            var getOrderNumber =await _hsrpColorStickerService.GetOrderNumber(info);
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
        //    public static async Task<CustomerInformationResponse> CustomerInformation(
        //     CustomerInfoModel customerInfo)
        //{


        //    var customerInformationresponseData = new CustomerInformationResponse();
        //    var customerInformationData = new CustomerInformationData();
        //    ICollection<ValidationResult> results = null;
        //    if (!Validate(customerInfo, out results))
        //    {
        //        customerInformationresponseData.status = "false";
        //        customerInformationresponseData.message = results.Select(x => x.ErrorMessage).FirstOrDefault();
        //        return customerInformationresponseData;
        //    }
        //    string getStateId = customerInfo.stateid;
        //    string getOemId = customerInfo.oemid;
        //    string getNonHomo = customerInfo.non_homo;
        //    string getOrderType = customerInfo.order_type;
        //    string getVehicleRegno = customerInfo.vehicleregno;
        //    string getChassisNo = customerInfo.chassisno;
        //    string getEngineNo = customerInfo.engineno;
        //    string getBhartStage = customerInfo.bhart_stage;
        //    string getVehRegDate = customerInfo.veh_reg_date;
        //    string getCustomerName = customerInfo.customer_name;
        //    string getCustomerEmail = customerInfo.customer_email;
        //    string getCustomerMobile = customerInfo.customer_mobile;
        //    string getCustomerBillingAddress = customerInfo.customer_billing_address;
        //    string getCustomerState = customerInfo.customer_state;
        //    string getCustomerCity = customerInfo.customer_city;
        //    string getCustomerGstin = customerInfo.customer_gstin;
        //    string getRCFile = customerInfo.rc_file;
        //    string getVehMaker = customerInfo.maker;
        //    string getVehicleType = customerInfo.vehicle_type;
        //    string getFuelType = customerInfo.fuel_type;
        //    string getVehicleCategory = customerInfo.vehicle_category;
        //    string getOrderPlateSticker = customerInfo.plate_sticker;
        //    string OemVehicleType = customerInfo.OemVehicleType;
        //    string IsVehicletypeEnable = "N";
        //    string realOrdertype = "OB";
        //    string ReplacementType = customerInfo.ReplacementType;
        //    var qstr = "";
        //    string insertVahanLogQuery = "";


        //}
        static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        }

    }
}
