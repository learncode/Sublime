using System;
using System.Data;
using System.Data.Common;
using SublimeDal.Core.Entities;
using SublimeDal.Library;

namespace SublimeDal.Core.Context {
   public interface IConnectionProvider : IDisposable {
      DbConnection CreateConnection(string connectionStringKey);
      DbDataAdapter Adapter { get; }
      IDbDataParameter CreateParameter();
   }

   public class ConnectionProvider : IConnectionProvider {

      IDbConnectionStringProvider _connectionStringProvider;
      DbProviderFactory _dbProviderFactory;
      DbConnection _dbConnection;
      DbDataAdapter _adapter;

      public ConnectionProvider(IDbConnectionStringProvider connectionStringProvider) {
         _connectionStringProvider = connectionStringProvider;
      }

      public DbDataAdapter Adapter {
         get {
            if (_dbConnection == null)
               throw new ObjectDisposedException("Connection object does not exist. Cannot return adapter object.");
            if (_adapter == null)
               throw new ObjectDisposedException("Adapter not initialized.");
            return _adapter;
         }
      }

      public DbConnection CreateConnection(string connectionStringKey) {
         Require.NotNullOrEmptyString(connectionStringKey, "Connection string is empty.");

         ConnectionStringEntity connectionStringEntity = _connectionStringProvider.GetConnectionString(connectionStringKey);
         _dbProviderFactory = DbProviderFactories.GetFactory(connectionStringEntity.ProviderName);

         Require.NotNull(_dbProviderFactory, "Provider factory cannot be null. Please provide a valid provider name.");

         _dbConnection = _dbProviderFactory.CreateConnection();
         _dbConnection.ConnectionString = connectionStringEntity.ConnectionString;
         _adapter = _dbProviderFactory.CreateDataAdapter();

         return _dbConnection;
      }

      public IDbDataParameter CreateParameter() {
         Require.NotNull(_dbProviderFactory, "Provider factory cannot be null. Please access this method with a valid connection.");
         return _dbProviderFactory.CreateParameter();
      }

      public void Dispose() {
         CloseConnection();
         GC.SuppressFinalize(this);
      }

      protected virtual void CloseConnection() {
         try {
            if (_dbConnection != null) {
               _dbConnection.Close();
               _dbConnection.Dispose();
               _adapter.Dispose();
            }
         } finally {
            _dbConnection = null;
            _adapter = null;
         }
      }

      ~ConnectionProvider() {
         CloseConnection();
      }
   }
}
