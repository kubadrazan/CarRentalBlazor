using Microsoft.Data.SqlClient;
using SharedDataModels;
using System.Text.Json;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using System.Reflection.PortableExecutable;

namespace DatabaseIntegrationTesting
{
    public class SqlServerTests : IClassFixture<CustomWebApplicationFactory<MiniCarRentalAPI.Program>>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly CustomWebApplicationFactory<MiniCarRentalAPI.Program> _factory;

        public SqlServerTests(ITestOutputHelper testOutputHelper, CustomWebApplicationFactory<MiniCarRentalAPI.Program> factory)
        {
            this._testOutputHelper = testOutputHelper;
            this._factory = factory;
        }

        [Fact]
        public void Can_Select_1_From_Database()
        {
            using var db = _factory.CreateThrowawayDb();

            _testOutputHelper.WriteLine($"Created database {db.Name}");

            using var connection = new SqlConnection(db.ConnectionString);
            connection.Open();
            using var cmd = new SqlCommand("SELECT 1", connection);
            var result = Convert.ToInt32(cmd.ExecuteScalar());

            _testOutputHelper.WriteLine(result.ToString());

            Assert.Equal(1, result);
        }
    }

}
