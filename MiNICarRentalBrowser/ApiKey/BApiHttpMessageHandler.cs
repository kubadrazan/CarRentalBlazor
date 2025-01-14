using MiNICarRentalBrowser.ApiKey;

namespace MiNICarRentalBrowser
{
	public class BApiHttpMessageHandler : DelegatingHandler
	{
		private readonly ApiKeyProvider _apiKeyProvider;
		private readonly string _apiKeyName = "bApiKey";

		public BApiHttpMessageHandler(ApiKeyProvider apiKeyProvider) 
		{
			_apiKeyProvider = apiKeyProvider;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string apiKey = _apiKeyProvider.GetApiKeyAsync(_apiKeyName);
			request.Headers.Add("X-Api-Key", apiKey);

			Console.WriteLine($"Request URI: {request.RequestUri}");

			var response = await base.SendAsync(request, cancellationToken);

			return response;
		}
	}
}