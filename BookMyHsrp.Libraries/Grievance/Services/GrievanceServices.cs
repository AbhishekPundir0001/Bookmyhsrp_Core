using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.Grievance.Queries;
using BookMyHsrp.Libraries.OrderCancel.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.Grievance.Services
{
    public class GrievanceServices : IGrievanceServices
    {
        private readonly DapperRepository _databaseHelper;
        private readonly string _connectionString;
        public GrievanceServices(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
        }

        public async Task<dynamic> getRecord(dynamic VehicleRegNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", VehicleRegNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(GreivanceQueries.getRecord, parameters);
            return receipts;
        }
        public string GetRandomNumber()
        {
            Random r = new Random();
            var x = r.Next(0, 9);
            return x.ToString("0");
        }
            public async Task<dynamic> greivanceinsert(string VehicleRegNo, string OrderNo, string MobileNo, string EmailId, string Query, string CustomerName)
        {
            var parameters = new DynamicParameters();
            string TicketNo = "BMHSRPTICKETNO" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + GetRandomNumber();
            parameters.Add("@VehicleregNo", VehicleRegNo);
            parameters.Add("OrderNo", OrderNo);
            parameters.Add("MobileNo", MobileNo);
            parameters.Add("EmailId", EmailId);
            parameters.Add("Query", Query);
            parameters.Add("TicketNo", TicketNo);
            parameters.Add("CustomerName", CustomerName);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(GreivanceQueries.greivanceinsert, parameters);
            return TicketNo;
        }
    }
}
