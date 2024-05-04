using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HsrpWithColorSticker.Queries
{
    public class HsrpWithColorStickerQueries
    {
        public static string GetStateDetails => "select HSRP_StateID,HSRPStateName,StateCode as HSRPStateShortName from BMHSRPStates where HSRP_StateID=@StateId";
        public static string GetBookingHistory => "SELECT BookingHistoryID FROM [BookMyHSRP].dbo.Appointment_BookingHist WHERE VehicleRegNo = UPPER(LTRIM(RTRIM(@RegistrationNo))) AND RIGHT(LTRIM(RTRIM(@ChassisNo)), 5) = RIGHT(@ChassisNo, 5) AND RIGHT(LTRIM(RTRIM(@EngineNo)), 5) = RIGHT(@EngineNo, 5)   AND OrderStatus IN ('Success', 'Shipped', 'Success-Test')   AND PlateSticker = 'plate';"; 
        public static string InsertVahanLog => "Insert into [BookMyHSRP].dbo.VahanResponseLog ([VehicleRegNo],[ChassisNo],[EngineNo],[Fuel],[BharatState],[VehicleClass],[VehicleType],[Maker],[VahanRespose],[RegDate],[PlateSticker],[VahanFrontLaserCode],[VahanRearLaserCode] ) values(@RegistrationNo,@ChassisNo,@EngineNo,@Fuel,@Norms,@VehicleCategory,@VehicleType,@Maker,@ResponseJson,@RegistrationDate,'Plate',@HsrpFrontLasserCode,@HsrpRearLasserCode)";
        public static string GetOemId => "select Oemid,'https://bookmyhsrp.com/OEMLOGO'+REPLACE(replace(oem_logo,'.png','.jpg'),'Images/brands','') as oem_logo from[hsrpoem].dbo.oemmaster where vahanoemname=@MakerName union select Oemid,'https://bookmyhsrp.com/OEMLOGO' + REPLACE(replace(oem_logo, '.png', '.jpg'), 'Images/brands', '') as oem_logo from[hsrpoem].[dbo].[OEMMasterNameMapping]  where vahanoemname=@MakerName";
        public static string insertVahanLogQuery => "insert into [BookMyHSRP].dbo.VahanResponseLog ([VehicleRegNo] ,[ChassisNo],[EngineNo],[Fuel],[BharatState],[VehicleClass],[VehicleType],[Maker],[VahanRespose],[RegDate],[PlateSticker],[VahanFrontLaserCode],[VahanRearLaserCode] ) values ( @RegistrationNo,@ChassisNo,@EngineNo,@Fuel,@Norms,@VehicleCategory,@VehicleType,@Maker,@ResponseJson,@RegistrationDate,'Plate',@HsrpFrontLasserCode,@HsrpRearLasserCode) ";
        public static string checkMappingInHsrpOem => "select Oemid,'https://bookmyhsrp.com/OEMLOGO'+REPLACE(replace(oem_logo,'.png','.jpg'),'Images/brands','') as oem_logo from [hsrpoem].dbo.oemmaster  where vahanoemname=@MakerName union select Oemid,'https://bookmyhsrp.com/OEMLOGO' + REPLACE(replace(oem_logo, '.png', '.jpg'), 'Images/brands', '') as oem_logo from[hsrpoem].[dbo].[OEMMasterNameMapping]  where vahanoemname =@MakerName ";
        public static string OemRtoMapping => "execute CheckOemMappedOrNot @OemId, @RegistrationNumber";
        public static string GetTaxInvoiceSummaryReport => "select IsVehicleTypeEnable from [hsrpoem].dbo.oemmaster where Oemid=@OemId";
        public static string GetBookingHistoryId => "SELECT BookingHistoryID FROM [BookMyHSRP].dbo.Appointment_BookingHist WHERE VehicleRegNo = @RegistrationNo AND RIGHT(TRIM(Chassisno), 5) = @ChassisNo AND RIGHT(TRIM(Engineno), 5) =@EngineNo AND OrderStatus IN ('Success', 'Shipped', 'Success-Test')  AND PlateSticker = 'plate'";
        public static string InsertVahanLogQuery => "Insert into [BookMyHSRP].dbo.VehiclePlateEntryLog (vahanbsstage, VehicleRegNo, RegistrationDate,ChassisNo,EngineNo,OwnerName,EmailID, MobileNo,CustomerAddress,StateName,OwnerCity,GSTNO,VahanValidation,VahanResponse,Ordertype, VehicleType,VehicleClass,Created_Date,VahanDateTime,HSRP_StateID,OEMID,NonHomologVehicle) values(@BharatStage,@RegistrationNo,@RegistrationDate,@ChassisNo,@EngineNo,@OwnerName,@EmailId,@MobileNo,REPLACE(@BillingAddress, '''', ''''''),@StateName,'','','','ORDER Already Created !',@OrderType, @VehicleCategory ,@VehicleType,getdate(),getdate(),@StateId,@OemId,@NonHomo) ";
        public static string VehicleSession => "select HSRPHRVehicleType,VehicleTypeid,VehicleCategory from [hsrpoem].[dbo].[VahanVehicleType] where VahanVehicleType=Trim(@VehicleCatType) ";
        public static string GetOemVehicleType => "exec GetOEMvehicleType @VahanVehicleType, @OrderType , @vehicleclass, @oemid , @FuelType";
        public static string InsertMissMatchDataLog => "INSERT INTO [dbo].[MisMatchDataLog] ([VahanMaker],[Vehicleregno],[Chassisno],[EngineNO],[MobileNo],[Emailid],[VahanVehicleType],[OrderType],[VehicleClass],[Oemid],[ErrorMsg])values (@Maker,@RegistrationNo,@ChassisNo,Trim(@EngineNo),Trim(@MobileNo),@EmailId,Trim(@VehicleCatVahan),'OB',Trim(@VehicleType),@OemId,'Vehicle Type Mismatch')";


    }
}
