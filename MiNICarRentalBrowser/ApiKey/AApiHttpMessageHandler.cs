using MiNICarRentalBrowser.ApiKey;

namespace MiNICarRentalBrowser
{
	public class AApiHttpMessageHandler : DelegatingHandler
	{
		private readonly ApiKeyProvider _apiKeyProvider;
		private readonly string _apiKeyName = "aApiKey";

		public AApiHttpMessageHandler(ApiKeyProvider apiKeyProvider) 
		{
			_apiKeyProvider = apiKeyProvider;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string apiKey  = _apiKeyProvider.GetApiKeyAsync(_apiKeyName);
			string clientId = "MiNICarRentalBrowser";
			request.Headers.Add("X-Api-Key", apiKey);
			request.Headers.Add("X-Client-Id", clientId);

			Console.WriteLine($"Request URI: {request.RequestUri}");

			var response = await base.SendAsync(request, cancellationToken);

			return response;
		}
	}
}