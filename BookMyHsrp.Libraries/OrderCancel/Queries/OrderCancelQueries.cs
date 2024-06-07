using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OrderCancel.Queries
{
    public class OrderCancelQueries
    {
        public static string DealerWalletdetail = "select * from[HSRPOEM].dbo.dealerwalletorder where OrderNo = @OrderNo and VehicleRegNo = @VehicleregNo";
        public static string CancelOrderGet = "exec usp_CancelOrderGet @OrderNo,@VehicleregNo";
        public static string  CancelOrderDetails = "exec usp_Appointment_BookingHist_CencelOrder @OrderNo, @VehicleregNo" ;
        public static string DealerAddress = "select DealerAffixationCenterAddress,DealerAffixationCenterName,DealerAffixationCenterCity,DealerAffixationCenterPinCode from [HSRPOEM].dbo.DealerAffixationCenter where DealerID = @DealerId ";
        public static string OrderStatus = "Select top 1 isnull(OrderStatus,'') OrderStatus, OrderNo FROM HSRPOEM.dbo.hsrprecords  WHERE  OrderNo= @OrderNo and IsBookMyHsrpRecord ='Y'";
        public static string voidOrder = "exec HSRPOEM.dbo.sp_bookmyhsrp_void @OrderNo ,@Reason , @VehicleregNo ";
        public static string OrderStatusUpdate = "UPDATE Appointment_BookingHist SET OrderStatus='ORDER CANCELLED' WHERE OrderNo=@OrderNo and VehicleRegNo=@VehicleregNo";
        public static string checkBookApp = "select top 1 orderno from hsrpoem.dbo.BookMyHSRPAppointment WHERE OrderNo=@OrderNo and VehicleRegNo=@VehicleregNo";
        public static string updateBookApp = "UPDATE hsrpoem.dbo.BookMyHSRPAppointment SET OrderStatus = 'ORDER CANCELLED',Process = 'R',ProcessMessage = @Reason WHERE OrderNo=@OrderNo and VehicleRegNo=@VehicleregNo ";
        public static string Smsqry = "select top 1 LandLineNo from Appointment_BookingHist WHERE OrderNo=@OrderNo and VehicleRegNo=@VehicleregNo";
        public static string SMSLogSaveQuery = "INSERT INTO [BookMyHSRP].dbo.Appointment_SMSDetails " +
    "(OwnerName, VehicleRegNo, MobileNo, SMSText, SentResponseCode, SentDateTime) " +
    "VALUES (@OwnerName, @VehicleRegNo, @MobileNo, @SMSText, @SentResponseCode, GETDATE())";
        public static string checkcancelrecord = "select 1 from [BookMyHSRP].dbo.Appointment_BookingHist a inner join[BookMyHSRP].dbo.ExpressAffixatonCenter b on a.affix_id = b.DealeraffixationId where a.OrderNo =@OrderNo and VehicleRegNo=@VehicleregNo ";
        public static string updatecancelledlog = "INSERT INTO HSRPOEM.dbo.CancelledLog(OrderNo, VehicleNo, AppointmentDate, AppointmentSlot, EngineNo, ChassisNo, VehicleMake, FuelType, FitmentAddress, VehicleType, VehicleClass, Reason, CancelledDate, OrderStatus, PrevOrderStatus) VALUES (@OrderNo, @VehicleNo, '1900-01-01', @AppointmentSlot, @EngineNo, @ChassisNo, @VehicleMake, @FuelType, @FitmentAddress, @VehicleType, @VehicleClass, @Reason, GETDATE(), 'ORDER CANCELLED', 'Success')";
        public static string updatecancelledlog2 = "INSERT INTO HSRPOEM.dbo.CancelledLog(OrderNo, VehicleNo, AppointmentDate, AppointmentSlot, EngineNo, ChassisNo, VehicleMake, FuelType, FitmentAddress, VehicleType, VehicleClass, Reason, CancelledDate, OrderStatus, PrevOrderStatus) VALUES (@OrderNo, @VehicleNo, @AppointmentDate, @AppointmentSlot, @EngineNo, @ChassisNo, @VehicleMake, @FuelType, @FitmentAddress, @VehicleType, @VehicleClass, @Reason, GETDATE(), 'ORDER CANCELLED', 'Success')";
        public static string cancelledlogerror = "Insert into HSRPOEM.dbo.CancelledLog(OrderNo,VehicleNo,AppointmentDate,AppointmentSlot,EngineNo,ChassisNo,VehicleMake,FuelType,FitmentAddress,VehicleType,VehicleClass,CancelledDate,ExceptionMsg) VALUES (@OrderNo, @VehicleregNo, @AppointmentDate, @AppointmentSlot, @EngineNo, @ChassisNo, @VehicleMake, @FuelType, @FitmentAddress, @VehicleType, @VehicleClass,GETDATE(),@ExceptionMsg";
        public static string cancellationpagequery = "select top 1 HSRPRecord_CreationDate,case when getdate() Between HSRPRecord_CreationDate And DATEADD(HOUR, 24, HSRPRecord_CreationDate) then 'Y' else 'N' end isAbleToCancelled,OrderNo,OrderStatus,SlotTime,SlotBookingDate,EmailID,ChassisNo,EngineNo,VehicleRegNo,Dealerid,OrderStatus,VehicleClass,VehicleType,ManufacturerModel,fuelType,ManufacturerName from Appointment_BookingHist where OrderNo= @OrderNo and VehicleRegNo= @VehicleregNo";
    }
}

