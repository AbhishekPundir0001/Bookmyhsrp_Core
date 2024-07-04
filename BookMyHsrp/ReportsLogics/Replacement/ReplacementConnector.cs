using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Models;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Services;
using BookMyHsrp.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static BookMyHsrp.Libraries.OemMaster.Models.OemMasterModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using BookMyHsrp.Libraries.Replacement.Services;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.ReplacementHsrpColorStickerModel;

namespace BookMyHsrp.ReportsLogics.Replacement
{
    public class ReplacementConnector
    {

        private readonly IReplacementService _replacementService;
        private readonly string nonHomo;
        private readonly string _nonHomoOemId;
        private readonly string OemId;
        public ReplacementConnector(IReplacementService hsrpColorStickerService, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            // _fetchDataAndCache = fetchDataAndCache; // Dependency injection
            // _hsrpWithColorStickerService = hsrpWithColorStickerService ?? throw new ArgumentNullException(nameof(hsrpWithColorStickerService));
            _replacementService = hsrpColorStickerService ?? throw new ArgumentNullException(nameof(hsrpColorStickerService));
            nonHomo = dynamicData.Value.NonHomo;
            _nonHomoOemId = dynamicData.Value.NonHomoOemId;
            OemId = dynamicData.Value.OemID;

        }
        public async Task<dynamic> VahanInformation(ReplacementVahanDetailsDto requestDto)
        {

            ICollection<ValidationResult> results = null;
            var vehicleValidationResponse = new ReplacementResponseDto();
            vehicleValidationResponse.data = new ReplacementVehicleValidation();
            if (!Validate(requestDto, out results))
            {
                vehicleValidationResponse.status = "false";
                vehicleValidationResponse.message = results.Select(x => x.ErrorMessage).FirstOrDefault();
                return vehicleValidationResponse;
            }
            var resultGot = await _replacementService.VahanInformation(requestDto);

            ReplacementVehicleValidation vehicleValidationData = new ReplacementVehicleValidation();
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

            string responseJson = await _replacementService.RosmertaApi(getVehicleRegno, getChassisNo, getEngineNo, "5UwoklBqiW");
            if (responseJson == "Error While Calling Vahan Service - The remote server returned an error: (500) Internal Server Error.")
            {
                vehicleValidationResponse.message = "Error While Calling Vahan Service - The remote server returned an error: (500) Internal Server Error.";
                return vehicleValidationResponse;
            }
            ReplacementVehicleDetails _vd = JsonConvert.DeserializeObject<ReplacementVehicleDetails>(responseJson);
            if (_vd != null)
            {
                if (_vd != null && _vd.stateCd != null && _vd.stateCd.ToLower().StartsWith(stateshortname.ToString().ToLower()) == false)
                {
                    vehicleValidationResponse.status = "0";
                    vehicleValidationResponse.message = "Please input Correct Registration Number of " + statename;
                    return vehicleValidationResponse;
                }
            }
            if (_vd != null && _vd.stateCd != null && _vd.message != "Vehicle Not Found")
            {
                var hasError = false;
                if (_vd.stateCd != null)
                {
                    if(stateID == 25)
                    {
                        if(_vd.hsrpFrontLaserCode == "" || _vd.hsrpRearLaserCode == "")
                        {
                            vehicleValidationResponse.status = "false"; 
                            vehicleValidationResponse.message = "You are not authorized to book re-order. For any query kindly mail to online@bookmyhsrp.com";
                            hasError = true;
                        }
                    }

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
                _vd.message = _vd.message.Replace("Vehicle status :- . ", "");
                if (_vd.message == "Vehicle details available in Vahan")
                {
                    bool CheckedStatus = false;
                    if (stateID == 25)
                    {
                        var bmhsrpOrissa = await _replacementService.strBmHSRPOrissa(requestDto.RegistrationNo, requestDto.ChassisNo, requestDto.EngineNo);
                        if(bmhsrpOrissa.Count >0)
                        {
                            CheckedStatus = await checkVehicleForDFDRDB(requestDto);
                            if (CheckedStatus == false)
                            {
                                vehicleValidationResponse.status = "false";
                                vehicleValidationResponse.message = "You are not authorized to book re-order. For any query kindly mail to online@bookmyhsrp.com";
                                return vehicleValidationResponse;
                            }
                        }
                    }
                    else
                    {
                        CheckedStatus = await checkVehicleForDFDRDB(requestDto);
                        if (CheckedStatus == false)
                        {
                            vehicleValidationResponse.status = "false";
                            vehicleValidationResponse.message = "You are not authorized to book re-order. For any query kindly mail to online@bookmyhsrp.com";
                            return vehicleValidationResponse;
                        }
                    }

                    var insertVahanLogQuery = _replacementService.InsertVaahanLog(getVehicleRegno, getChassisNo, getEngineNo, _vd);
                    var show_damage_both = true;
                    if (requestDto.isReplacement)
                    {
                        if (!string.IsNullOrEmpty(_vd.hsrpRearLaserCode) && !string.IsNullOrEmpty(_vd.hsrpFrontLaserCode))
                        {
                            if (!_vd.hsrpRearLaserCode.StartsWith("CC") && !_vd.hsrpFrontLaserCode.StartsWith("CC"))
                            {
                                show_damage_both = false;
                            }
                            var statusCheck = await CheckVehicleForDfdrdb(
                                new ReplacementVahanDetailsDto()
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
                                    "You are not authorized to book re-order. For any query kindly mail to support@bookmyhsrp.com";
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
                    var oemId = await _replacementService.GetOemId(_vd.maker);
                    if (oemId.Count > 0)
                    {
                        oemImgPath = oemId[0].oem_logo.ToString();
                        oemid = oemId[0].Oemid.ToString();
                        if (oemid == "20")
                        {
                            oemid = "272";
                        }
                        vehicleValidationData = new ReplacementVehicleValidation();
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
                    var insertVahanLogQuery = await _replacementService.InsertVaahanLog(getVehicleRegno, getChassisNo, getEngineNo, _vd);
                    var oemDetail = await _replacementService.GetOemId(_vd.maker);
                    if (oemDetail.Count > 0)
                    {
                        oemDetail[0].oem_logo.ToString();
                        oemid = oemDetail[0].Oemid.ToString();
                        vehicleValidationResponse.data.non_homo = "Y";
                        vehicleValidationResponse.data.oem_img_path = oemDetail[0].oem_logo.ToString();
                        var checkOrderExistsCHECK = await _replacementService.OemRtoMapping(getVehicleRegno, oemid);
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
                                    ReplacementVehicleDetails details = new ReplacementVehicleDetails();
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
                    var insertVahanLogQuery = await _replacementService.InsertVaahanLogWithoutApi(getVehicleRegno, getChassisNo, getEngineNo, _vd);
                    vehicleValidationResponse.status = "false";
                    vehicleValidationResponse.message = "Your vehicle detail didn't match with vahan service..";
                    vehicleValidationResponse.data = vehicleValidationData;
                    return vehicleValidationResponse;
                }

            }
            else
            {
                vehicleValidationResponse.status = "false";
                vehicleValidationResponse.message = "You are not authorized to book re-order. For any query kindly mail to online@bookmyhsrp.com";
                vehicleValidationResponse.data = vehicleValidationData;
                return vehicleValidationResponse;
            }
            return vehicleValidationResponse;
        }

        public async Task<List<OemVehicleTypeList>> GetOemVehicleTypes(string VehicleClass, string OemId)
        {
            List<OemVehicleTypeList> lst = new List<OemVehicleTypeList>();

            var vehicleDetails = await _replacementService.GetVehicleDetails(OemId, VehicleClass);



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


        public async Task<TrackOrderResponse> CheckVehicleForDfdrdb(ReplacementVahanDetailsDto info)
        {
            var trackOrderResponse = new TrackOrderResponse();

            ICollection<ValidationResult> results = null;
            if (!Validate(info, out results))
            {
                trackOrderResponse.status = "false";
                trackOrderResponse.message = results.Select(x => x.ErrorMessage).FirstOrDefault();
                return trackOrderResponse;
            }
            var getOrderNumber = await _replacementService.GetOrderNumber(info);
            if (getOrderNumber)
            {

                var data = await _replacementService.SelectByOrderNumber(getOrderNumber[0].Orderno);
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
                var result = await _replacementService.CheckDateBetweenCloseDate(info);

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
            var data = _replacementService.SessionBookingDetails(requestDto);
            return data;


        }
        public async Task<dynamic> CustomerInfo(ReplacementCustomerInfoModel customerInfo, dynamic sessionDetails)
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
            string realOrdertype = string.Empty;
            string ReplacementType = customerInfo.ReplacementType;
            var qstr = "";
            string insertVahanLogQuery = "";
            var session = new Session();
            session.IsOTPVerify = "N";
            session.OTPno = null;
            session.VehicleType_imgPath = "www";
            session.OEMImgPath = "www";
            var jsonDeSerializer = System.Text.Json.JsonSerializer.Deserialize<RootDto>(sessionDetails);
            try
            {
                if (customerInfo.OrderType.Trim().ToUpper() == "BDB")
                {
                    realOrdertype = "DB";

                }
                else if (customerInfo.OrderType.Trim().ToUpper() == "BDR")
                {
                    customerInfo.OrderType = "DR";

                }
                else if (customerInfo.OrderType.Trim().ToUpper() == "BDF")
                {
                    realOrdertype = "DF";
                }
                else
                {
                    realOrdertype = "OB";
                }



                var nonHomo = jsonDeSerializer.NonHomo;
                if (nonHomo == "Y")
                {
                    var vehicleCategory = customerInfo.VehicleCatVahan;
                    customerInformationresponseData.data.NonHomoVehicleType = vehicleCategory;
                }
                IsVehicletypeEnable = "N";
                var oemId = jsonDeSerializer.OemId;
                var taxInvoiceSummary = await _replacementService.TaxInvoiceSummary(oemId);
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
                    #region Check Registartion date for Rajasthan State on or before 01.04.2029
                    if (customerInfo.StateId == "27")
                    {
                        string dateString = customerInfo.RegistrationDate;
                        IFormatProvider theCultureInfo = new CultureInfo("en-GB", true);
                        var resultDateTime = DateTime.TryParseExact(dateString, "dd/MM/yyyy", theCultureInfo, DateTimeStyles.None, out var dt) ? dt : null as DateTime?;
                        DateTime to = DateTime.ParseExact("01/04/2019", "dd/MM/yyyy", theCultureInfo);
                        if (resultDateTime.HasValue)
                        {
                            if (RunCheckIfOrderCanBeTakenAfter2019(stateId: getStateId))
                            {
                                var txtTotalDays = ((resultDateTime.Value - to).TotalDays);
                                if (txtTotalDays > 0)
                                {
                                    customerInformationresponseData.Status = "false";
                                    customerInformationresponseData.Message = "Vehicle owner's with vehicles manufactured after 01 April 2019, should contact their respective Automobile Dealers for HSRP affixation.";
                                    return customerInformationresponseData;
                                }
                            }
                        }
                    }
                    #endregion
                    //string dateString = customerInfo.RegistrationDate;
                    //DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //string formattedDate = date.ToString("dd-MM-yyyy");

                    //IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                    //DateTime from_date = DateTime.ParseExact(formattedDate, "dd-MM-yyyy", theCultureInfo);
                    //DateTime to = DateTime.ParseExact("25-11-2019", "dd-MM-yyyy", theCultureInfo);
                    //string txt_total_days = ((from_date - to).TotalDays).ToString();
                    //int diffResult = int.Parse(txt_total_days.ToString());
                    //if (customerInfo.StateId != "25")
                    //{
                    //    if (diffResult >= 0)
                    //    {
                    //        customerInformationresponseData.Message = "Vehicle owner's with vehicles manufactured after 1st April 2019, should contact their respective Automobile Dealers for HSRP affixation";
                    //        return customerInformationresponseData;
                    //    }
                    //}
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
                var bookingHistoryId = await _replacementService.BookingHistoryId(customerInfo.RegistrationNo, customerInfo.ChassisNo, customerInfo.EngineNo);
                if (bookingHistoryId.Count > 0)
                {
                    var insertVahanLogQueryCustomer = await _replacementService.InsertVahanLogQueryCustomer(customerInfo, OrderType, billingaddress, NonHomo, OemId);
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
                    var insertVahanLogQueryCustomer = await _replacementService.InsertVahanLogQueryCustomer(customerInfo, OrderType, billingaddress, NonHomo, OemId);

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
                    var vehiclecategory = "";
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
                        int vehicletypeid;
                        var vehicletypeidIntoString = "";
                        var vehicleSession = await _replacementService.VehicleSession(customerInfo.VehicleCatVahan);
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
                                var oemIdNew = await _replacementService.GetOemId(customerInfo.MakerVahan);
                                int newId = 0;
                                foreach (var Id in oemIdNew)
                                {
                                    newId = Id.Oemid;
                                }
                                var getOemVehicleType = await _replacementService.OemVehicleType(HSRPHRVehicleType, customerInfo.VehicleTypeVahan, newId, customerInfo.FuelTypeVahan);
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
                                    var insertMissmatchDataLog = await _replacementService.InsertMisMatchDataLog(customerInfo, oemIdNew);
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
                    customerInformationresponseData.data.RealOrderType = realOrdertype;
                    customerInformationresponseData.data.VehicleCat = vehiclecategory;
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


        public async Task<bool> checkVehicleForDFDRDB(dynamic requestDto)
        {
            bool checker = false;
            var data = await _replacementService.strBmhsrp1(requestDto.RegistrationNo, requestDto.ChassisNo, requestDto.EngineNo);
            if(data.Count>0)
            {
                int days = 0;
                var data2 = await _replacementService.strBmhsrp2(requestDto.RegistrationNo, requestDto.ChassisNo, requestDto.EngineNo);
                if (data2[0].row_count == 0)
                {
                    days = 7;
                }
                else if (data2[0].row_count == 1)
                {
                    days = 90;
                }
                else if (data2[0].row_count == 2)
                {
                    days = 180;
                }

                var hsrpdata1 = await _replacementService.strHsrpRecord1(requestDto.RegistrationNo, requestDto.ChassisNo);
                if (hsrpdata1.Count >= 2)
                {
                    days = 180;
                }
                else if(hsrpdata1.Count == 1)
                {
                    days = 90;
                }

                var hsrpdata2 = await _replacementService.strHsrpRecord2(requestDto.RegistrationNo, requestDto.ChassisNo);
                if(hsrpdata2.Count >3)
                {
                    days = 180;
                }

                var hsrpdata3 = await _replacementService.strHsrpRecord3(data[0].Orderno,days);
                if (hsrpdata3.Count > 0)
                {
                    if (hsrpdata3[0].ReBookingAllow == "Y")
                    {
                        checker = true;
                    }
                }

            }
            else
            {
                var data4 = await _replacementService.strHsrpRecord4(requestDto.RegistrationNo, requestDto.ChassisNo);
                if(data4.Count >0)
                {
                    if (data4.Count == 1)
                    {
                        int recordId = Convert.ToInt32(data4[0].HSRPRecordId);
                        var data6= await _replacementService.strHsrpRecord6(requestDto.RegistrationNo, requestDto.ChassisNo, recordId.ToString());
                        if(data6.Count>0)
                        {
                            if (data6[0].ReBookingAllow == "Y")
                            {
                                checker = true;
                            }
                        }
                    }
                    if (data4.Count == 2)
                    {
                        var data7 = await _replacementService.strHsrpRecord7(requestDto.RegistrationNo, requestDto.ChassisNo);
                        if(data7.Count>0)
                        {
                            if (data7[0].ReBookingAllow == "Y")
                            {
                                checker = true;
                            }
                        }
                    }
                }
                else
                {
                    var data8 = await _replacementService.strHsrpRecord8(requestDto.RegistrationNo, requestDto.ChassisNo);
                    if(data8.Count>0)
                    {
                        var data9 = await _replacementService.strHsrpRecord9(requestDto.RegistrationNo, requestDto.ChassisNo);
                        if (data9.Count > 0)
                        {
                            if (data9[0].ReBookingAllow == "Y")
                            {
                                checker = true;
                            }
                        }
                    }
                    else
                    {
                        var data10 = await _replacementService.strHsrpRecord9(requestDto.RegistrationNo, requestDto.ChassisNo);
                        if (data10.Count > 0)
                        {
                            if (data10[0].ReBookingAllow == "Y")
                            {
                                checker = true;
                            }
                        }
                    }
                }
            }

            return checker;
        }


    }
}
