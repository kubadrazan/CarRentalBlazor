namespace MiniCarRentalAPI.Services
{
	public interface IApiKeyValidatorService
	{
		bool IsValidApiKey(string apiKey, string clientId);
	}
	public class ApiKeyValidatorService : IApiKeyValidatorService
	{
		private readonly IConfiguration _configuration;
		public ApiKeyValidatorService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public bool IsValidApiKey(string apiKey, string clientId)
		{
			return _configuration.GetValue<string>(clientId) == apiKey;
		}
	}
}