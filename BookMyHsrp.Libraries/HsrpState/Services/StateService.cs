using BookMyHsrp.Libraries.HsrpState.Models;
using BookMyHsrp.Libraries.HsrpState.Queries;
using Microsoft.Extensions.Options;
using BookMyHsrp.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Libraries.HsrpState.Services
{
    public class StateService : IStateService
    {
        private readonly DapperRepository _databaseHelper;
        private readonly string _connectionString;
        public StateService(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
        }
        public async Task<IEnumerable<StateModels.Root>> GetAllStates()
        {
            var result = await _databaseHelper.QueryAsync<StateModels.Root>(StateQueries.GetAllStates);
            return result;
        }
    }
}
