using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SharedDataModels;
using System.Text.Json;
using ThrowawayDb;
using Xunit.Abstractions;
using Azure.Core;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using System.Net.Http.Json;
using System.Text;

namespace DatabaseIntegrationTesting
{
    public class SqlServerTests : IClassFixture<CustomWebApplicationFactory<MiniCarRentalAPI.Program>>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly CustomWebApplicationFactory<MiniCarRentalAPI.Program> _factory;
        private readonly IConfigurationRoot _config;

        public SqlServerTests(ITestOutputHelper testOutputHelper, CustomWebApplicationFactory<MiniCarRentalAPI.Program> factory)
        {
            this._testOutputHelper = testOutputHelper;
            this._factory = factory;

            var config = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(@"..\..\..\..\DatabaseIntegrationTesting"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            var builtConfig = config.Build();
            config.AddAzureKeyVault(
                new Uri(builtConfig.GetValue<string>("KeyVault:https")),
                new DefaultAzureCredential()
                );
            _config = config.Build();
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

        [Theory]
        [InlineData("/api/Cars/0")]
        public async Task GetCarTest(string url)
        {
            // Arrange
            using var db = _factory.CreateThrowawayDb();
            var client = _factory.CreateClientForDatabase(db);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Api-Key", _config["aApiKey"]);
            request.Headers.Add("X-Client-Id", "MiNICarRentalBrowser");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            //var str = await response.Content.ReadAsStringAsync();
            //Assert.Equal("", str);
            var result = await JsonSerializer.DeserializeAsync<Car>(response.Content.ReadAsStream(),
                new JsonSerializerOptions()
            {
                    PropertyNameCaseInsensitive = true
            });
            Assert.NotNull(result);
            Assert.Equal(2016, result.ProductionYear);
            Assert.Equal(150, result.HorsePower);
            Assert.Equal("black", result.Colour);
            Assert.Equal(52.22791496537448, result.Location.Latitude, 0.0001);
            Assert.Equal(21.005406856024276, result.Location.Longitude, 0.0001);
            Assert.Equal("Alfa Romeo", result.Model.Brand.Name);
            Assert.Equal("Giulia", result.Model.Name);
        }
    }

}
