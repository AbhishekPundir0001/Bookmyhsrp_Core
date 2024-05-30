using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.Receipt.Queries;
using BookMyHsrp.Libraries.Receipt.Services;
using BookMyHsrp.Libraries.ReceiptValidity.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ReceiptValidity.Services
{
    public class ReceiptValidityService : IReceiptValidityService
    {
        //private readonly FetchDataAndCache _fetchDataAndCache;
        private readonly DapperRepository _databaseHelper;
        private readonly string _connectionString;

        public ReceiptValidityService(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
        }

        public async Task<dynamic> CheckReceiptValidity(dynamic dto)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(ReceiptValidityQueries.ReceiptValidity, parameters);
            return receipts;
        }
    }
}