using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.OrderCancel.Queries;
using BookMyHsrp.Libraries.OrderCancel.Services;
using BookMyHsrp.Libraries.ReceiptValidity.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.OrderCancel.Services
{
    public class OrderCancelServices : IOrderCancelServices
    {

        private readonly DapperRepository _databaseHelper;
        private readonly string _connectionString;

        public OrderCancelServices(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
        }

        public async Task<dynamic> CancelOrderGet(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.CancelOrderGet, parameters);
            return receipts;
        }
        
            public async Task<dynamic> CancelOrderDetails(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.CancelOrderDetails, parameters);
            return receipts;
        }
        public async Task<dynamic> DealerWalletdetail(dynamic dto)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo",dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.DealerWalletdetail, parameters);
            return receipts;
        }
        public async Task<dynamic> DealerAddress(dynamic dto)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.DealerAddress, parameters);
            return receipts;
        }
    }
   

}
