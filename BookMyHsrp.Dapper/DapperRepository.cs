using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMyHsrp.Dapper
{
    public class DapperRepository
    {
        //https://github.com/kndenney/dapper-database-helper/blob/master/DatabaseHelper.cs
        private readonly string? _connectionString;

        public DapperRepository(string? connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters? parameters = null, CommandType commandType = CommandType.Text)
        {
            return await WithConnection(async (connection) => await connection.QueryAsync<T>(sql, parameters, commandType: commandType, commandTimeout: 600));
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, DynamicParameters? parameters = null)
        {
            return await WithConnection(async (connection) => await connection.QuerySingleOrDefaultAsync<T>(sql, parameters));
        }

        public async Task<int> ExecuteAsync(string sql, DynamicParameters? parameters = null)
        {
            return await WithConnection(async (connection) => await connection.ExecuteAsync(sql, parameters));
        }

        public async Task<int> ExecuteStoredProcedureAsync(string procedureName, DynamicParameters? parameters = null)
        {
            return await WithConnection(async (connection) => await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure));
        }

        public async Task<DataTable> QueryAsyncDataTable(string sql, DynamicParameters? parameters = null)
        {
            return await WithConnection(async (connection) =>
            {
                using var command = new SqlCommand(sql, (SqlConnection)connection);
                if (parameters != null)
                {
                    foreach (var param in parameters.ParameterNames)
                    {
                        command.Parameters.AddWithValue(param, parameters.Get<object>(param));
                    }
                }
                using var dataReader = await command.ExecuteReaderAsync();
                var dataTable = new DataTable();
                dataTable.Load(dataReader);
                return dataTable;
            });
        }
        public async Task ExecuteTransactionAsync(IEnumerable<Action<IDbConnection, IDbTransaction>> operations)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var operation in operations)
                        {
                            operation(connection, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"An error occurred in {GetType().FullName} with exception message: {ex.Message}", ex);
                    }
                }
            }
        }

        private async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return await getData(connection);
            }
            catch (SqlException ex)
            {
                throw new Exception($"An error occurred in {GetType().FullName} with exception message: {ex.Message}", ex);
            }
        }
    }
}
