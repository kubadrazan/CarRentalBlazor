using MiNICarRentalBrowser.Data;

namespace MiNICarRentalBrowser.ApiKey
{
	public class ApiKeyProvider
	{
		private readonly IConfiguration _configuration;

		public ApiKeyProvider(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GetApiKeyAsync(string apiKeyName)
		{
			return _configuration[apiKeyName] ?? throw new KeyNotFoundException($"Key: {apiKeyName} NotFound");
		}
	}
}
