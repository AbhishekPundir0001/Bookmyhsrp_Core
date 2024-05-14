using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HomeDelivery.Queries;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookMyHsrp.Libraries.HsrpWithColorSticker.Models.HsrpColorStickerModel;

namespace BookMyHsrp.Libraries.HomeDelivery.Services
{
    public class HomeDeliveryService
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
        public HomeDeliveryService(IOptionsSnapshot<ConnectionString> connectionStringOptions, FetchDataAndCache fetchDataAndCache, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;

            _databaseHelper = new DapperRepository(_connectionString);
            //_appSettings = appSettings;
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelperPrimary = new DapperRepository(_connectionString);
            //_httpContextAccessor = httpContextAccessor;
            _fetchDataAndCache = fetchDataAndCache;
            _vehicleStatusAPI = dynamicData.Value.VehicleStatusAPI;
            //_oemId = dynamicData.Value.OemID;
            //_nonHomo = dynamicData.Value.NonHomo;
            //_nonHomoOemId = dynamicData.Value.NonHomoOemId;

        }
        public async Task<dynamic> IsHomeDeliveryAllowed(int stateId, string CheckFor)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@StateId", stateId);
            parameters.Add("@CheckFor", CheckFor);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(
                 HomeDeliveryQueries.CheckAvalibility, parameters);
            return result;
        }
        public async Task<dynamic> CheckPinCode(dynamic sessionValue, string pincode)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@OemId", sessionValue.OemId);
            parameters.Add("@StateId", sessionValue.StateId);
            parameters.Add("@PinCode", pincode);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(
                 HomeDeliveryQueries.CheckPincode, parameters);
            return result;
        }
    }
}
