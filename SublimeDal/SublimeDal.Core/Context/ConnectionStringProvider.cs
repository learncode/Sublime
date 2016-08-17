using System.Configuration;
using System.Runtime.CompilerServices;
using SublimeDal.Core.Entities;
using SublimeDal.Library;

namespace SublimeDal.Core.Context {
   public interface IDbConnectionStringProvider {
      ConnectionStringEntity GetConnectionString(string connectionStringKey);
   }

   public class ConnectionStringProvider : IDbConnectionStringProvider {
      public ConnectionStringEntity GetConnectionString(string connectionStringKey) {
         Require.NotNullOrEmptyString(connectionStringKey, @"Connection string cannot be empty.");
         Require.NotNull(ConfigurationManager.ConnectionStrings, "Please provide a config file with a connection strings entry");

         ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringKey];

         if (settings == null)
            throw new ConfigurationErrorsException(string.Format("Cannot find the connection string: [{0}] in the config file.", connectionStringKey));

         if (string.IsNullOrWhiteSpace(settings.ConnectionString) || string.IsNullOrWhiteSpace(settings.ProviderName))
            throw new ConfigurationErrorsException(string.Format("Require connection string as well as provider name to instantiate connection for Key [{0}].", connectionStringKey));

         return new ConnectionStringEntity() {ConnectionString = settings.ConnectionString, ProviderName = settings.ProviderName};
      }
   }
}
