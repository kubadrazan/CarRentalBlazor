using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SharedDataModels;
using System.Text.Json;
using ThrowawayDb;
using Xunit.Abstractions;
using Azure.Core;

namespace DatabaseIntegrationTesting
{
    public class SqlServerTests: IClassFixture<CustomWebApplicationFactory<MiniCarRentalAPI.Program>>
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
            //using var database = ThrowawayDatabase.Create(
            //    SqlServerSettings.Username,
            //    SqlServerSettings.Password,
            //    SqlServerSettings.Host
            //);
            using var db = _factory.CreateThrowawayDb();

            _testOutputHelper.WriteLine($"Created database {db.Name}");

            using var connection = new SqlConnection(db.ConnectionString);
            connection.Open();
            using var cmd = new SqlCommand("SELECT 1", connection);
            var result = Convert.ToInt32(cmd.ExecuteScalar());

            _testOutputHelper.WriteLine(result.ToString());

            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData("/api/Cars/0")]
        public async Task BasicTest(string url)
        {
            // Arrange
            using var db = _factory.CreateThrowawayDb();
            var client = _factory.CreateClientForDatabase(db);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            string apiKey = "";
            request.Headers.Add("X-Api-Key", apiKey);
            request.Headers.Add("X-Client-Id", "MiNICarRentalBrowser");
            
            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299            
            var result = await JsonSerializer.DeserializeAsync<Car>(response.Content.ReadAsStream());
            Assert.NotNull(result);
        }
    }

}
