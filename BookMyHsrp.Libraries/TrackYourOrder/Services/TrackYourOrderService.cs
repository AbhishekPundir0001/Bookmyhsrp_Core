using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.Receipt.Queries;
using BookMyHsrp.Libraries.TrackYoutOrder.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.TrackYoutOrder.Services
{
    public class TrackYourOrderService : ITrackYourOrderService
    {
        private readonly DapperRepository _databaseHelper;
        private readonly string _connectionString;

        public TrackYourOrderService(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
        }

        public async Task<dynamic> GetTrackYourOrderStatus(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", dto.OrderNo);
            parameters.Add("@VehicleregNo", dto.VehicleregNo);

            var receipts = await _databaseHelper.QueryAsync<dynamic>(TrackYourOrderQueries.TrackYourOrder, parameters);
            return receipts;
        }
        public async Task<dynamic> GetTrackYourOrderStatusSp(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", dto.OrderNo);
            parameters.Add("@VehicleRegNo", dto.VehicleregNo);

            var spreceipts = await _databaseHelper.QueryAsync<dynamic>(TrackYourOrderQueries.SpTrackYourOrder, parameters);
            return spreceipts;
        }
        public async Task<dynamic> GetFitmentDate(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", dto.OrderNo);
            parameters.Add("@VehicleRegNo", dto.VehicleregNo);

            var fitmentdate = await _databaseHelper.QueryAsync<dynamic>(TrackYourOrderQueries.FitmentDate, parameters);
            return fitmentdate;
        }
        public async Task<dynamic> GetDealerName(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Dealerid", dto.Dealerid);
            var DealerName = await _databaseHelper.QueryAsync<dynamic>(TrackYourOrderQueries.Dealername, parameters);
            return DealerName;
        }

    }
}
