using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.AppointmentSlot.Queries;
using BookMyHsrp.Libraries.DealerDelivery.Queries;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.DealerDelivery.Services
{
    public class DealerDeliveryService:IDealerDeliveryService
    {
        private readonly FetchDataAndCache _fetchDataAndCache;
        //private readonly AppSettings _appSettings;
        private readonly DapperRepository _databaseHelper;
        private readonly DapperRepository _databaseHelperPrimary;
        private readonly DynamicDataDto _dynamicDataDto;
        private readonly string _connectionString;
        private readonly string _vehicleStatusAPI;
        private readonly string _oemId;
        private readonly string _nonHomo;
        private readonly string _nonHomoOemId;
        string msg = string.Empty;
        public DealerDeliveryService(IOptionsSnapshot<ConnectionString> connectionStringOptions, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;

            _databaseHelper = new DapperRepository(_connectionString);
            //_appSettings = appSettings;
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelperPrimary = new DapperRepository(_connectionString);

        }
        public async Task<dynamic> GetDealersForRajasthan(string OemId, string StateId,string Vehicletype, string VehicleCat, string VehicleClass, string Ordertype)
        {
            var parameters =new DynamicParameters();
            parameters.Add("@oemid", OemId);
            parameters.Add("@StateId", StateId);
            //parameters.Add("@Searchtext", "");
            parameters.Add("@VehicleCat", VehicleCat);
            parameters.Add("@VehicleType", Vehicletype);
            parameters.Add("@VehicleClass", VehicleClass);
            parameters.Add("@OrderType", Ordertype);
            var result= await _databaseHelperPrimary.QueryAsync<dynamic>(DealerDeliveryQueries.GetDealersForRajasthan, parameters);
            return result;


        }
        public async Task<dynamic> GetDealersForRajasthanElse(string OemId, string StateId, string Vehicletype, string VehicleCat, string VehicleClass, string Ordertype)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@oemid", OemId);
            parameters.Add("@StateId", StateId);
            //parameters.Add("@Searchtext", "");
            parameters.Add("@VehicleCat", VehicleCat);
            parameters.Add("@VehicleType", Vehicletype);
            parameters.Add("@VehicleClass", VehicleClass);
            parameters.Add("@OrderType", Ordertype);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(DealerDeliveryQueries.GetDealersForRajasthanElse, parameters);
            return result;


        }
        public async Task<dynamic> GetDealers(string OemId, string StateId, string Vehicletype, string VehicleCat, string VehicleClass, string Ordertype)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@oemid", OemId);
            parameters.Add("@StateId", StateId);
            //parameters.Add("@Searchtext", "");
            parameters.Add("@VehicleCat", VehicleCat);
            parameters.Add("@VehicleType", Vehicletype);
            parameters.Add("@VehicleClass", VehicleClass);
            parameters.Add("@OrderType", Ordertype);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(DealerDeliveryQueries.GetDealers, parameters);
            return result;


        }
        public async Task<dynamic> GetDealersElse(string OemId, string StateId, string Vehicletype, string VehicleCat, string VehicleClass, string Ordertype)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@oemid", OemId);
            parameters.Add("@StateId", StateId);
            //parameters.Add("@Searchtext", "");
            parameters.Add("@VehicleCat", VehicleCat);
            parameters.Add("@VehicleType", Vehicletype);
            parameters.Add("@VehicleClass", VehicleClass);
            parameters.Add("@OrderType", Ordertype);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(DealerDeliveryQueries.GetDealersElse, parameters);
            return result;


        }
        public async Task<dynamic> CheckOemRate(string Ordertype, string VehicleType, string StateIdBackup)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderType", Ordertype);
            parameters.Add("@VehicleType", VehicleType);
            //parameters.Add("@Searchtext", "");
            parameters.Add("@HSRP_StateId", StateIdBackup);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(DealerDeliveryQueries.CheckOemRate, parameters);
            return result;


        }
       public async Task<dynamic> CheckOemRateQuery(string OemId, string Ordertype, string VehicleClass, string VehicleType, string VehicleCategoryId, string FuelType, string DeliveryPoint, string StateId, string StateName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OemID",OemId);
            parameters.Add("@OrderType", Ordertype);
            parameters.Add("@VehicleClass", VehicleClass);
            parameters.Add("@VehicleType", VehicleType);
            parameters.Add("@VehicleCategoryID", VehicleCategoryId);
            parameters.Add("@FulType", FuelType);
            parameters.Add("@AppointmentType", DeliveryPoint);
            parameters.Add("@HSRP_StateID", StateId);
            parameters.Add("@CustomerStateName", StateName);
            parameters.Add("@PlateSticker", "");
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(DealerDeliveryQueries.CheckOemRateQuery, parameters);
            return result;


        }
        public async Task<dynamic> GetAffixationId(string Id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", Id);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(DealerDeliveryQueries.GetAffixationId, parameters);
            return result;
        }


    }
    
}
