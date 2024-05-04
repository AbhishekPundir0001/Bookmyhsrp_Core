using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.HsrpState.Models;
using BookMyHsrp.Libraries.HsrpState.Queries;
using BookMyHsrp.Libraries.OemMaster.Models;
using BookMyHsrp.Libraries.OemMaster.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OemMaster.Services
{
    public class OemMasterService:IOemMasterService
    {
        private readonly FetchDataAndCache _fetchDataAndCache;
        private readonly DapperRepository _databaseHelper;
        private readonly string _connectionString;
        public OemMasterService(IOptionsSnapshot<ConnectionString> connectionStringOptions, FetchDataAndCache fetchDataAndCache)
        {
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
            _fetchDataAndCache = fetchDataAndCache;
        }

        public Task<IEnumerable<OemMasterModel.OemVehicleTypeList>> GetAllOemByVehicleType(string vehicleType)
        {
            var oemid = _fetchDataAndCache.GetStringFromCache("OemId");
            var parameter = new DynamicParameters();
            parameter.Add("@VehicleType", vehicleType);
            parameter.Add("@OemId", oemid);
            var result = _databaseHelper.QueryAsync<OemMasterModel.OemVehicleTypeList>(OemMasterQueries.GetAllOemByVehicleType, parameter);
            return result;



        }
    }
}
