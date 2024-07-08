using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.VerifyPaymentDetail.Queries
{
    public class VerifyPaymentDetailsQueries
    {
        public static readonly string CheckSuperTagRate = "select ItemID,ItemName,ActiveStatus,RecordCreationDate,BasicItemPrice,CGSTAmount,IGSTAmount,SGSTAmount,TotalAmountWithGST from [HSRPOEM].[dbo].BookMyHSRPItemMaster Where ItemID=1";
        public static readonly string CheckFrameRate = "select ItemID,ActiveStatus,BasicItemPrice,CGSTAmount,IGSTAmount,SGSTAmount,TotalAmountWithGST from [HSRPOEM].[dbo].FrameItemMaster Where VehicleType=@VehicleType and OrderType=@OrderType";
        public static readonly string CheckOemRate = "exec usp_GetTaxRate @OrderType , @VehicleType,@StateId";
        public static readonly string CheckOemRateQuery = "CheckOrdersRates @OemId,@OrderType,@VehicleClass,@VehicleType,@VehiclecategoryId,@FuelType,@DeliveryPoint,@StateId ,@StateName";
        public static readonly string GetBookingHistoryId = "select BookingHistoryID from [BookMyHSRP].dbo.Appointment_BookingHist where VehicleRegNo = @RegistrationNo and right(trim(Chassisno),5) =  right(trim(@ChassisNo),5 )and right(trim(Engineno),5) = right(trim(@Engineno),5 ) and OrderStatus in ('Success','Shipped','Success-Test') and PlateSticker = 'plate' and OrderType = @OrderType";
        public static readonly string GetBookingHistoryIdSticker = "select BookingHistoryID from [BookMyHSRP].dbo.Appointment_BookingHist where VehicleRegNo = @RegistrationNo and right(trim(Chassisno),5) =  right(trim(@ChassisNo),5 )and right(trim(Engineno),5) = right(trim(@Engineno),5 ) and OrderStatus in ('Success','Shipped','Success-Test') and PlateSticker = 'sticker' and OrderType = @OrderType";
        public static readonly string CheckSticker = "select top 1 Orderno,BookingHistoryID from [BookMyHSRP].dbo.Appointment_BookingHist where VehicleRegNo = @RegistrationNo and right(trim(@Chassisno),5) = (@Chassisno.Substring((@Chassisno.Length - 5) and right(trim(Engineno),5) = @Engineno.Substring(@Engineno.Length - 5)  and  OrderStatus in ('Success') and PlateSticker='sticker' order by BookingHistoryID desc ";
        public static readonly string Check = "select top 1 Orderno,BookingHistoryID from [BookMyHSRP].dbo.Appointment_BookingHist where VehicleRegNo = @RegistrationNo and right(trim(@Chassisno),5) = (@Chassisno.Substring((@Chassisno.Length - 5) and right(trim(Engineno),5) = @Engineno.Substring(@Engineno.Length - 5)  and  OrderStatus in ('Success') and PlateSticker='plate' order by BookingHistoryID desc ";
        public static readonly string GetBetweenData = "select top 1 case when getdate() Between OrderClosedDate And DATEADD(DAY, 7, OrderClosedDate) then 'N' else 'Y' end ReBookingAllow,OrderClosedDate, Vehicleregno from hsrprecords WITH (NOLOCK) where Orderno =@OrderNo and  OrderClosedDate <>'' order by OrderClosedDate desc";
        public static readonly string GetDataBetweenElse = "select top 1 case when getdate() Between OrderClosedDate And DATEADD(DAY, 7, OrderClosedDate) then 'N' else 'Y' end ReBookingAllow,OrderClosedDate, Vehicleregno from hsrprecords WITH (NOLOCK) where Vehicleregno =@RegistrationNo and right(trim(Chassisno),5) = @ChassisNo and  OrderClosedDate<>'' order by OrderClosedDate desc";
        public static readonly string PaymentConfirmation = "exec PaymentConfirmation @MyOrderId,'','',@OrderStatus,@FaliureMessage,'','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','',@PaymentGateWay";
        public static readonly string AppointmentBlockDate = "exec CheckECAppointmentBlockedDates @SlotDate, @DealerAffixationId,@DeliveryPoint";
        public static readonly string GetOemId = "select oemid from [HSRPOEM].dbo.DealerAffixationCenter where DealerAffixationID=@DealerAffixationId";
        public static readonly string CheckdealerAffixation = "select a.OemID, c.Name OemName, a.DealerID, a.StateID, a.RTOLocationID,b.RTOLocationName from [HSRPOEM].dbo.DealerAffixationCenter a, [HSRPOEM].dbo.rtolocation b, [HSRPOEM].dbo.OEMMaster c  where a.rtolocationid = b.RTOLocationID and a.OemID = c.OEMID and DealerAffixationID = @DealerAffixationId ";
        public static readonly string CheckOem = "select oemid, name OemName from [HSRPOEM].dbo.OEMMaster where oemid = @OemId";
        public static readonly string CheckOemRateFromTax = "exec usp_GetTaxRate @OrderType,@VehicleType,@StateIdBackup ";
        public static readonly string CheckOemRateFromOrderRate = "CheckOrdersRates @OemId, @OrderType,@VehicleClass, @VehicleType,@VehicleTypeId,@Fuel,@DeliveryPoint,@StateId,@StateName";
        public static readonly string PaymentInitiated = "PaymentInitiatedMX @DealerAffixationCenterId, @orderNo,@orderType,@SlotId,@SlotTime,@SlotBookingDate,@HSRPStateID,@RTOLocationID,@RTOName,@OwnerName,@OwnerFatherName,@Address1,@State,@City,@Pin,@MobileNo,@LandlineNo,@EmailID,@VehicleClass,@VehicleType,@ManufacturerName,@ChassisNo,@EngineNo,@ManufacturingYear,@VehicleRegNo,@FrontPlateSize,@RearPlateSize,@TotalAmount,@NetAmount,@BookingType,@BookingClassType,@FuelType,@DealerId,@OEMID,@BookedFrom,@AppointmentType,@BasicAmount,@FitmentCharge,@ConvenienceFee,@HomeDeliveryCharge,@GSTAmount,@CustomerGSTNo,@VehicleRCImage,@BharatStage,@ShippingAddress1,@ShippingAddress2,@ShippingCity,@ShippingState,@ShippingPinCode,@ShippingLandMark,@IGSTAmount,@CGSTAmount,@SGSTAmount,'',@FrontLaserCode,@RearLaserCode, @NonHomologVehicle, @isSuperTag,@isFrame,@FrontHSRPFileName,@RearHSRPFileName,@FileFIR,@Firno,@FirDate,@Firinfo,@PoliceStation,@ReplacementReason";  
        public static readonly string InsertSuperTagOrder = "InsertSuperTagOrder @orderNo,@CustomerName, @CustomerMobile,@CustomerEmail,@CustomerBillingAddress,@StateName,@City',@Pin";
        public static readonly string RazorPayOrderIdUpdate = "update [BookMyHSRP].dbo.Appointment_bookingHist set razorpay_order_id = @Order_No where OrderNo =@Orderno ";
        public static readonly string PaymentInitiatedSticker = "PaymentInitiated_v1MX @DealerAffixationCenterId, @orderNo,@orderType,@SlotId,@SlotTime,@SlotBookingDate,@HSRPStateID,@RTOLocationID,@RTOName,@OwnerName,@OwnerFatherName,@Address1,@State,@City,@Pin,@MobileNo,@LandlineNo,@EmailID,@VehicleClass,@VehicleType,@ManufacturerName,@ChassisNo,@EngineNo,@ManufacturingYear,@VehicleRegNo,@FrontPlateSize,@RearPlateSize,@TotalAmount,@NetAmount,@BookingType,@BookingClassType,@FuelType,@DealerId,@OEMID,@BookedFrom,@AppointmentType,@BasicAmount,@FitmentCharge,@ConvenienceFee,@HomeDeliveryCharge,@GSTAmount,@CustomerGSTNo,@BharatStage,@BharatStage,@ShippingAddress1,@ShippingAddress2,@ShippingCity,@ShippingState,@ShippingPinCode,@ShippingLandMark,@IGSTAmount,@CGSTAmount,@SGSTAmount,@PlateSticker,@FrontLaserCode,@RearLaserCode, @NonHomologVehicle,@FrontLaserFileName,@RearLaserFileName,@FileName1,@FileName2,@LaserFileValidation,@supertag,@MRDCharges";


    }
}
