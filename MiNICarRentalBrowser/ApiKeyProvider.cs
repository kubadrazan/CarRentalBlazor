using MiNICarRentalBrowser.Data;

namespace MiNICarRentalBrowser
{
    public class ApiKeyProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiA;

        public ApiKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiA = configuration.GetValue<string>("ApiUrls:ApiRentalA") ?? throw new Exception("No apiA Url in configuration file!");
        }

        public string GetApiKeyAsync(string apiUrl)
        {
            string keyName = string.Empty;
            if (_apiA == apiUrl)
                keyName = "aApiKey";
            else
                throw new KeyNotFoundException($"No key found for Url {apiUrl}");

            return _configuration[keyName] ?? throw new KeyNotFoundException($"No key found for good Url {apiUrl}");
        }
    }
}
