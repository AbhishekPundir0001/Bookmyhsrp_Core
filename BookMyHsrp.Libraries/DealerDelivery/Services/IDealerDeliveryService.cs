using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Libraries.DealerDelivery.Services
{
    public interface IDealerDeliveryService
    {
        Task<dynamic> GetDealersForRajasthan(string OemId, string StateId,string VehicleType ,string VehicleCat, string VehicleClass, string Ordertype);
        Task<dynamic> GetDealersForRajasthanElse(string OemId, string StateId,string VehicleType ,string VehicleCat, string VehicleClass, string Ordertype);
        Task<dynamic> GetDealers(string OemId, string StateId,string VehicleType ,string VehicleCat, string VehicleClass, string Ordertype);
        Task<dynamic> GetDealersElse(string OemId, string StateId,string VehicleType ,string VehicleCat, string VehicleClass, string Ordertype);
        Task<dynamic> CheckOemRate(string Ordertype, string VehicleType,string StateIdBackup);
        Task<dynamic> CheckOemRateQuery(string OemId,string Ordertype,string VehicleClass,string VehicleType,string VehicleCategoryId,string FuelType,string DeliveryPoint,string StateId,string StateName);
        Task<dynamic> GetAffixationId(string Id);
    }
}
