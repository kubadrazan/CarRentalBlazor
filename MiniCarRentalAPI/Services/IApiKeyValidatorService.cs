namespace MiniCarRentalAPI.Services
{
	public interface IApiKeyValidatorService
	{
		bool IsValidApiKey(string apiKey);
	}
	public class ApiKeyValidatorService : IApiKeyValidatorService
	{
		public bool IsValidApiKey(string apiKey)
		{
			return apiKey == "1"; // TODO check if present in dbContext
		}
	}
}