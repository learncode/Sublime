using System;
using Moq;
using Ploeh.AutoFixture.Xunit2;
using SublimeDal.Core.Context;
using SublimeDal.Core.Entities;
using Xunit;

namespace SublimeDal.Tests {
   public class ConnectionProviderTests {
      [Fact]
      public void GetConnection_creates_a_new_connection() {
         Mock<IDbConnectionStringProvider> connectionStringProvider = new Mock<IDbConnectionStringProvider>();
         ConnectionProvider sut = new ConnectionProvider(connectionStringProvider.Object);
         ConnectionStringEntity entity = new ConnectionStringEntity() { ConnectionString = @"Data Source=.;Initial Catalog=master;", ProviderName = @"System.Data.SqlClient" };
         connectionStringProvider.Setup(csp => csp.GetConnectionString(It.IsAny<string>())).Returns(entity);
         using (var connection = sut.CreateConnection("Test")) {
            Assert.NotNull(connection);
         }
      }

      [Fact]
      public void GetConnection_sets_adapter_when_connection_is_created() {
         Mock<IDbConnectionStringProvider> connectionStringProvider = new Mock<IDbConnectionStringProvider>();
         ConnectionProvider sut = new ConnectionProvider(connectionStringProvider.Object);
         ConnectionStringEntity entity = new ConnectionStringEntity() { ConnectionString = @"Data Source=.;Initial Catalog=master;", ProviderName = @"System.Data.SqlClient" };
         connectionStringProvider.Setup(csp => csp.GetConnectionString(It.IsAny<string>())).Returns(entity);
         using (var connection = sut.CreateConnection("Test")) {
            Assert.NotNull(sut.Adapter);
         }
      }

      [Fact]
      public void GetConnection_throws_an_error_when_an_invalid_provider_is_passed() {
         Mock<IDbConnectionStringProvider> connectionStringProvider = new Mock<IDbConnectionStringProvider>();
         ConnectionProvider sut = new ConnectionProvider(connectionStringProvider.Object);
         ConnectionStringEntity entity = new ConnectionStringEntity() { ConnectionString = @"Data Source=.;Initial Catalog=master;", ProviderName = @"abc" };
         connectionStringProvider.Setup(csp => csp.GetConnectionString(It.IsAny<string>())).Returns(entity);

         Assert.Throws<ArgumentException>(() => sut.CreateConnection("Test"));
      }      

      [Fact]
      public void GetConnection_fetch_adapter_throws_exception_when_connection_is_not_set() {
         Mock<IDbConnectionStringProvider> connectionStringProvider = new Mock<IDbConnectionStringProvider>();
         ConnectionProvider sut = new ConnectionProvider(connectionStringProvider.Object);
         Assert.Throws<ObjectDisposedException>( () => sut.Adapter);
      }
   }   
}
