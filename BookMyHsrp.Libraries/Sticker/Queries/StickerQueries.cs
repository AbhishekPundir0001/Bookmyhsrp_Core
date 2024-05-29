using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Sticker.Queries
{
    public class StickerQueries
    {
        public static string GetStateDetails => "select HSRP_StateID,HSRPStateName,StateCode as HSRPStateShortName from BMHSRPStates where HSRP_StateID=@StateId";
        public static string GetBookingHistory => "exec CheckStickerBooking @RegistrationNo,@ChassisNo,@EngineNo";
        public static string InsertVahanLog => "insert into [BookMyHSRP].dbo.VahanResponseLog (VehicleRegNo,ChassisNo,EngineNo,Fuel,BharatState,VehicleClass,VehicleType,Maker,VahanRespose,RegDate,PlateSticker,VahanFrontLaserCode,VahanRearLaserCode,RequestFrom) values(@RegistrationNo,@ChassisNo,@EngineNo,@Fuel,@Norms,@VehicleCategory,@VehicleType,@Maker,@ResponseJson,@RegistrationDate,'Plate',@HsrpFrontLasserCode,@HsrpRearLasserCode,'API')";
        public static string checkVehicleForSticker => "select top 1 Vehicleregno from [hsrpoem].dbo.hsrprecords WITH (NOLOCK) where Vehicleregno =@RegNo and (hsrp_front_lasercode=@hsrp_front_lasercode or hsrp_rear_lasercode=@hsrp_front_lasercode) and (hsrp_rear_lasercode=@hsrp_rear_lasercode or hsrp_front_lasercode=@hsrp_rear_lasercode) and Vehicletype not in ('Scooter','Motor Cycle')";
        public static string checkVehicleForStickerDL => "select top 1 Vehicleregno from hsrprecords WITH (NOLOCK) where Vehicleregno =@RegNo and (hsrp_front_lasercode=@hsrp_front_lasercode or hsrp_rear_lasercode=@hsrp_front_lasercode) and (hsrp_rear_lasercode=@hsrp_rear_lasercode or hsrp_front_lasercode=@hsrp_rear_lasercode) and Vehicletype not in ('Scooter','Motor Cycle')";
        public static string GetOemId => "select Oemid,'https://bookmyhsrp.com/OEMLOGO'+REPLACE(replace(oem_logo,'.png','.jpg'),'Images/brands','') as oem_logo from[hsrpoem].dbo.oemmaster where vahanoemname=@MakerName union select Oemid,'https://bookmyhsrp.com/OEMLOGO' + REPLACE(replace(oem_logo, '.png', '.jpg'), 'Images/brands', '') as oem_logo from[hsrpoem].[dbo].[OEMMasterNameMapping]  where vahanoemname=@MakerName";
        public static string VehicleSession => "select HSRPHRVehicleType,VehicleTypeid,VehicleCategory from [hsrpoem].[dbo].[VahanVehicleType] where VahanVehicleType=Trim(@VehicleCatType) ";
        public static string VehiclePlateEntryLog => " insert into [BookMyHSRP].dbo.VehiclePlateEntryLog (vahanbsstage, VehicleRegNo, RegistrationDate,ChassisNo,EngineNo,OwnerName,EmailID, MobileNo, CustomerAddress,StateName,OwnerCity,GSTNO,VahanValidation,VahanResponse,Ordertype,VehicleType,VehicleClass,Created_Date,HSRP_StateID,OEMID,StickerValidation,FLaserCode,RLaserCode,fuelType,vahanfuel) values (@SessionBs,@SessionRN,@SessionRD,@SessionCHN,@SessionEN,@SessionON,@SessionEID,@SessionMn,@SessionBA,@Stateid,'','','','',@S_OrderType,@VehicleCat,@VehicleType,getdate(),@S_StateId,@S_Oemid,'N',@SFLCode,@SRLCode,@FuelType,@FuelType) ";

        public static string DateFormate => "DECLARE @date DATETIME = GETDATE(); SELECT FORMAT(@date, 'yyyyMMddHHmmssfff') AS FormattedDate;";


    }
}
