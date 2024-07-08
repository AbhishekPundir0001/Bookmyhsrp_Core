using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.VerifyPaymentDetail.Services
{
    public interface IVerifyPaymentDetailService
    {
        Task<dynamic> CheckSuperTagRate();
        Task<dynamic> CheckFrameRate(string _VehicleTyeForFrame,string orderType);
        Task<dynamic> CheckOemRate(string orderType,string  StateId,string vehicletype);
        Task<dynamic> CheckOemRateQuery(dynamic vehicledetails, dynamic userDetails,dynamic DealerAppointment,string orderType);
        Task<dynamic> GetBookingId(dynamic vehicledetails, dynamic userDetails,dynamic DealerAppointment,string realOrderType);
        Task<dynamic> Check(string VehicleRegNo, string ChassisNo, string EngineNo);
        Task<dynamic> GetBetweenData(string orderNo);
        Task<dynamic> GetDataBetweenElse(string VehicleRegNo, string ChassisNo);
        Task<dynamic> PaymentConfirmation(string order_status,string failure_message,string payment_gateway_type);
        Task<dynamic> AppointmentBlockDate(string SelectedSlotDate, string DealerAffixationCenterId, string DeliveryPoint);
        Task<dynamic> GetOemId(string DealerAffixationCenterId);
        Task<dynamic> CheckdealerAffixation(string DealerAffixationCenterId);
        Task<dynamic> CheckOem(string OemId);
        Task<dynamic> CheckOemRateFromTax(string orderType, string VehicleType, string StateIdBackup);
        Task<dynamic> CheckOemRateFromOrderRate(string OemId,string  orderType,string VehicleClass,string VehicleType,string VehicleCategoryId,string Fuel,string DeliveryPoint, string StateId, string StateName);
        Task<dynamic> PaymentInitiated(string DealerAffixationCenterId, string orderNo, string orderType, string SlotId, string SlotTime, string SlotBookingDate, string HSRPStateID, string RTOLocationID, string RTOName, string OwnerName, string OwnerFatherName, string Address1, string State, string City, string Pin, string MobileNo, string LandlineNo, string EmailID, string VehicleClass, string VehicleType, string ManufacturerName, string ChassisNo, string EngineNo, string ManufacturingYear, string VehicleRegNo, string FrontPlateSize, string RearPlateSize, string TotalAmount, string NetAmount, string BookingType, string BookingClassType, string FuelType, string DealerId, string OEMID, string BookedFrom, string AppointmentType, string BasicAmount, string FitmentCharge, string ConvenienceFee, string HomeDeliveryCharge, string GSTAmount, string CustomerGSTNo, string VehicleRCImage, string BharatStage, string ShippingAddress1, string ShippingAddress2, string ShippingCity, string ShippingState, string ShippingPinCode, string ShippingLandMark, string IGSTAmount, string CGSTAmount, string SGSTAmount, string FrontLaserCode, string RearLaserCode, string NonHomologVehicle, string isSuperTag, string isFrame, string FrontHSRPFileName, string RearHSRPFileName, string FileFIR, string Firno, string FirDate, string Firinfo, string PoliceStation, string ReplacementReason);
        Task<dynamic> InsertSuperTagOrder(string orderNo,string CustomerName, string CustomerMobile, string CustomerEmail,string CustomerBillingAddress,string StateName, string City, string Pin);
        Task<dynamic> RazorPayOrderIdUpdate(string Order_No,string  orderno);
        Task<dynamic> PaymentInitiatedSticker(string DealerAffixationCenterId, string orderNo, string orderType, string SlotId, string SlotTime, string SlotBookingDate, string HSRPStateID, string RTOLocationID, string RTOName, string OwnerName, string OwnerFatherName, string Address1, string State, string City, string Pin, string MobileNo, string LandlineNo, string EmailID, string VehicleClass, string VehicleType, string ManufacturerName, string ChassisNo, string EngineNo, string ManufacturingYear, string VehicleRegNo, string FrontPlateSize, string RearPlateSize, string TotalAmount, string NetAmount, string BookingType, string BookingClassType, string FuelType, string DealerId, string OEMID, string BookedFrom, string AppointmentType, string BasicAmount, string FitmentCharge, string ConvenienceFee, string HomeDeliveryCharge, string GSTAmount, string CustomerGSTNo, string VehicleRCImage, string BharatStage, string ShippingAddress1, string ShippingAddress2, string ShippingCity, string ShippingState, string ShippingPinCode, string ShippingLandMark, string IGSTAmount, string CGSTAmount, string SGSTAmount, string FrontLaserCode, string RearLaserCode, string NonHomologVehicle, string FrontLaserFileName, string RearLaserFileName, string File3, string File4, string LaserFileValidationFlag, string isSuperTag, string PlateSticker);


    }
}
