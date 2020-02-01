using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace VPMDataManager.Library.Internal.DataAccess
{
    public class SQLDataAccess : IDisposable, ISQLDataAccess
    {

        public SQLDataAccess(IConfiguration config, ILogger<SQLDataAccess> logger )
        {
            _config = config;
            _logger = logger;
        }


        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                List<T> rows = conn.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();

                return rows;


            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection conn = new SqlConnection(connectionString))
            {
                conn.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            }
        }


        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public void StartTransaction(string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            isClosed = false;

        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {

            List<T> rows = _connection
            .Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction)
            .ToList();

            return rows;

        }

        private bool isClosed = false;
        private readonly IConfiguration _config;
        private readonly ILogger<SQLDataAccess> _logger;

        public void CommitTransaction()
        {

            _transaction?.Commit();
            _connection?.Close();
            isClosed = true;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            isClosed = true;
        }

        public void Dispose()
        {
            if (!isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CommitTransaction falló en el método Dispose.");
                }
            }

            _transaction = null;
            _connection = null;
        }
    }
}
