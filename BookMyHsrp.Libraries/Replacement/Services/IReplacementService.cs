using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.ReplacementHsrpColorStickerModel;

namespace BookMyHsrp.Libraries.Replacement.Services
{
    public interface IReplacementService
    {
        Task<dynamic> VahanInformation(ReplacementVahanDetailsDto requestDto);

        Task<dynamic> CheckOrderExixts(string VehicleRegNo, string chassisNo, string engineNo);

        Task<dynamic> OemRtoMapping(string VehicleRegNo, string OemId);
        Task<dynamic> InsertVaahanLog(string VehicleRegNo, string chassisNo, string engineNo, ReplacementVehicleDetails vahanDetailsDto);

        Task<dynamic> InsertVaahanLogWithoutApi(string VehicleRegNo, string chassisNo, string engineNo, ReplacementVehicleDetails vahanDetailsDto);

        Task<dynamic> GetOrderNumber(ReplacementVahanDetailsDto vahanDetailsDto);

        Task<dynamic> SelectByOrderNumber(string OrderNo);
        Task<dynamic> CheckDateBetweenCloseDate(ReplacementVahanDetailsDto vahanDetailsDto);
        Task<dynamic> GetOemId(string makerName);
        Task<dynamic> GetVehicleDetails(string OemId, string VehicleClass);
        Task<dynamic> TaxInvoiceSummary(string OemId);

        Task<dynamic> BookingHistoryId(string RegistrationNo, string ChassisNo, string EngineNo);
        Task<dynamic> VehicleSession(string VehicleCatVahan);

        Task<dynamic> OemVehicleType(string HSRPHRVehicleType, string VehicleTypeVahan, int newId, string FuelTypeVahan);

        Task<dynamic> InsertMisMatchDataLog(dynamic customerInfo, string OemId);

        Task<dynamic> InsertVahanLogQueryCustomer(dynamic requestDto, string OrderType, string billingaddress, string NonHomo, string OemId);

        Task<dynamic> RosmertaApi(string VehicleRegNo, string ChassisNo, string EngineNo, string Key);

        Task<dynamic> SessionBookingDetails(dynamic requestDto);

        #region 7 days, 90 days, 180 days
        Task<dynamic> strBmhsrp1(string RegistrationNo, string ChassisNo, string EngineNo);

        Task<dynamic> strBmhsrp2(string RegistrationNo, string ChassisNo, string EngineNo);

        Task<dynamic> strHsrpRecord1(string RegistrationNo, string ChassisNo);

        Task<dynamic> strHsrpRecord2(string RegistrationNo, string ChassisNo);
        Task<dynamic> strHsrpRecord3(string OrderNo, int days);

        Task<dynamic> strHsrpRecord4(string RegistrationNo, string ChassisNo);

        Task<dynamic> strHsrpRecord5(string RegistrationNo, string ChassisNo);

        Task<dynamic> strHsrpRecord6(string RegistrationNo, string ChassisNo, string HSRPRecordId);

        Task<dynamic> strHsrpRecord7(string RegistrationNo, string ChassisNo);

        Task<dynamic> strHsrpRecord8(string RegistrationNo, string ChassisNo);

        Task<dynamic> strHsrpRecord9(string RegistrationNo, string ChassisNo);

        Task<dynamic> strHsrpRecord10(string RegistrationNo, string ChassisNo);

        Task<dynamic> strBmHSRPOrissa(string RegistrationNo, string ChassisNo, string EngineNo);

        #endregion
    }
}
