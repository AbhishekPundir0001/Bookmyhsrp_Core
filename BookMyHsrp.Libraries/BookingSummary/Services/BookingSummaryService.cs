using BookMyHsrp.Dapper;
using BookMyHsrp.Libraries.AppointmentSlot.Queries;
using BookMyHsrp.Libraries.BookingSummary.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.BookingSummary.Services
{
    public class BookingSummaryService:IBookingSummaryService
    {
        private readonly DapperRepository _databaseHelperPrimary;
        private readonly string _connectionString;
        private readonly DapperRepository _databaseHelper;
        public BookingSummaryService(IOptionsSnapshot<ConnectionString> connectionStringOptions, IOptionsSnapshot<DynamicDataDto> dynamicData)
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;

            _databaseHelper = new DapperRepository(_connectionString);
            _connectionString = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelperPrimary = new DapperRepository(_connectionString);
        }
        public async Task<dynamic> BookingSummaryConfirmation(dynamic DealerAppointment)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DealerAffixationId", DealerAppointment.DealerAffixationCenterId);
            var result = await _databaseHelperPrimary.QueryAsync<dynamic>(BookingSummaryQueries.BookingSummaryConfirmation, parameters);
            return result;
        }
    }
}
