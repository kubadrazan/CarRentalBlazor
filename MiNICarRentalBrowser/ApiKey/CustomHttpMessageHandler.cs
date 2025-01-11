using MiNICarRentalBrowser.ApiKey;

namespace MiNICarRentalBrowser
{
	public class CustomHttpMessageHandler : DelegatingHandler
	{
		private readonly ApiKeyProvider _apiKeyProvider;

		public CustomHttpMessageHandler(ApiKeyProvider apiKeyProvider) 
		{
			_apiKeyProvider = apiKeyProvider;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string baseUrl = request.RequestUri?.GetLeftPart(UriPartial.Authority) ?? throw new InvalidOperationException("Invalid request URI");
			string apiKey  = _apiKeyProvider.GetApiKeyAsync(baseUrl);
			string clientId = "MiNICarRentalBrowser";
			request.Headers.Add("X-Api-Key", apiKey);
			request.Headers.Add("X-Client-Id", clientId);

			Console.WriteLine($"Request URI: {request.RequestUri}");

			var response = await base.SendAsync(request, cancellationToken);

			return response;
		}
	}
}