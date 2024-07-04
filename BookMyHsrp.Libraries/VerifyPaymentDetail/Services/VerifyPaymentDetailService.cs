using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpState.Models;
using BookMyHsrp.Libraries.HsrpState.Queries;
using BookMyHsrp.Libraries.VerifyPaymentDetail.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.VerifyPaymentDetail.Services
{
    public class VerifyPaymentDetailService: IVerifyPaymentDetailService
    {
        private readonly DapperRepository _databaseHelper;
        private readonly string _primaryDatabaseHO;


        public VerifyPaymentDetailService(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _primaryDatabaseHO = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelper = new DapperRepository(_primaryDatabaseHO);

        }
        public async Task<dynamic> CheckSuperTagRate()
        {
            var result =await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.CheckSuperTagRate);
            return result;
        }
        public async Task<dynamic> CheckFrameRate(string _VehicleTyeForFrame,string orderType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderType", orderType);
            parameters.Add("@VehicleType", _VehicleTyeForFrame);
            var result =await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.CheckFrameRate, parameters);
            return result;
        }
        public async Task<dynamic> CheckOemRate(string orderType, string StateId, string vehicletype)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderType", orderType);
            parameters.Add("@VehicleType", vehicletype);
            parameters.Add("@StateId", StateId);
            var result =await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.CheckOemRate, parameters);
            return result;
        }
        public async Task<dynamic> CheckOemRateQuery(dynamic  vehicleDetails, dynamic userDetails,dynamic DealerAppointment, string orderType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OemId", vehicleDetails.OemId);
            parameters.Add("@OrderType", orderType);
            parameters.Add("@VehicleClass", vehicleDetails.VehicleClass);
            parameters.Add("@VehicleType", userDetails.VehicleType);
            parameters.Add("@VehicleCategoryId", userDetails.VehicleCategoryId);
            parameters.Add("@FuelType", userDetails.FuelType);
            parameters.Add("@DeliveryPoint", DealerAppointment.DeliveryPoint);
            parameters.Add("@StateId", vehicleDetails.StateId);
            parameters.Add("@StateName", vehicleDetails.StateName);
            var result =await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.CheckOemRateQuery, parameters);
            return result;
        }
        public async Task<dynamic> GetBookingId(dynamic vehicleDetails, dynamic userDetails, dynamic DealerAppointment, string realOrderType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Engineno", vehicleDetails.EngineNo);
            parameters.Add("@OrderType", realOrderType.ToString());
            parameters.Add("@ChassisNo", vehicleDetails.ChassisNo);
            parameters.Add("@RegistrationNo", vehicleDetails.VehicleRegNo);
            if(vehicleDetails.PlateSticker == "Sticker")
            {
                var result1 = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.GetBookingHistoryIdSticker, parameters);
                return result1;
            }
            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.GetBookingHistoryId, parameters);
            return result;
        }
        public async Task<dynamic> Check(string VehicleRegNo, string ChassisNo, string EngineNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Engineno", EngineNo);
            parameters.Add("@ChassisNo", ChassisNo);
            parameters.Add("@RegistrationNo", VehicleRegNo);
            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.Check, parameters);
            return result;
        }
        public async Task<dynamic> GetBetweenData(string orderNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", orderNo);
           
            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.GetBetweenData, parameters);
            return result;
        }
        public async Task<dynamic> GetDataBetweenElse(string VehicleRegNo, string ChassisNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RegistrationNo", VehicleRegNo);
            parameters.Add("@ChassisNo", ChassisNo);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.GetDataBetweenElse, parameters);
            return result;
        }
        public async Task<dynamic> PaymentConfirmation(string order_status, string failure_message, string payment_gateway_type)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderStatus", order_status);
            parameters.Add("@FaliureMessage", failure_message);
            parameters.Add("@PaymentGateWay", payment_gateway_type);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.PaymentConfirmation, parameters);
            return result;
        }
        public async Task<dynamic> AppointmentBlockDate(string SelectedSlotDate, string DealerAffixationCenterId, string DeliveryPoint)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SlotDate", SelectedSlotDate);
            parameters.Add("@DealerAffixationId", DealerAffixationCenterId);
            parameters.Add("@DeliveryPoint", DeliveryPoint);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.AppointmentBlockDate, parameters);
            return result;
        }
        public async Task<dynamic> GetOemId(string DealerAffixationCenterId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DealerAffixationId", DealerAffixationCenterId);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.GetOemId, parameters);
            return result;
        }
        public async Task<dynamic> CheckdealerAffixation(string DealerAffixationCenterId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DealerAffixationId", DealerAffixationCenterId);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.CheckdealerAffixation, parameters);
            return result;
        }
        public async Task<dynamic> CheckOem(string OemId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OemId", OemId);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.CheckOem, parameters);
            return result;
        }
        public async Task<dynamic> CheckOemRateFromTax(string orderType, string VehicleType, string StateIdBackup)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderType", orderType);
            parameters.Add("@VehicleType", VehicleType);
            parameters.Add("@StateIdBackup", StateIdBackup);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.CheckOemRateFromTax, parameters);
            return result;
        }
        public async Task<dynamic> CheckOemRateFromOrderRate(string OemId, string orderType, string VehicleClass, string VehicleType, string VehicleCategoryId, string Fuel, string DeliveryPoint, string StateId, string StateName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OemId", OemId);
            parameters.Add("@OrderType", orderType);
            parameters.Add("@VehicleClass", VehicleClass);
            parameters.Add("@VehicleType", VehicleType);
            parameters.Add("@VehicleTypeId", VehicleCategoryId);
            parameters.Add("@Fuel", Fuel);
            parameters.Add("@DeliveryPoint", DeliveryPoint);
            parameters.Add("@StateId", StateId);
            parameters.Add("@StateName", StateName);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.CheckOemRateFromOrderRate, parameters);
            return result;
        }
        public async Task<dynamic> PaymentInitiated(string DealerAffixationCenterId, string orderNo, string orderType, string SlotId, string SlotTime, string SlotBookingDate, string HSRPStateID, string RTOLocationID, string RTOName, string OwnerName, string OwnerFatherName, string Address1, string State, string City, string Pin, string MobileNo, string LandlineNo, string EmailID, string VehicleClass, string VehicleType, string ManufacturerName, string ChassisNo, string EngineNo, string ManufacturingYear, string VehicleRegNo, string FrontPlateSize, string RearPlateSize, string TotalAmount, string NetAmount, string BookingType, string BookingClassType, string FuelType, string DealerId, string OEMID, string BookedFrom, string AppointmentType, string BasicAmount, string FitmentCharge, string ConvenienceFee, string HomeDeliveryCharge, string GSTAmount, string CustomerGSTNo, string VehicleRCImage, string BharatStage, string ShippingAddress1, string ShippingAddress2, string ShippingCity, string ShippingState, string ShippingPinCode, string ShippingLandMark, string IGSTAmount, string CGSTAmount, string SGSTAmount, string PlateSticker, string FrontLaserCode, string RearLaserCode, string NonHomologVehicle, string isSuperTag, string isFrame, string FrontHSRPFileName, string RearHSRPFileName, string FileFIR, string Firno, string FirDate, string Firinfo, string PoliceStation, string ReplacementReason)
         {
            var parameters = new DynamicParameters();
            parameters.Add("@DealerAffixationCenterId", DealerAffixationCenterId);
            parameters.Add("@orderNo", orderNo);
            parameters.Add("@orderType", orderType);
            parameters.Add("@SlotId", SlotId);
            parameters.Add("@SlotTime", SlotTime);
            parameters.Add("@SlotBookingDate", SlotBookingDate);
            parameters.Add("@HSRPStateID", HSRPStateID);
            parameters.Add("@RTOLocationID", RTOLocationID);
            parameters.Add("@RTOName", RTOName);
            parameters.Add("@OwnerName", OwnerName);
            parameters.Add("@OwnerFatherName", OwnerFatherName);
            parameters.Add("@Address1", Address1);
            parameters.Add("@State", State);
            parameters.Add("@City", City);
            parameters.Add("@Pin", Pin);
            parameters.Add("@MobileNo", MobileNo);
            parameters.Add("@LandlineNo", LandlineNo);
            parameters.Add("@EmailID", EmailID);
            parameters.Add("@VehicleClass", VehicleClass);
            parameters.Add("@VehicleType", VehicleType);
            parameters.Add("@ManufacturerName", ManufacturerName);
            parameters.Add("@ChassisNo", ChassisNo);
            parameters.Add("@EngineNo", EngineNo);
            parameters.Add("@ManufacturingYear", ManufacturingYear);
            parameters.Add("@VehicleRegNo", VehicleRegNo);
            parameters.Add("@FrontPlateSize", FrontPlateSize);
            parameters.Add("@RearPlateSize", RearPlateSize);
            parameters.Add("@TotalAmount", TotalAmount);
            parameters.Add("@NetAmount", NetAmount);
            parameters.Add("@BookingType", BookingType);
            parameters.Add("@BookingClassType", BookingClassType);
            parameters.Add("@FuelType", FuelType);
            parameters.Add("@DealerId", DealerId);
            parameters.Add("@OEMID", OEMID);
            parameters.Add("@BookedFrom", BookedFrom);
            parameters.Add("@AppointmentType", AppointmentType);
            parameters.Add("@BasicAmount", BasicAmount);
            parameters.Add("@FitmentCharge", FitmentCharge);
            parameters.Add("@ConvenienceFee", ConvenienceFee);
            parameters.Add("@HomeDeliveryCharge", HomeDeliveryCharge);
            parameters.Add("@GSTAmount", GSTAmount);
            parameters.Add("@CustomerGSTNo", CustomerGSTNo);
            parameters.Add("@VehicleRCImage", VehicleRCImage);
            parameters.Add("@BharatStage", BharatStage);
            parameters.Add("@ShippingAddress1", ShippingAddress1);
            parameters.Add("@ShippingAddress2", ShippingAddress2);
            parameters.Add("@ShippingCity", ShippingCity);
            parameters.Add("@ShippingState", ShippingState);
            parameters.Add("@ShippingPinCode", ShippingPinCode);
            parameters.Add("@ShippingLandMark", ShippingLandMark);
            parameters.Add("@IGSTAmount", IGSTAmount);
            parameters.Add("@CGSTAmount", CGSTAmount);
            parameters.Add("@SGSTAmount", SGSTAmount);

            parameters.Add("@PlateSticker", PlateSticker);
            parameters.Add("@FrontLaserCode", FrontLaserCode);
            parameters.Add("@RearLaserCode", RearLaserCode);
            parameters.Add("@NonHomologVehicle", NonHomologVehicle);
            parameters.Add("@isSuperTag", isSuperTag);
            parameters.Add("@isFrame", isFrame);
            parameters.Add("@FrontHSRPFileName", FrontHSRPFileName);
            parameters.Add("@RearHSRPFileName", RearHSRPFileName);
            parameters.Add("@FileFIR", FileFIR);
            parameters.Add("@Firno", Firno);
            parameters.Add("@FirDate", FirDate);
            parameters.Add("@Firinfo", Firinfo);
            parameters.Add("@PoliceStation", PoliceStation);
            parameters.Add("@ReplacementReason", ReplacementReason);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.PaymentInitiated, parameters);
            return result;
        }

        public async Task<dynamic> PaymentInitiatedSticker(string DealerAffixationCenterId, string orderNo, string orderType, string SlotId, string SlotTime, string SlotBookingDate, string HSRPStateID, string RTOLocationID, string RTOName, string OwnerName, string OwnerFatherName, string Address1, string State, string City, string Pin, string MobileNo, string LandlineNo, string EmailID, string VehicleClass, string VehicleType, string ManufacturerName, string ChassisNo, string EngineNo, string ManufacturingYear, string VehicleRegNo, string FrontPlateSize, string RearPlateSize, string TotalAmount, string NetAmount, string BookingType, string BookingClassType, string FuelType, string DealerId, string OEMID, string BookedFrom, string AppointmentType, string BasicAmount, string FitmentCharge, string ConvenienceFee, string HomeDeliveryCharge, string GSTAmount, string CustomerGSTNo, string VehicleRCImage, string BharatStage, string ShippingAddress1, string ShippingAddress2, string ShippingCity, string ShippingState, string ShippingPinCode, string ShippingLandMark, string IGSTAmount, string CGSTAmount, string SGSTAmount, string FrontLaserCode, string RearLaserCode, string NonHomologVehicle, string FrontLaserFileName, string RearLaserFileName, string File3, string File4, string LaserFileValidationFlag, string isSuperTag, string PlateSticker)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DealerAffixationCenterId", DealerAffixationCenterId);
            parameters.Add("@orderNo", orderNo);
            parameters.Add("@orderType", orderType);
            parameters.Add("@SlotId", SlotId);
            parameters.Add("@SlotTime", SlotTime);
            parameters.Add("@SlotBookingDate", SlotBookingDate);
            parameters.Add("@HSRPStateID", HSRPStateID);
            parameters.Add("@RTOLocationID", RTOLocationID);
            parameters.Add("@RTOName", RTOName);
            parameters.Add("@OwnerName", OwnerName);
            parameters.Add("@OwnerFatherName", OwnerFatherName);
            parameters.Add("@Address1", Address1);
            parameters.Add("@State", State);
            parameters.Add("@City", City);
            parameters.Add("@Pin", Pin);
            parameters.Add("@MobileNo", MobileNo);
            parameters.Add("@LandlineNo", LandlineNo);
            parameters.Add("@EmailID", EmailID);
            parameters.Add("@VehicleClass", VehicleClass);
            parameters.Add("@VehicleType", VehicleType);
            parameters.Add("@ManufacturerName", ManufacturerName);
            parameters.Add("@ChassisNo", ChassisNo);
            parameters.Add("@EngineNo", EngineNo);
            parameters.Add("@ManufacturingYear", ManufacturingYear);
            parameters.Add("@VehicleRegNo", VehicleRegNo);
            parameters.Add("@FrontPlateSize", FrontPlateSize);
            parameters.Add("@RearPlateSize", RearPlateSize);
            parameters.Add("@TotalAmount", TotalAmount);
            parameters.Add("@NetAmount", NetAmount);
            parameters.Add("@BookingType", BookingType);
            parameters.Add("@BookingClassType", BookingClassType);
            parameters.Add("@FuelType", FuelType);
            parameters.Add("@DealerId", DealerId);
            parameters.Add("@OEMID", OEMID);
            parameters.Add("@BookedFrom", BookedFrom);
            parameters.Add("@AppointmentType", AppointmentType);
            parameters.Add("@BasicAmount", BasicAmount);
            parameters.Add("@FitmentCharge", FitmentCharge);
            parameters.Add("@ConvenienceFee", ConvenienceFee);
            parameters.Add("@HomeDeliveryCharge", HomeDeliveryCharge);
            parameters.Add("@GSTAmount", GSTAmount);
            parameters.Add("@CustomerGSTNo", CustomerGSTNo);
            parameters.Add("@VehicleRCImage", VehicleRCImage);
            parameters.Add("@BharatStage", BharatStage);
            parameters.Add("@ShippingAddress1", ShippingAddress1);
            parameters.Add("@ShippingAddress2", ShippingAddress2);
            parameters.Add("@ShippingCity", ShippingCity);
            parameters.Add("@ShippingState", ShippingState);
            parameters.Add("@ShippingPinCode", ShippingPinCode);
            parameters.Add("@ShippingLandMark", ShippingLandMark);
            parameters.Add("@IGSTAmount", IGSTAmount);
            parameters.Add("@CGSTAmount", CGSTAmount);
            parameters.Add("@SGSTAmount", SGSTAmount);

            parameters.Add("@PlateSticker", PlateSticker);
            parameters.Add("@FrontLaserCode", FrontLaserCode);
            parameters.Add("@RearLaserCode", RearLaserCode);
            parameters.Add("@NonHomologVehicle", NonHomologVehicle);
            parameters.Add("@FrontLaserFileName", FrontLaserFileName);
            parameters.Add("@RearLaserFileName", RearLaserFileName);
            parameters.Add("@FileName1", File3);
            parameters.Add("@FileName2", File4);
            parameters.Add("@LaserFileValidation", LaserFileValidationFlag);
            parameters.Add("@supertag", isSuperTag);
            parameters.Add("@MRDCharges", "0.00");

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.PaymentInitiatedSticker, parameters);
            return result;
        }


        public async Task<dynamic> InsertSuperTagOrder(string orderNo, string CustomerName, string CustomerMobile, string CustomerEmail, string CustomerBillingAddress, string StateName, string City, string Pin)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@orderNo", orderNo);
            parameters.Add("@CustomerName", CustomerName);
            parameters.Add("@CustomerMobile", CustomerMobile);
            parameters.Add("@CustomerEmail", CustomerEmail);
            parameters.Add("@CustomerBillingAddress", CustomerBillingAddress);
            parameters.Add("@StateName", StateName);
            parameters.Add("@City", City);
            parameters.Add("@Pin", Pin);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.InsertSuperTagOrder, parameters);
            return result;
        }
        public async Task<dynamic> RazorPayOrderIdUpdate(string Order_No, string orderno)
        { 
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", orderno);
            parameters.Add("@Order_No", Order_No);

            var result = await _databaseHelper.QueryAsync<dynamic>(VerifyPaymentDetailsQueries.RazorPayOrderIdUpdate, parameters);
            return result;
        }
    }
}
