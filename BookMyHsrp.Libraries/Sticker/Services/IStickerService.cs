using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.Sticker.Models.StickerModel;

namespace BookMyHsrp.Libraries.Sticker.Services
{
    public interface IStickerService
    {
        Task<dynamic> GetStateDetails(VahanDetailsDto requestDto);

        Task<dynamic> isAbleToBook(string VehicleRegNo, string chassisNo, string engineNo);

        Task<dynamic> RosmertaApi(string VehicleRegNo, string ChassisNo, string EngineNo, string Key);
        Task<dynamic> InsertVaahanLog(string VehicleRegNo, string chassisNo, string engineNo, VehicleDetails vahanDetailsDto);
        Task<dynamic> checkVehicleForStickerPr(string RegNo, string FrontLaser, string RearLaser);
        Task<dynamic> checkVehicleForStickerDL(string RegNo, string FrontLaser, string RearLaser);
        Task<dynamic> checkVehicleForStickerHR(string RegNo, string FrontLaser, string RearLaser);
        Task<dynamic> GetOemId(string makerName);
        Task<dynamic> VehicleSession(string VehicleCatVahan);

        Task<dynamic> VehiclePlateEntryLog(string SessionBs, string SessionRN, string SessionRD, string SessionCHN, string SessionEN, string SessionON, string SessionEID, string SessionMn, string SessionBA, string Stateid, string S_OrderType, string VehiceCat, string VehicleType, string S_StateId, string S_Oemid, string SFLCode, string SRLCode, string FuelType);

    }
}
