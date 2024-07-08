using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.ReAppointment.Queries;
using BookMyHsrp.Libraries.ReAppointmentBookingSummary.Queries;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BookMyHsrp.Libraries.ReAppointmentBookingSummary.Services
{
    public class ReAppointmentBookingSummaryServices : IReAppointmentBookingSummaryServices
    {
        private readonly DapperRepository _databaseHelper;
        private readonly DapperRepository _databaseHelper1;
        private readonly string _connectionString;
        private readonly string _connectionString1;

        public ReAppointmentBookingSummaryServices(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _connectionString1 = connectionStringOptions.Value.SecondaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
            _databaseHelper1 = new DapperRepository(_connectionString1);
        }
        public async Task<dynamic> ReScheduleFreeCheck(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(ReAppointmentBookingSummaryQueries.checking, parameters);
            return receipts;
        }
    }
}
