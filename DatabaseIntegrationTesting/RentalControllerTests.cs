using Microsoft.Data.SqlClient;
using SharedDataModels;
using System.Text.Json;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using System.Reflection.PortableExecutable;

namespace DatabaseIntegrationTesting
{
    public class RetnalControllerTests : IClassFixture<CustomWebApplicationFactory<MiniCarRentalAPI.Program>>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly CustomWebApplicationFactory<MiniCarRentalAPI.Program> _factory;
        private readonly IConfigurationRoot _config;

        public RetnalControllerTests(ITestOutputHelper testOutputHelper, CustomWebApplicationFactory<MiniCarRentalAPI.Program> factory)
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

        [Theory]
        [InlineData("/api/Rental/offers/0")]
        public async Task GetCarTest(string url)
        {
            // Arrange
            using var db = _factory.CreateThrowawayDb();
            var client = _factory.CreateClientForDatabase(db);

            HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, url);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var result = await JsonSerializer.DeserializeAsync<List<Offer>>(response.Content.ReadAsStream(),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("X-Api-Key", _config["aApiKey"]);
            request.Headers.Add("X-Client-Id", "MiNICarRentalBrowser");

            return request;
        }
    }

}
