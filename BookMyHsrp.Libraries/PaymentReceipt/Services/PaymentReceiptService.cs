using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.OrderCancel.Queries;
using BookMyHsrp.Libraries.PaymentReceipt.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.PaymentReceipt.Services
{
    public class PaymentReceiptService:IPaymentReceiptService
    {

        private readonly DapperRepository _databaseHelper;
        private readonly string _connectionString;

        public PaymentReceiptService(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
        }

        public async Task<dynamic> UpdateStatusOfPayment(string OrderNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(PaymentReceiptQueries.UpdateStatus, parameters);
            return receipts;
        }

    }
}
