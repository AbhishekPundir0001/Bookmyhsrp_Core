using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.AppointmentSlot.Queries;
using BookMyHsrp.Libraries.HsrpWithColorSticker.Queries;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.AppointmentSlot.Services
{
    public class AppointmentSlotService : IAppointmentSlotServices
    {
        private readonly DapperRepository _databaseHelperPrimary;
        private readonly string _connectionString;
        private readonly DapperRepository _databaseHelper;
        public AppointmentSlotService(IOptionsSnapshot<ConnectionString> connectionStringOptions, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;

            _databaseHelper = new DapperRepository(_connectionString);
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelperPrimary = new DapperRepository(_connectionString);
        }

        public async Task<dynamic> GetAffixationId(string Id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", Id);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(AppointmentSlotQueries.GetAffixationId, parameters);
            return result;
        }
        public async Task<dynamic> GetHolidays()
        {
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(AppointmentSlotQueries.GetHolidays);
            return result;

        }
        public async Task<dynamic> CheckAppointmentDate(string OrderType,dynamic vehicledetails, dynamic userdetails, dynamic DealerAppointment)
        {
            var dealerId = DealerAppointment.DealerAffixationCenterId;
            var OemId = userdetails.OemId;
            var affixationcentreId = DealerAppointment.DealerAffixationCenterId;
            var vehicleTypeId = userdetails.VehicleTypeId;
            var dealerPoint = DealerAppointment.DeliveryPoint;
            var stateId = vehicledetails.StateId;
            var nonHomo = vehicledetails.NonHomo;
            var parameters =new  DynamicParameters();
            parameters.Add("@OemId", OemId);
            parameters.Add("@DealerId", affixationcentreId);
            parameters.Add("@VehicleTypeId", vehicleTypeId);
            parameters.Add("@DeliveryPoint", dealerPoint);
            parameters.Add("@StateId", stateId);
            parameters.Add("@NonHomo", nonHomo);
            parameters.Add("@OrderType", OrderType);

            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(AppointmentSlotQueries.CheckAppointmentDate, parameters);
            return result;

        }
        public async  Task<dynamic> AppointmentBlockedDates(string tempdate, string dealerId, dynamic dealiveryPoint)
        {
           
            var parameters = new DynamicParameters();
            parameters.Add("@TempDate", tempdate);
            parameters.Add("@DealerId", dealerId);
            parameters.Add("@DealiveryPoint", dealiveryPoint);
            
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(AppointmentSlotQueries.AppointmentBlockedDatesQuery, parameters);
            return result;

        }
        public async Task<dynamic> AppointmentBlockedDatesForSelectedDate(string selectedDate, string dealerId, dynamic dealiveryPoint)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SelectedDate", selectedDate);
            parameters.Add("@DealerId", dealerId);
            parameters.Add("@DealiveryPoint", dealiveryPoint);

            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(AppointmentSlotQueries.AppointmentBlockedDatesForSelectedDateQuery, parameters);
            return result;
        }
        public async Task<dynamic> CheckAppointmentSlotTime(string selectedDate, string vehicleTypeId, string dealerId, string dealiveryPoint, string stateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SelectedDate", selectedDate);
            parameters.Add("@VehicleTypeId", vehicleTypeId);
            parameters.Add("@DealerId", dealerId);
            parameters.Add("@DealiveryPoint", dealiveryPoint);
            parameters.Add("@StateId", stateId);

            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(AppointmentSlotQueries.CheckAppointmentSlotTime, parameters);
            return result;
        }
        public async Task<dynamic> CheckAppointmentSlotTimeHome(string selectedDate, string vehicleTypeId, string dealerId, string dealiveryPoint, string stateId)
    
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SelectedDate", selectedDate);
            parameters.Add("@VehicleTypeId", vehicleTypeId);
            parameters.Add("@DealerId", dealerId);
            parameters.Add("@DealiveryPoint", dealiveryPoint);
            parameters.Add("@StateId", stateId);

            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(AppointmentSlotQueries.CheckAppointmentSlotTimeHome, parameters);
            return result;
        }
        public async Task<dynamic> AppointmentBlockedDatesForHomes(string tempdate, string dealerId, dynamic dealiveryPoint)

        {
            var parameters = new DynamicParameters();
            parameters.Add("@TempDate", tempdate);
            parameters.Add("@DealerId", dealerId);
            parameters.Add("@DealiveryPoint", dealiveryPoint);

            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(AppointmentSlotQueries.AppointmentBlockedDatesForHomes, parameters);
            return result;
        }
    }
}
