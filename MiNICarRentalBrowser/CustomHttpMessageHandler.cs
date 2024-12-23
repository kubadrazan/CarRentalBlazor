namespace MiNICarRentalBrowser
{
	public class CustomHttpMessageHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) // TODO Different acction for each api
		{
			request.Headers.Add("X-Api-Key", "1"); // TODO get ApiKey from vault

			Console.WriteLine($"Request URI: {request.RequestUri}");

			var response = await base.SendAsync(request, cancellationToken);


			return response;
		}
	}
}