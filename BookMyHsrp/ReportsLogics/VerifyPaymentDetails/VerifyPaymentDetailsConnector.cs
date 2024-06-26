using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.TrackYoutOrder.Services;
using BookMyHsrp.Libraries.VerifyPaymentDetail.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using Razorpay.Api;
using StackExchange.Redis;
using System.Data;
using System.Net;
using System.Text;
using static BookMyHsrp.Libraries.VerifyPaymentDetail.Models.VerifyPaymentDetailModel;
using static iTextSharp.text.pdf.AcroFields;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BookMyHsrp.ReportsLogics.VerifyPaymentDetails
{
    public class VerifyPaymentDetailsConnector
    {
        private readonly IVerifyPaymentDetailService _verifyPaymentDetailService;
        private readonly string _key;
        private readonly string _secret;
        private readonly string _Host;
        string Order_No = string.Empty;
        public VerifyPaymentDetailsConnector(IVerifyPaymentDetailService verifyPaymentDetailService, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            _verifyPaymentDetailService = verifyPaymentDetailService ?? throw new ArgumentNullException(nameof(verifyPaymentDetailService));
            _key = dynamicData.Value.key;
            _secret = dynamicData.Value.secret;
            _Host = dynamicData.Value.Host;
        }
        public async Task<dynamic> CheckSuperTagRate(dynamic vehicledetails, dynamic userdetails, dynamic DealerAppointment)
        {
            string status = "";
            decimal BasicItemPrice =0;
            decimal CGSTAmount = 0;
            decimal IGSTAmount = 0;
            decimal SGSTAmount = 0;
            var model = new GetVerifyPaymentDetailModel();
            var result = await _verifyPaymentDetailService.CheckSuperTagRate();
            if (result.Count > 0)
            {

                foreach (var item in result)
                {
                    status = item.ActiveStatus;
                    BasicItemPrice = item.BasicItemPrice;
                    CGSTAmount = item.CGSTAmount;
                    IGSTAmount = item.IGSTAmount;
                    SGSTAmount = item.SGSTAmount;
                }
                if (status == "Y")
                {

                    var vehicleCategory = userdetails.VehicleCat;
                    if (vehicleCategory.Trim().ToUpper() == "2W" || vehicleCategory.Trim().ToUpper() == "3W")
                    {
                        model.ParkPlusArea = false;
                    }
                    else
                    {
                        model.ParkPlusArea = true;
                    }
                    model.SuperTagAmount = "Delivered to Park+FasTag ship to address in 7-10 days";
                    model.HdnGstBasicAmtST = BasicItemPrice.ToString();
                    model.HdnCGSTAmountST = CGSTAmount.ToString();
                    model.HdnIGSTAmountST = IGSTAmount.ToString(); ;
                    model.HdnSGSTAmountST = SGSTAmount.ToString(); ;

                }
                else
                {
                    model.SuperTagAmount = "true";
                    model.HdnGstBasicAmtST = "00.00";
                    model.HdnCGSTAmountST = "00.00";
                    model.HdnIGSTAmountST = "00.00";
                    model.HdnSGSTAmountST = "00.00";
                    model.ParkPlusArea = false;
                }
            }
            else
            {
                model.SuperTagAmount = "true";
                model.HdnGstBasicAmtST = "00.00";
                model.HdnCGSTAmountST = "00.00";
                model.HdnIGSTAmountST = "00.00";
                model.HdnSGSTAmountST = "00.00";
                model.ParkPlusArea = false;
            }
            if (vehicledetails.DeliveryPoint == "Home")
            {
                model.DivFrame = true;
                string orderType = userdetails.OrderType == null ? "" : userdetails.OrderType.ToString();
                if (orderType == "")
                {

                }
                string _VehicleTyeForFrame = string.Empty;
                var vehicletype = userdetails.VehicleType;
                if (vehicletype != null)
                {
                    _VehicleTyeForFrame = vehicletype;
                    if (_VehicleTyeForFrame == "Scooter_2W")
                    {
                        _VehicleTyeForFrame = "Scooter";
                    }
                }

                var data1 = await _verifyPaymentDetailService.CheckFrameRate(_VehicleTyeForFrame, orderType);
                if (data1.Count > 0)
                {
                    model.FrameArea = true;
                    foreach (var item in data1)
                    {
                        status = item.ActiveStatus;
                        BasicItemPrice = item.BasicItemPrice;
                        CGSTAmount = item.CGSTAmount;
                        IGSTAmount = item.IGSTAmount;
                        SGSTAmount = item.SGSTAmount;
                    }

                }

            }
            else
            {
                model.SuperTagAmount = "true";
                model.HdnGstBasicAmtST = "00.00";
                model.HdnCGSTAmountST = "00.00";
                model.HdnIGSTAmountST = "00.00";
                model.HdnSGSTAmountST = "00.00";
                model.DivSupertag = true;
                model.FrameArea = false;
                model.CheckShipping = false;
                model.TrDeliveryCharge = false;
            }
          var data = await  PaymentDescription(vehicledetails, userdetails, DealerAppointment);
            return data;

        }
        public async Task<dynamic> PaymentDescription(dynamic vehicledetails, dynamic userdetails,dynamic DealerAppointment)
        {
            var model = new GetVerifyPaymentDetailModel();
            if (vehicledetails.StateId == "27")
            {
                var vehicletype = userdetails.VehicleType;
                string orderType = userdetails.OrderType == null ? "" : userdetails.OrderType.ToString();
                var checkOemRate = await _verifyPaymentDetailService.CheckOemRate(orderType, vehicledetails.StateId, vehicletype);
                if (checkOemRate.Count > 0)
                {

                    model.IGSTAmount = 0;
                    model.CGSTAmount = 0;
                    model.SGSTAmount = 0;
                    model.FrontPlateSize = 0;
                    model.RearPlateSize = 0;
                    model.GstBasic_Amt = 0;
                    model.FittmentCharges = 0;
                    model.BMHConvenienceCharges = 0;
                    //decimal GrossTotal = Convert.ToDecimal(dtTax.Rows[0]["GrossTotal"]);
                    decimal GrossTotal = 0;
                    decimal GSTAmount = 0;
                    foreach (var item in checkOemRate)
                    {
                        model.FrontPlateSize = item.FrontPlateSize;
                        model.RearPlateSize = item.RearPlateSize;
                        model.GstBasic_Amt = Convert.ToDecimal(item.GSTBasicAmount) + Convert.ToDecimal(item.FittmentCharges);
                        model.FittmentCharges = Convert.ToDecimal(item.FittmentCharges);
                        model.BMHConvenienceCharges = Convert.ToDecimal(item.BMHConvenienceCharges);
                        GrossTotal = Convert.ToDecimal(item.GstBasic_Amt) + Convert.ToDecimal(model.BMHConvenienceCharges);
                        GSTAmount = Convert.ToDecimal(item.GSTAmount);

                    }
                    if (vehicledetails.StateIdBackup == vehicledetails.StateId)
                    {

                        foreach (var item in checkOemRate)
                        {

                            model.CGSTAmount = Convert.ToDecimal(item.CGSTAmount);
                            model.SGSTAmount = Convert.ToDecimal(item.SGSTAmount);
                        }
                    }
                    else
                    {
                        model.IGSTAmount = model.CGSTAmount + model.SGSTAmount;

                    }
                    model.GstRate = GSTAmount;
                    foreach (var item in checkOemRate)
                    {

                        model.NetAmount = item.NetAmount;
                    }
                    if (orderType == "BDR")
                    {
                        if (userdetails.VehicleCat == "2W")
                        {
                            model.HSRPFitmentCost = true;
                            model.HSRPFitmentCostStr = false;
                            model.HSRPFitCostFP = false;
                            model.HSRPFitCostFPStr = false;
                            model.HSRPCompSet = false;
                            model.HSRPCompSetStr = false;
                            model.HSRPFitCost = false;


                        }
                        else
                        {
                            model.HSRPFitmentCost = false;
                            model.HSRPFitmentCostStr = true;
                            model.HSRPFitCostFP = false;
                            model.HSRPFitCostFPStr = false;
                            model.HSRPCompSet = false;
                            model.HSRPCompSetStr = false;
                            model.HSRPFitCost = false;

                        }
                    }
                    else if (orderType == "BDF")
                    {
                        if (userdetails.VehicleCat == "2W")
                        {
                            model.HSRPFitmentCost = false;
                            model.HSRPFitmentCostStr = false;
                            model.HSRPFitCostFP = true;
                            model.HSRPFitCostFPStr = false;
                            model.HSRPCompSet = false;
                            model.HSRPCompSetStr = false;
                            model.HSRPFitCost = false;
                        }
                        else
                        {
                            model.HSRPFitmentCost = false;
                            model.HSRPFitmentCostStr = false;
                            model.HSRPFitCostFP = false;
                            model.HSRPFitCostFPStr = true;
                            model.HSRPCompSet = false;
                            model.HSRPCompSetStr = false;
                            model.HSRPFitCost = false;
                        }


                    }
                    else if (orderType == "OB" || orderType == "BDB")
                    {
                        if (userdetails.VehicleCat == "2W")
                        {
                            model.HSRPFitmentCost = false;
                            model.HSRPFitmentCostStr = false;
                            model.HSRPFitCostFP = false;
                            model.HSRPFitCostFPStr = false;
                            model.HSRPCompSet = true;
                            model.HSRPCompSetStr = false;
                            model.HSRPFitCost = false;
                        }
                        else
                        {
                            model.HSRPFitmentCost = false;
                            model.HSRPFitmentCostStr = false;
                            model.HSRPFitCostFP = false;
                            model.HSRPFitCostFPStr = false;
                            model.HSRPCompSet = false;
                            model.HSRPCompSetStr = true;
                            model.HSRPFitCost = false;
                        }
                    }
                    else
                    {
                        model.HSRPFitmentCost = false;
                        model.HSRPFitmentCostStr = false;
                        model.HSRPFitCostFP = false;
                        model.HSRPFitCostFPStr = false;
                        model.HSRPCompSet = false;
                        model.HSRPCompSetStr = true;
                        model.HSRPFitCost = true;
                    }
                }
                else
                {
                    model.Message = "Rate not found.!!";
                    return model;
                }
                try
                {
                    model.OwnerName = userdetails.CustomerName;
                    model.OwnerName = userdetails.CustomerEmail;
                    model.Mobile = userdetails.CustomerMobile;
                    model.Address= userdetails.CustomerBillingAddress;
                    model.PinCode= "";
                }
                catch
                {

                }
            }
            else
            {
               var orderType = userdetails.OrderType == null ? "" : userdetails.OrderType.ToString();
                var oemid = vehicledetails.OemId;
                var vehicleCLASS = vehicledetails.VehicleClass;
                var checkOemRate = await _verifyPaymentDetailService.CheckOemRateQuery(vehicledetails,userdetails, DealerAppointment, orderType);
                if(checkOemRate.Count>0)
                {
                    var status = "";
                    foreach(var item in checkOemRate)
                    {
                        status = Convert.ToString(item.status);
                        model.GstBasic_Amt = item.GstBasic_Amt;
                        model.FittmentCharges = item.FittmentCharges;
                        model.BMHConvenienceCharges = item.BMHConvenienceCharges;
                        model.BMHHomeCharges = item.BMHHomeCharges;
                        model.FrontPlateSize = item.FrontPlateSize;
                        model.RearPlateSize = item.RearPlateSize;
                    }
                    if (status == "1")
                    {
                        if (DealerAppointment.DeliveryPoint == "Dealer")
                        {
                            if (model.GstBasic_Amt.ToString() == "0.00" || model.GstBasic_Amt.ToString() == null || model.FittmentCharges.ToString() == "0.00" || model.FittmentCharges.ToString() == null || model.BMHConvenienceCharges.ToString() == "0.00" || model.BMHConvenienceCharges.ToString() == null)
                            {
                                model.Message = "Rates not matched. For any query kindly mail to online@bookmyhsrp.com";
                                return model;
                            }
                        }
                        else
                        {
                            if (model.GstBasic_Amt.ToString() == "0.00" || model.GstBasic_Amt.ToString() == null || model.FittmentCharges.ToString() == "0.00" || model.FittmentCharges.ToString() == null || model.BMHConvenienceCharges.ToString() == "0.00" || model.BMHConvenienceCharges.ToString() == null || model.BMHHomeCharges.ToString() == "0.00" || model.BMHHomeCharges.ToString() == null)
                            {
                                model.Message = "Rates not matched. For any query kindly mail to online@bookmyhsrp.com";
                                return model;
                            }
                        }
                        foreach (var item in checkOemRate)
                        {
                            model.FrontPlateSize = item.FrontPlateSize;
                            model.RearPlateSize = item.RearPlateSize;
                            model.GstBasic_Amt = item.GstBasic_Amt + item.FittmentCharges;
                            model.FittmentCharges = item.FittmentCharges;
                            model.BMHConvenienceCharges = item.BMHConvenienceCharges;
                            model.BMHHomeCharges = item.BMHHomeCharges;
                            model.GrossTotal = item.GrossTotal;
                            model.GSTAmount = item.GSTAmount;
                            model.IGSTAmount = item.IGSTAmount;
                            model.CGSTAmount = item.CGSTAmount;
                            model.SGSTAmount = item.SGSTAmount;
                            model.GstRate = item.gst;
                            model.TotalAmount = item.TotalAmount;
                            model.MRDCharges = item.MRDCharges;

                        }
                        if (model.MRDCharges.ToString() == "0.00")
                        {
                            model.HdnMRDCharges = "0.00";
                        }
                        if (orderType == "BDR")
                        {
                            if (userdetails.VehicleCat == "2W")
                            {
                                model.HSRPFitmentCost = true;
                                model.HSRPFitmentCostStr = false;
                                model.HSRPFitCostFP = false;
                                model.HSRPFitCostFPStr = false;
                                model.HSRPCompSet = false;
                                model.HSRPCompSetStr = false;
                                model.HSRPFitCost = false;


                            }
                            else
                            {
                                model.HSRPFitmentCost = false;
                                model.HSRPFitmentCostStr = true;
                                model.HSRPFitCostFP = false;
                                model.HSRPFitCostFPStr = false;
                                model.HSRPCompSet = false;
                                model.HSRPCompSetStr = false;
                                model.HSRPFitCost = false;

                            }
                        }
                        else if (orderType == "BDF")
                        {
                            if (userdetails.VehicleCat == "2W")
                            {
                                model.HSRPFitmentCost = false;
                                model.HSRPFitmentCostStr = false;
                                model.HSRPFitCostFP = true;
                                model.HSRPFitCostFPStr = false;
                                model.HSRPCompSet = false;
                                model.HSRPCompSetStr = false;
                                model.HSRPFitCost = false;
                            }
                            else
                            {
                                model.HSRPFitmentCost = false;
                                model.HSRPFitmentCostStr = false;
                                model.HSRPFitCostFP = false;
                                model.HSRPFitCostFPStr = true;
                                model.HSRPCompSet = false;
                                model.HSRPCompSetStr = false;
                                model.HSRPFitCost = false;
                            }


                        }
                        else if (orderType == "OB" || orderType == "BDB")
                        {
                            if (userdetails.VehicleCat == "2W")
                            {
                                model.HSRPFitmentCost = false;
                                model.HSRPFitmentCostStr = false;
                                model.HSRPFitCostFP = false;
                                model.HSRPFitCostFPStr = false;
                                model.HSRPCompSet = true;
                                model.HSRPCompSetStr = false;
                                model.HSRPFitCost = false;
                            }
                            else
                            {
                                model.HSRPFitmentCost = false;
                                model.HSRPFitmentCostStr = false;
                                model.HSRPFitCostFP = false;
                                model.HSRPFitCostFPStr = false;
                                model.HSRPCompSet = false;
                                model.HSRPCompSetStr = true;
                                model.HSRPFitCost = false;
                            }
                        }
                        else
                        {
                            model.HSRPFitmentCost = false;
                            model.HSRPFitmentCostStr = false;
                            model.HSRPFitCostFP = false;
                            model.HSRPFitCostFPStr = false;
                            model.HSRPCompSet = false;
                            model.HSRPCompSetStr = true;
                            model.HSRPFitCost = true;
                        }

                    }
                    else
                    {
                        foreach (var item in checkOemRate)
                        {
                            model.Message = item.message;

                        }
                        return model;
                    }

                }
                else
                {
                    model.Message = "Rate not found.!!!";
                    return model;
                }
                try
                {
                    model.OwnerName = userdetails.CustomerName;
                    model.EmailID = userdetails.CustomerEmail;
                    model.Mobile = userdetails.CustomerMobile;
                    model.Address = userdetails.CustomerBillingAddress;
                    model.PinCode = "";
                }
                catch
                {

                }

            }
            return model;
        }
        public async Task<dynamic> Payment(dynamic vehicledetails, dynamic userdetails, dynamic DealerAppointment, dynamic bookingDetail,string ip,string Payment,dynamic timeSlotChecking)
        {
            var data = "";
            var modelResult = new PaymentDetails();
            string realOrdertype = string.Empty;
            string orderType = userdetails.OrderType == null ? "" : userdetails.OrderType.ToString();
            if (orderType == "BDB")
            {
                realOrdertype = "DB";
            }
            else if (orderType == "BDR")
            {
                realOrdertype = "DR";
            }
            else if (orderType == "BDF")
            {
                realOrdertype = "DF";
            }
            else
            {
                realOrdertype = "OB";
            }
            if (realOrdertype == "OB")
            {
                var result = await _verifyPaymentDetailService.GetBookingId(vehicledetails, userdetails, DealerAppointment, realOrdertype);
                if (vehicledetails.VehicleRegNo == "DL10CG7191")
                {

                }
                else
                {
                    if (result.Count > 0)
                    {
                        modelResult.Message = "Order for this registration number already exists. For any query kindly mail to online@bookmyhsrp.com";
                        return modelResult;
                    }
                }
            }
            else
            {
                if (vehicledetails.StateId == "25")
                {
                    modelResult.CheckedStatus = false;
                    modelResult.CheckedStatus = CheckVehicleForDFDRDB(vehicledetails.VehicleRegNo, vehicledetails.ChassisNo, vehicledetails.EngineNo);
                    if (modelResult.CheckedStatus == false)
                    {
                        modelResult.Message = "Order for this registration number already exists. For any query kindly mail to online@bookmyhsrp.com";
                        return modelResult;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(modelResult.Message))
            {
                try
                {
                    var order_status = "Failed";
                    var failure_message = "User started new transaction.";
                    var payment_gateway_type = "razorpay";
                    string paymentFailedQuery = await _verifyPaymentDetailService.PaymentConfirmation(order_status, failure_message, payment_gateway_type);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    //need to add in logger
                }
            }
            var appointmentBlockDateQuery = await _verifyPaymentDetailService.AppointmentBlockDate(bookingDetail.SlotDate, DealerAppointment.DealerAffixationCenterId, DealerAppointment.DeliveryPoint);
            if (DealerAppointment.Affix != "Affixval")
            {

                var status = "";
                foreach (var item in appointmentBlockDateQuery)
                {
                    status = item.status.ToString();
                }
                if (appointmentBlockDateQuery.Count > 0 && status == "1")
                {
                    modelResult.Message = "Slot booked. Choose another Slot.";
                    return modelResult;
                }
                else if (appointmentBlockDateQuery.Count == 0)
                {
                    modelResult.Message = "Slot booked. Choose another Slot.";
                    return modelResult;
                }
            }
            if (DealerAppointment.DeliveryPoint == "Dealer")
            {
                var getOemId = await _verifyPaymentDetailService.GetOemId(DealerAppointment.DealerAffixationCenterId);
                if (getOemId.Count > 0)
                {
                    int OemId = 0;
                    foreach (var item in getOemId)
                    {
                        OemId = item.oemid;
                    }
                    if (Convert.ToInt32(vehicledetails.OemId) != OemId)
                    {
                        modelResult.Message = "Selected dealer is not valid.";
                    }
                }
            }
            modelResult.ChkFastTag = false;
            foreach (var slotInfo in timeSlotChecking)
            {
                if (slotInfo.SlotName == bookingDetail.SlotTime)
                {
                    modelResult.SlotTime= slotInfo.SlotName.ToString();
                    modelResult.SlotId = slotInfo.SlotID.ToString();// Return SlotID if SlotName matches time
                }
            }
            modelResult.ChkFrame = false;
            modelResult.SlotId = modelResult.SlotId;
            modelResult.SlotTime = modelResult.SlotTime;
            modelResult.SlotBookingDate = bookingDetail.SlotDate;
            modelResult.HSRPStateID = vehicledetails.StateId;
            modelResult.RTOLocationID = string.Empty;
            modelResult.RTOName = string.Empty;
            modelResult.OwnerName = userdetails.CustomerName;
            modelResult.OwnerFatherName = "";
            modelResult.Address1 = userdetails.CustomerBillingAddress;
            modelResult.State = vehicledetails.StateName;
            modelResult.City = "";
            modelResult.Pin = "";
            modelResult.MobileNo = userdetails.CustomerMobile;
            modelResult.LandlineNo = "";
            modelResult.EmailID = userdetails.CustomerEmail;
            modelResult.VehicleClass = vehicledetails.VehicleClass;
            modelResult.VehicleType = userdetails.VehicleType;
            modelResult.ManufacturerName = "";
            modelResult.ChassisNo = vehicledetails.ChassisNo;
            modelResult.EngineNo = vehicledetails.EngineNo;
            modelResult.VehicleRegNo = vehicledetails.VehicleRegNo;
            modelResult.ManufacturingYear = userdetails.RegistrationDate;
            modelResult.FrontPlateSize = "";
            modelResult.RearPlateSize = "";
            modelResult.TotalAmount = "";
            modelResult.NetAmount = "";
            modelResult.BookingType = "";
            modelResult.FuelType = vehicledetails.FuelType;
            modelResult.DealerId = "";
            modelResult.OEMID = vehicledetails.OemId;
            modelResult.BookedFrom = "Website";
            modelResult.AppointmentType = DealerAppointment.DeliveryPoint;
            modelResult.BasicAmount = "";
            modelResult.FitmentCharge = "";
            modelResult.ConvenienceFee = "";
            modelResult.HomeDeliveryCharge = "";
            modelResult.GSTAmount = "";
            modelResult.IGSTAmount = "0";
            modelResult.CGSTAmount = "0";
            modelResult.SGSTAmount = "0";
            modelResult.CustomerGSTNo = "0";
            modelResult.BharatStage = userdetails.BhartStage;
            modelResult.ShippingAddress1 = "";
            modelResult.ShippingAddress2 = "";
            modelResult.ShippingCity = "";
            modelResult.ShippingState = "";
            modelResult.ShippingPinCode = "";
            modelResult.ShippingLandMark = "";
            modelResult.DealerAffixationAddress = "";
            modelResult.NonHomologVehicle = vehicledetails.NonHomo;
            modelResult.TotalAmountST = 0;
            modelResult.Basic_AmtST = 0;
            modelResult.CGSTAmountST = 0;
            modelResult.IGSTAmountST = 0;
            modelResult.SGSTAmountST = 0;
            modelResult.ErpItemCode = string.Empty;
            modelResult.TotalAmountFrm = 0;
            modelResult.Basic_AmtFrm = 0;
            modelResult.CGSTAmountFrm = 0;
            modelResult.IGSTAmountFrm = 0;
            modelResult.SGSTAmountFrm = 0;
            modelResult.FrontHSRPFileName = string.Empty;
            modelResult.RearHSRPFileName = string.Empty;
            modelResult.FileFIR = string.Empty;
            modelResult.FirDate = string.Empty;
            modelResult.Firinfo = string.Empty;
            modelResult.PoliceStation = string.Empty;
            modelResult.Firno = string.Empty;
            modelResult.FrontLaserCode = string.Empty;
            modelResult.RearLaserCode = string.Empty;
            modelResult.ReplacementReason = string.Empty;
            if (orderType == "")
            {

            }
            if (orderType != "" && orderType != "OB")
            {
                if (Convert.ToInt32(modelResult.HSRPStateID) != 25)
                {
                    if (modelResult.ReplacementReason == "LT" || modelResult.ReplacementReason == "BD")
                    {
                        if (modelResult.VehicleRCImage == "")
                        {

                        }
                        if (orderType == "BDB")
                        {
                            if (modelResult.FileFIR == "" || modelResult.FirDate == "" || modelResult.PoliceStation == "" || modelResult.Firno == "")
                            {

                            }
                        }
                        if (orderType == "BDF")
                        {
                            if (modelResult.RearHSRPFileName == "" || modelResult.FileFIR == "" || modelResult.FirDate == "" || modelResult.PoliceStation == "" || modelResult.Firno == "")
                            {

                            }
                        }
                        if (orderType == "BDR")
                        {
                            if (modelResult.RearHSRPFileName == "" || modelResult.FileFIR == "" || modelResult.FirDate == "" || modelResult.PoliceStation == "" || modelResult.Firno == "")
                            {

                            }
                        }
                    }
                }
                else if (Convert.ToInt32(modelResult.HSRPStateID) == 25)
                {
                    if (modelResult.ReplacementReason == "LT")
                    {
                        if (orderType == "BDF" || orderType == "BDR" || orderType == "BDB")
                        {
                            if (modelResult.FileFIR == "" || modelResult.FirDate == "" || modelResult.PoliceStation == "" || modelResult.Firno == "")
                            {

                            }
                        }
                    }
                }
            }
            if (DealerAppointment.DeliveryPoint == "Home")
            {

            }
            try
            {
                var checkdealerAffixation = await _verifyPaymentDetailService.CheckdealerAffixation(DealerAppointment.DealerAffixationCenterId);
                if (checkdealerAffixation.Count > 0)
                {

                    foreach (var item in checkdealerAffixation)
                    {
                        modelResult.RTOLocationID = item.RTOLocationID.ToString();
                        modelResult.RTOName = item.RTOLocationName.ToString();
                        modelResult.DealerId = item.DealerID.ToString();
                    }
                }
                var checkoem = await _verifyPaymentDetailService.CheckOem(vehicledetails.OemId);
                if (checkoem.Count > 0)
                {
                    foreach (var item in checkdealerAffixation)
                    {

                        modelResult.OEMID = item.OemID.ToString();
                        modelResult.ManufacturerName = item.OemName.ToString();
                    }
                }
            }
            catch (Exception ev)
            {
                modelResult.Message = ev.Message;
            }
            try
            {
                if (vehicledetails.StateIdBackup.ToString() == "27")
                {
                    var checkOemRate = await _verifyPaymentDetailService.CheckOemRateFromTax(orderType, userdetails.VehicleType, vehicledetails.StateIdBackup);
                    if (checkOemRate.Count > 0)
                    {
                        foreach (var item in checkOemRate)
                        {
                            modelResult.FrontPlateSize = item.FrontPlateSize;
                            modelResult.RearPlateSize = item.RearPlateSize;
                            modelResult.BasicAmount = item.GstBasicAmount;
                            modelResult.FitmentCharge = item.FittmentCharges;
                            modelResult.ConvenienceFee = item.ConvenienceCharges;
                            modelResult.HomeDeliveryCharge = "0";
                            modelResult.TotalAmount = item.GrossTotal;
                            modelResult.GSTAmount = item.GSTAmount;
                        }
                        if (vehicledetails.StateIdBackup.toString() == vehicledetails.StateId)
                        {
                            foreach (var item in checkOemRate)
                            {
                                modelResult.CGSTAmount = item.CGSTAmount;
                                modelResult.SGSTAmount = item.SGSTAmount;
                            }
                        }
                        else
                        {
                            foreach (var item in checkOemRate)
                            {
                                modelResult.IGSTAmount = (Convert.ToDecimal(item.CGSTAmount) + Convert.ToDecimal(item.SGSTAmount)).toString();

                            }
                        }
                        foreach (var item in checkOemRate)
                        {
                            modelResult.IGSTAmount = item.NetAmount.toString();
                            modelResult.ChkSuperTag = true;

                        }
                        modelResult.CustomerName = userdetails.CustomerName;
                        modelResult.CustomerMobileNo = userdetails.CustomerMobile;
                        modelResult.CustomerEmailID = userdetails.CustomerEmail;
                        modelResult.CustomerAddress1 = userdetails.CustomerBillingAddress;
                        modelResult.CustomerState = vehicledetails.StateName;



                    }

                }
                else
                {
                    var result = await _verifyPaymentDetailService.CheckOemRateFromOrderRate(vehicledetails.OemId, orderType, vehicledetails.VehicleClass, userdetails.VehicleType, userdetails.VehicleCategoryId, vehicledetails.FuelType, DealerAppointment.DeliveryPoint, vehicledetails.StateId, vehicledetails.StateName);
                    if (DealerAppointment.DeliveryPoint == "Dealer")
                    {
                        foreach (var item in result)
                        {
                            modelResult.GSTAmount = item.GstBasic_Amt.ToString();
                            modelResult.FittmentCharges = item.FittmentCharges.ToString();
                            modelResult.BMHConvenienceCharges = item.BMHConvenienceCharges.ToString();
                            modelResult.BMHHomeCharges = item.BMHHomeCharges.ToString();
                        }
                        if (modelResult.GSTAmount.ToString() == "0.00" || modelResult.GSTAmount.ToString() == null || modelResult.FittmentCharges.ToString() == "0.00" || modelResult.FittmentCharges.ToString() == null || modelResult.BMHConvenienceCharges == "0.00" || modelResult.BMHConvenienceCharges == null)
                        {
                            modelResult.Message = "Rates not matched. For any query kindly mail to online@bookmyhsrp.com";
                            return modelResult;
                        }
                    }
                    else
                    {
                        if (modelResult.GSTAmount.ToString() == "0.00" || modelResult.GSTAmount.ToString() == null || modelResult.FittmentCharges.ToString() == "0.00" || modelResult.FittmentCharges.ToString() == null || modelResult.BMHConvenienceCharges == "0.00" || modelResult.BMHConvenienceCharges == null || modelResult.BMHConvenienceCharges == "0.00" || modelResult.BMHConvenienceCharges == null)
                        {
                            modelResult.Message = "Rates not matched. For any query kindly mail to online@bookmyhsrp.com";
                            return modelResult;
                        }
                    }
                    if (modelResult.ChkFastTag)
                    {
                        var CheckSuperTagRate = await _verifyPaymentDetailService.CheckSuperTagRate();
                        if (CheckSuperTagRate.Count > 0)
                        {
                            foreach (var item in CheckSuperTagRate)
                            {
                                modelResult.Status = item.ActiveStatus;
                            }
                            if (modelResult.Status == "Y")
                            {
                                foreach (var item in CheckSuperTagRate)
                                {
                                    modelResult.TotalAmountST = Convert.ToDecimal(item.TotalAmountWithGST);
                                    modelResult.Basic_AmtST = Convert.ToDecimal(item.BasicItemPrice);
                                    modelResult.CGSTAmountST = Convert.ToDecimal(item.CGSTAmount);
                                    modelResult.IGSTAmountST = Convert.ToDecimal(item.IGSTAmount);
                                    modelResult.SGSTAmountST = Convert.ToDecimal(item.SGSTAmountST);
                                }
                                modelResult.CustomerName = userdetails.CustomerName;
                                modelResult.CustomerMobileNo = userdetails.CustomerMobile;
                                modelResult.CustomerEmailID = userdetails.CustomerEmail;
                                modelResult.CustomerAddress1 = userdetails.CustomerBillingAddress;
                                modelResult.CustomerState = vehicledetails.StateName;



                            }

                            else
                            {
                                modelResult.Message = "Not Allowed to buy Park+FasTag.!!";
                                return modelResult;
                            }
                        }
                    }

                    if (modelResult.ChkFrame && DealerAppointment.DeliveryPoint == "Home")
                    {

                    }
                    if (result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            modelResult.Status = item.status.ToString();
                            modelResult.Message = item.message.ToString();
                        }
                        if (modelResult.Status == "1")
                        {
                            foreach (var item in result)
                            {
                                modelResult.FrontPlateSize = item.FrontPlateSize.ToString();
                                modelResult.RearPlateSize = item.RearPlateSize.ToString();
                                modelResult.BasicAmount = item.GstBasic_Amt.ToString();
                                modelResult.FitmentCharge = item.FittmentCharges.ToString();
                                modelResult.ConvenienceFee = item.BMHConvenienceCharges.ToString();
                                modelResult.HomeDeliveryCharge = "0";
                                modelResult.TotalAmount = item.GrossTotal.ToString();
                                modelResult.GSTAmount = item.GSTAmount.ToString();
                                modelResult.IGSTAmountST = item.IGSTAmount;
                                modelResult.SGSTAmountST = item.SGSTAmount;
                                modelResult.CGSTAmountST = item.CGSTAmount;
                                modelResult.NetAmount =item.TotalAmount.ToString();
                            }
                            if (modelResult.ChkFastTag)
                            {
                                modelResult.FinalAmount = modelResult.NetAmount + modelResult.TotalAmountST;
                                modelResult.GSTAmount = string.Format("{0:0.00}", modelResult.GSTAmount);
                                modelResult.IGSTAmount = string.Format("{0:0.00}", (modelResult.IGSTAmount + modelResult.IGSTAmountST).ToString());
                                modelResult.CGSTAmount = string.Format("{0:0.00}", (Convert.ToDecimal(modelResult.CGSTAmount) + modelResult.CGSTAmountST).ToString());
                                modelResult.SGSTAmount = string.Format("{0:0.00}", (Convert.ToDecimal(modelResult.SGSTAmount) + modelResult.SGSTAmountST).ToString());
                                modelResult.GrandTotal = string.Format("{0:0.00}", string.Format("{0:0.00}", modelResult.FinalAmount));
                                modelResult.BasicAmount = string.Format("{0:0.00}", (Convert.ToDecimal(modelResult.TotalAmount) + modelResult.Basic_AmtST).ToString());
                                modelResult.SuperTagBasicAmount = string.Format("{0:0.00}", modelResult.Basic_AmtST);
                            }
                            if (modelResult.ChkFrame)
                            {
                                modelResult.FinalAmount = modelResult.NetAmount + modelResult.TotalAmountST;
                                modelResult.GSTAmount = string.Format("{0:0.00}", modelResult.GSTAmount);
                                modelResult.IGSTAmount = string.Format("{0:0.00}", (modelResult.IGSTAmount + modelResult.IGSTAmountST).ToString());
                                modelResult.CGSTAmount = string.Format("{0:0.00}", (Convert.ToDecimal(modelResult.CGSTAmount) + modelResult.CGSTAmountST).ToString());
                                modelResult.SGSTAmount = string.Format("{0:0.00}", (Convert.ToDecimal(modelResult.SGSTAmount) + modelResult.SGSTAmountST).ToString());
                                modelResult.GrandTotal = string.Format("{0:0.00}", string.Format("{0:0.00}", modelResult.FinalAmount));
                                modelResult.BasicAmount = string.Format("{0:0.00}", (Convert.ToDecimal(modelResult.TotalAmount) + modelResult.Basic_AmtST).ToString());
                                modelResult.FrameBasicAmount = string.Format("{0:0.00}", modelResult.Basic_AmtST);
                            }
                        }
                        else
                        {
                            modelResult.Message = modelResult.Message;
                        }
                    }
                    else
                    {
                        modelResult.Message = "Rate not found.!!";
                        return modelResult;
                    }



                }

            }
            catch (Exception ev)
            {
                modelResult.Message = ev.Message;
            }
            try
            {
                 modelResult.orderNo = "BMHSRP" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + GetRandomNumber();
                 modelResult.isSuperTag = modelResult.ChkFrame == true ? "Y" : "N";
                if(DealerAppointment.DeliveryPoint=="Dealer")
                {
                    modelResult.isFrame = "N";
                }
                var paymentInitiated = await _verifyPaymentDetailService.PaymentInitiated(DealerAppointment.DealerAffixationCenterId, modelResult.orderNo, orderType, modelResult.SlotId, modelResult.SlotTime, modelResult.SlotBookingDate, modelResult.HSRPStateID, modelResult.RTOLocationID, modelResult.RTOName, modelResult.OwnerName, modelResult.OwnerFatherName, modelResult.Address1, modelResult.State, modelResult.City, modelResult.Pin, modelResult.MobileNo, modelResult.LandlineNo, modelResult.EmailID, modelResult.VehicleClass, modelResult.VehicleType, modelResult.ManufacturerName, modelResult.ChassisNo, modelResult.EngineNo, modelResult.ManufacturingYear, modelResult.VehicleRegNo, modelResult.FrontPlateSize, modelResult.RearPlateSize, modelResult.TotalAmount, modelResult.NetAmount, modelResult.BookingType, modelResult.BookingClassType, modelResult.FuelType, modelResult.DealerId, modelResult.OEMID, modelResult.BookedFrom, modelResult.AppointmentType, modelResult.BasicAmount, modelResult.FitmentCharge, modelResult.ConvenienceFee, modelResult.HomeDeliveryCharge, modelResult.GSTAmount, modelResult.CustomerGSTNo, modelResult.VehicleRCImage, modelResult.BharatStage, modelResult.ShippingAddress1, modelResult.ShippingAddress2, modelResult.ShippingCity, modelResult.ShippingState, modelResult.ShippingPinCode, modelResult.ShippingLandMark, modelResult.IGSTAmount, modelResult.CGSTAmount, modelResult.SGSTAmount, modelResult.FrontLaserCode, modelResult.RearLaserCode, modelResult.NonHomologVehicle, modelResult.isSuperTag, modelResult.isFrame, modelResult.FrontHSRPFileName, modelResult.RearHSRPFileName, modelResult.FileFIR, modelResult.Firno, modelResult.FirDate, modelResult.Firinfo, modelResult.PoliceStation, modelResult.ReplacementReason);
        if(paymentInitiated.Count>0)
                {
                    foreach(var item in paymentInitiated)
                    {
                        modelResult.Status = item.status.ToString();
                        modelResult.orderNo = item.OrderNo.ToString();
                    }
                    //if (Session["QueryStringValue"] != null)
                    //{
                    //    Value = Session["QueryStringValue"] as string[];
                    //    if (Value != null)
                    //    {
                    //        if (Value.Length == 6)
                    //        {
                    //            val1 = Value[0].ToString();
                    //            val2 = Value[1].ToString();
                    //            val3 = Value[2].ToString();
                    //            val4 = Value[3].ToString();
                    //            val5 = Value[4].ToString();
                    //            val6 = Value[5].ToString();

                    //            string url = Session["QueryStringURL"] == null ? "" : Session["QueryStringURL"].ToString();
                    //            BMHSRPv2.Models.Utils.ExecNonQuery("sp_CaptureQueryStringData '" + val1.Replace("'", "") + "','" + url.Replace("'", "") + "','" + OrderNo + "','" + val2.Replace("'", "") + "','" + val3.Replace("'", "") + "','" + val4.Replace("'", "") + "','" + val5.Replace("'", "") + "','" + val6.Replace("'", "") + "'", CnnString);
                    //        }

                    //    }
                    //}
                    if(modelResult.ChkFastTag)
                    {
                        var insertSuperTagOrder = await _verifyPaymentDetailService.InsertSuperTagOrder(modelResult.orderNo, userdetails.CustomerName, userdetails.CustomerMobile, userdetails.CustomerEmail, userdetails.CustomerBillingAddress, vehicledetails.StateName, modelResult.City, modelResult.Pin);
                    
                    }
                    if(modelResult.ChkFrame && DealerAppointment.DeliveryPoint=="Home")
                    {

                    }
                    if(modelResult.Status=="1")
                    {
                        modelResult.NetAmount = (Convert.ToDecimal(modelResult.NetAmount) + modelResult.TotalAmountST + modelResult.TotalAmountFrm).ToString();
                       data =  await RazorPay(modelResult.orderNo, modelResult.NetAmount,modelResult.OwnerName, modelResult.Address1,modelResult.City, modelResult.State, modelResult.Pin, modelResult.MobileNo, modelResult.EmailID, (modelResult.SlotBookingDate + " " + modelResult.SlotTime), DealerAppointment.DealerAffixationCenterId, modelResult.DealerAffixationAddress, modelResult.VehicleRegNo, modelResult.SlotId, DealerAppointment.DeliveryPoint ,  ip);
                        
                    }
                }
                else
                   {
                    foreach(var item in paymentInitiated)
                    {
                        modelResult.Message = item.message.ToString();
                    }
                }
            }
            catch (Exception ev)
            {
                modelResult.Message= ev.Message;
            }


            modelResult.Key = _key.ToString();
            modelResult.Order_No = Order_No;
            return modelResult;
            }
            
    public string GetRandomNumber()
    {
        Random r = new Random();
        var x = r.Next(0, 9);
        return x.ToString("0");
    }
        private async Task<dynamic> RazorPay(string orderno, string GrandTotal, string OwnerName, string Address,
            string CityName, string StateName, string PinCode,
            string MobileNo, string Emailid, string AppointmentDateTime, string DealerAffaxtionCenterID,
            string DealerAffaxtionAddress, string Regno, string TimeSlotID ,string DeliveryPoint , string _ip)
        {
            var modelResult = new PaymentDetails();
            
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            decimal payableAmount = Convert.ToDecimal(GrandTotal) * 100;

            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", payableAmount); // this amount should be same as transaction amount
            input.Add("currency", "INR");
            input.Add("receipt", orderno);
            input.Add("payment_capture", 1);
            //input.Add("name", "Dheerendra Singh");
            //input.Add("prefill","{ 'email':'dheerendra786@gmail.com','contact':'8882359687'}");

            //string key = "rzp_test_A8SlD8Ar6NexSY";  //rzp_test_Uy1r8Av2FjQdBP
            //string secret = "L657FHU7f3APTshth2yDhjgw";//iaRucYRkXy0nW2IvRrqPrMwj
            string key = _key.ToString();
            
            string secret = _secret.ToString();

            RazorpayClient client = new RazorpayClient(key, secret);

            try
            {
                Razorpay.Api.Order order = client.Order.Create(input);
                Order_No = order["id"].ToString();

                try
                {

                    #region Nlog
                    Logger logger = LogManager.GetLogger("databaseLogger");
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        string ip = string.Empty;

                        ip = _ip;

                        logger.WithProperty("RazorpayOrderid", Order_No)
                            .WithProperty("Regno", Regno)
                            .WithProperty("OrderNo", orderno)
                            .WithProperty("Email", Emailid)
                             .WithProperty("Remoteip", ip)
                              .WithProperty("OrderStatus", "Initiated")
                                   .WithProperty("Status", "")
                        .Info(str.ToString());






                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {
                        // get a Logger object and log exception here using NLog. 
                        // this will use the "databaseLogger" logger from our NLog.config file


                        //logger.Debug("Test");

                        // add custom message and pass in the exception
                        // logger.Error(ex,"message","my info message with {Property1} ,{Property2}", "Value1","Value2");

                    }

                    #endregion

                    var RazorPayOrderIDUpdate = await _verifyPaymentDetailService.RazorPayOrderIdUpdate(Order_No, orderno);
                    modelResult.hdnMyOrderID = orderno;
                    modelResult.hdnGatewayOrderID = Order_No;
                }
#pragma warning disable CS0168 // The variable 'ev' is declared but never used
                catch (Exception ev)
#pragma warning restore CS0168 // The variable 'ev' is declared but never used
                {

                }
            }
            catch (Exception ex)
            {
                Order_No = RandomString(10);
                Console.WriteLine(ex.Message);
            }


            string Host = _Host.ToString();

            //string ccavResponseHandler = @"http://localhost:55098//plate/PaymentReceipt.aspx";
            string ccavResponseHandler = Host + "plate/PaymentReceipt.aspx";

            ccavResponseHandler = ccavResponseHandler + "?" + DeliveryPoint.ToString();

            StringBuilder sbTable = new StringBuilder();
            sbTable.Clear();
            sbTable.Append("<form id='customerData' name='customerData' action='" + ccavResponseHandler + "' method='post'>");
            sbTable.Append("<script");
            sbTable.Append("src='https://checkout.razorpay.com/v1/checkout.js' ");
            sbTable.Append("data-key='" + key + "' ");
            sbTable.Append("data-amount='0' ");
            sbTable.Append("data-timeout=600 ");//in sec.
            sbTable.Append("data-name='Razorpay' ");
            sbTable.Append("data-description='BookMyHSRP' ");
            sbTable.Append("data-order_id='" + Order_No + "' ");
            sbTable.Append("<img src='https://razorpay.com/favicon.png' />");
            sbTable.Append("data-prefill.name='" + OwnerName + "' ");
            sbTable.Append("data-prefill.email='" + Emailid + "' ");
            sbTable.Append("data-prefill.contact='" + MobileNo + "' ");
            sbTable.Append("data-theme.color='#F37254' ");
            sbTable.Append("data-modal.confirm_close=true ");
            sbTable.Append("data-modal.escape=false ");
            //sbTable.Append($"data-modal.ondismiss=function(){{payment_checkoutClosed('{orderno}','{Order_No}');}} ");
            sbTable.Append("></script>");

            sbTable.Append("<input type='hidden' value='Hidden Element' name='hidden'>");
            sbTable.Append("<input type='hidden' value='" + orderno + "' name='generated_order_id'>");
            sbTable.Append("</form>");
            sbTable.Append("<script language='javascript'>");
            sbTable.Append("ValidatePayForm();");
            sbTable.Append("</script>");
            modelResult.Lateral = sbTable.ToString();
            modelResult.Order_No = Order_No;
            modelResult.orderNo= orderno;
            return modelResult.Lateral;

        }
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
        public async Task<dynamic> CheckVehicleForDFDRDB(string VehicleRegNo, string ChassisNo,dynamic EngineNo)
        {
            var modelResult = new PaymentDetails();
            
            modelResult.Checker = false;
            var result =await _verifyPaymentDetailService.Check(VehicleRegNo, ChassisNo, EngineNo);
            if(result.Count>0)
            {
                string orderNo = string.Empty;
                foreach(var data in result)
                {
                    orderNo = result.Orderno;

                }
                var getBetweenData = await _verifyPaymentDetailService.GetBetweenData(orderNo);
                if(getBetweenData.Count>0)
                {
                    var ReBookingAllow = "";
                    foreach (var data in getBetweenData)
                    {
                        ReBookingAllow = data.ReBookingAllow;
                    }
                    if(ReBookingAllow=="Y")
                    {
                        modelResult.Checker = true;
                    }
                }
            }
            else
            {
                var getDataBetweenElse =await _verifyPaymentDetailService.GetDataBetweenElse(VehicleRegNo, ChassisNo);
                if(getDataBetweenElse.Count>0)
                {
                    var ReBookingAllow = "";
                    foreach (var data in getDataBetweenElse)
                    {
                        ReBookingAllow = data.ReBookingAllow;
                    }
                    if (ReBookingAllow == "Y")
                    {
                        modelResult.Checker = true;
                    }
                }
            }
            return modelResult;
         }
    }
}