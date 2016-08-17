using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SublimeDal.Library;

namespace SublimeDal.Core.Context {
   public interface IDbContext {
      List<T> Query<T>(string connectionStringKey, string query, List<Tuple<string, object>> parameters) where T : new();
      List<T> Procedure<T>(string connectionStringKey, string procedureName, List<Tuple<string, object, DbType, ParameterDirection>> parameters) where T : new();
      int ExecuteQuery(string connectionStringKey, string query, List<Tuple<string, object>> parameters);
   }

   public class DbContext : IDbContext {
      IConnectionProvider _connectionProvider;

      public DbContext(IConnectionProvider connectionProvider) {
         _connectionProvider = connectionProvider;
      }
      
      public List<T> Query<T>(string connectionStringKey, string query, List<Tuple<string, object>> parameters) where T : new () {
         IDbDataParameter[] dbParameters = null;
         if (parameters != null) {
            dbParameters = new IDbDataParameter[parameters.Count];
            for (int i = 0; i < parameters.Count; i++) {
               dbParameters[i] = CreateParameter(parameters[i].Item1, parameters[i].Item2);
            }
         }
         DataSet dataset = ExecuteDataSet(connectionStringKey, query, CommandType.Text, dbParameters);
         return Mapper.GetList<T>(dataset.Tables[0]);
      }

      public List<T> Procedure<T>(string connectionStringKey, string procedureName, List<Tuple<string, object, DbType, ParameterDirection>> parameters) where T : new() {
         IDbDataParameter[] dbParameters = null;
         if (parameters != null) {
            dbParameters = new IDbDataParameter[parameters.Count];
            for (int i = 0; i < parameters.Count; i++) {
               dbParameters[i] = CreateParameter(parameters[i].Item1, parameters[i].Item2, parameters[i].Item3, parameters[i].Item4);
            }
         }
         DataSet dataset = ExecuteDataSet(connectionStringKey, procedureName, CommandType.StoredProcedure, dbParameters);
         return Mapper.GetList<T>(dataset.Tables[0]);
      }

      public int ExecuteQuery(string connectionStringKey, string query, List<Tuple<string, object>> parameters) {
         int result = -1;
         IDbDataParameter[] dbParameters = null;
         if (parameters != null) {
            dbParameters = new IDbDataParameter[parameters.Count];
            for (int i = 0; i < parameters.Count; i++) {
               dbParameters[i] = CreateParameter(parameters[i].Item1, parameters[i].Item2);
            }
         }

         using(_connectionProvider) {
            using (DbConnection connection = _connectionProvider.CreateConnection(connectionStringKey)) {
               connection.Open();
               using (DbCommand command = connection.CreateCommand()) {
                  command.CommandText = query;
                  if (parameters != null) {
                     command.Parameters.AddRange(dbParameters);
                  }
                  result = command.ExecuteNonQuery();
               }
            }
         }

         return result;
      }

      private DataSet ExecuteDataSet(string connectionStringKey, string commandText, CommandType commandType, IDbDataParameter[] parameters) {
         DataSet dataset = new DataSet();
         using (_connectionProvider) {
            using (DbConnection connection = _connectionProvider.CreateConnection(connectionStringKey)) {
               connection.Open();
               using (DbCommand command = connection.CreateCommand()) {
                  command.CommandText = commandText;
                  command.CommandType = commandType;
                  if (parameters != null) {
                     command.Parameters.AddRange(parameters);
                  }
                  DbDataAdapter adapter = _connectionProvider.Adapter;
                  adapter.SelectCommand = command;
                  adapter.Fill(dataset);
               }
            }
         }
         return dataset;
      }

      private IDbDataParameter CreateParameter(string parameterName, object value, DbType dbType, ParameterDirection direction) {
         Require.NotNullOrEmptyString(parameterName, string.Format("Invalid arguments passed to the method. Parameter name: [{0}]", "(empty)"));

         IDbDataParameter param = this._connectionProvider.CreateParameter();
         param.ParameterName = parameterName.GetParameterName();
         param.Value = value;
         param.DbType = dbType;
         param.Direction = direction;
         return param;
      }

      private IDbDataParameter CreateParameter(string parameterName, object value) {
         Require.NotNullOrEmptyString(parameterName, string.Format("Invalid arguments passed to the method. Parameter name: [{0}]", "(empty)"));

         IDbDataParameter param = this._connectionProvider.CreateParameter();
         param.ParameterName = parameterName.GetParameterName();
         param.Value = value;
         return param;
      }
   }
}
