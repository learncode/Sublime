using System.Configuration;
using Ploeh.AutoFixture.Xunit2;
using SublimeDal.Core.Context;
using SublimeDal.Core.Entities;
using Xunit;

namespace SublimeDal.Tests
{
    public class ConnectionStringProviderTests
    {
       [Fact]
       public void GetConnectionString_throws_an_exception_when_key_is_missing() {
          ConnectionStringProvider sut = new ConnectionStringProvider();
          Assert.Throws<ConfigurationErrorsException>(() => sut.GetConnectionString("somekey"));
       }

       [Fact]
       public void GetConnectionString_throws_an_exception_when_provider_name_is_empty() {
          ConnectionStringProvider sut = new ConnectionStringProvider();
          Assert.Throws<ConfigurationErrorsException>(() => sut.GetConnectionString("NoProviderTest"));
       }

       [Fact]
       public void GetConnectionString_fetches_connection_string_when_provided_a_valid_key() {
          ConnectionStringProvider sut = new ConnectionStringProvider();
          ConnectionStringEntity connectionString = sut.GetConnectionString(@"Test");
          Assert.True(!string.IsNullOrWhiteSpace(connectionString.ConnectionString));
       }
    }
}
