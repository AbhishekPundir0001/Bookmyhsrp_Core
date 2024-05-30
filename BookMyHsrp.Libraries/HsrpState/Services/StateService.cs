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
        private readonly DapperRepository _databaseHelperPrimary;
        private readonly string _connectionString;
        private readonly string _primaryDatabaseHO;
        public StateService(IOptionsSnapshot<ConnectionString> connectionStringOptions)
        {
            _connectionString = connectionStringOptions.Value.SecondaryDatabaseHO;
            _primaryDatabaseHO = connectionStringOptions.Value.PrimaryDatabaseHO;
            _databaseHelper = new DapperRepository(_connectionString);
            _databaseHelperPrimary = new DapperRepository(_primaryDatabaseHO);
        }
        public async Task<IEnumerable<StateModels.Root>> GetAllStates()
        {
            var result = await _databaseHelper.QueryAsync<StateModels.Root>(StateQueries.GetAllStates);
            return result;
        }
        public async Task<IEnumerable<StateModels.Cities>> GetCityOfState(int StateId)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@HsrpStateid", StateId);
            var result = await _databaseHelperPrimary.QueryAsync<StateModels.Cities>(StateQueries.GetCities, parameter);
            return result;
        }

     
     
    }
}










