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
using static BookMyHsrp.Libraries.Receipt.Models.ReceiptModels;

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
        public async Task<dynamic> DealerAddress(dynamic dealerid)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Dealerid", dealerid);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.DealerAddress, parameters);
            return receipts;
        }
        public async Task<dynamic> OrderStatusCancel(dynamic dto)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.OrderStatus, parameters);
            return receipts;
        }

        public async Task<dynamic> voidOrder(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            parameters.Add("@Reason", dto.Reason);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.voidOrder, parameters);
            return receipts;
        }

        public async Task<dynamic> OrderStatusUpdate(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNo", dto.OrderNo);
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.OrderStatusUpdate, parameters);

            return receipts;
        }

        public async Task<dynamic> checkBookApp(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.checkBookApp, parameters);
            return receipts;
        }

        public async Task<dynamic> updateBookApp(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.updateBookApp, parameters);
            return receipts;
        }

        public async Task<dynamic> Smsqry(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.Smsqry, parameters);
            return receipts;
        }

        public async Task<dynamic> SMSLogSaveQuery(dynamic dto,string MobileNo , string sms, string responsecode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OwnerName", dto.OrderNo);
            parameters.Add("@MobileNo", MobileNo);
            parameters.Add("@SMSText", sms);
            parameters.Add("@SentResponseCode", responsecode);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.SMSLogSaveQuery, parameters);
            return receipts;
        }

        public async Task<dynamic> checkcancelrecord(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.checkcancelrecord, parameters);
            return receipts;
        }

        public async Task<dynamic> updatecancelledlog(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            parameters.Add("@AppointmentSlot", dto.AppointmentSlot);
            parameters.Add("@EngineNo", dto.EngineNo);
            parameters.Add("@ChassisNo", dto.ChassisNo);
            parameters.Add("@VehicleMake", dto.VehicleMake);
            parameters.Add("@FuelType", dto.FuelType);
            parameters.Add("@FitmentAddress", dto.FitmentAddress);
            parameters.Add("@VehicleType", dto.VehicleType);
            parameters.Add("@VehicleClass", dto.VehicleClass);
            parameters.Add("@Reason", dto.Reason);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.updatecancelledlog, parameters);
            return receipts;
        }

        public async Task<dynamic> updatecancelledlog2(dynamic logDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleNo", logDto.VehicleNo);
            parameters.Add("@OrderNo", logDto.OrderNo);
            parameters.Add("@AppointmentDate", logDto.AppointmentDate);
            parameters.Add("@AppointmentSlot", logDto.AppointmentSlot);
            parameters.Add("@EngineNo", logDto.EngineNo);
            parameters.Add("@ChassisNo", logDto.ChassisNo);
            parameters.Add("@VehicleMake", logDto.VehicleMake);
            parameters.Add("@FuelType", logDto.FuelType);
            parameters.Add("@FitmentAddress", logDto.FitmentAddress);
            parameters.Add("@VehicleType", logDto.VehicleType);
            parameters.Add("@VehicleClass", logDto.VehicleClass);
            parameters.Add("@Reason", logDto.Reason);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.updatecancelledlog2, parameters);
            return receipts;
        }

        public async Task<dynamic> cancelledlogerror(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            parameters.Add("@AppointmentDate", dto.AppointmentDate);
            parameters.Add("@AppointmentSlot", dto.AppointmentSlot);
            parameters.Add("@EngineNo", dto.EngineNo);
            parameters.Add("@ChassisNo", dto.ChassisNo);
            parameters.Add("@VehicleMake", dto.VehicleMake);
            parameters.Add("@FuelType", dto.FuelType);
            parameters.Add("@FitmentAddress", dto.FitmentAddress);
            parameters.Add("@VehicleType", dto.VehicleType);
            parameters.Add("@VehicleClass", dto.VehicleClass);
            parameters.Add("@ExceptionMsg", dto.ExceptionMsg);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.cancelledlogerror, parameters);
            return receipts;
        }
        public async Task<dynamic> cancellationfinalpage(dynamic dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@VehicleregNo", dto.VehicleregNo);
            parameters.Add("@OrderNo", dto.OrderNo);
            var receipts = await _databaseHelper.QueryAsync<dynamic>(OrderCancelQueries.cancellationpagequery, parameters);
            return receipts;

        }


    }
   

}
