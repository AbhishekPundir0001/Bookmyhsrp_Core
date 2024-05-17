using BookMyHsrp.Libraries.HsrpState.Models;
using BookMyHsrp.Libraries.HsrpState.Queries;
using Microsoft.Extensions.Options;
using BookMyHsrp.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

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
        public async Task<IEnumerable<StateModels.Cities>> GetCityOfState(string StateId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@StateId", StateId);
            var result = await _databaseHelper.QueryAsync<StateModels.Cities>(StateQueries.GetAllStates);
            return result;
        }

     
     
    }
}
