using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.OrderCancel.Queries;
using BookMyHsrp.Libraries.ReAppointment.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.ReAppointment.Services
{
    public class ReAppointmentServices : IReAppointmentServices
    {
        private readonly DapperRepository _databaseHelper;
        private readonly DapperRepository _databaseHelper1;
        private readonly string _connectionString;
        private readonly string _connectionString1;

        public ReAppointmentServices(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _connectionString1 = connectionStringOptions.Value.SecondaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
            _databaseHelper1 = new DapperRepository(_connectionString1);
        }

        public async Task<dynamic> GetOrderDetails(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(ReAppointmentQueries.GetOrderDetails, parameters);
            return receipts;
        }
        public async Task<dynamic> AuthorisedReschedule(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper1.QueryAsync<dynamic>(ReAppointmentQueries.AuthorisedReschedule, parameters);
            return receipts;
        }
        public async Task<dynamic> AuthorisedRescheduleSticker(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper1.QueryAsync<dynamic>(ReAppointmentQueries.AuthorisedRescheduleSticker, parameters);
            return receipts;
        }
        public async Task<dynamic> VerifyRescheduleSticker(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleRegNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(ReAppointmentQueries.VerifyRescheduleSticker, parameters);
            return receipts;
        }
    }
}
