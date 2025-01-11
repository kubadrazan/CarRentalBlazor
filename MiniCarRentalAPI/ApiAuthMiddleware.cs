using MiniCarRentalAPI.Services;

namespace MiniCarRentalAPI
{

	public class ApiAuthMiddleware
	{
		private readonly RequestDelegate _next;
		private const string ApiKeyHeaderName = "X-Api-Key";
		private const string ApiClientIdHeaderName = "X-Client-Id";
		private readonly IApiKeyValidatorService _apiKeyValidatorService;

		public ApiAuthMiddleware(RequestDelegate next, IApiKeyValidatorService apiKeyValidatorService)
		{
			_next = next;
			_apiKeyValidatorService = apiKeyValidatorService;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("API Key is missing.");
				return;
			}
			if (!context.Request.Headers.TryGetValue(ApiClientIdHeaderName, out var extractedClientId))
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Client Id is missing.");
				return;
			}

			if (!_apiKeyValidatorService.IsValidApiKey(extractedApiKey, extractedClientId))
			{
				context.Response.StatusCode = StatusCodes.Status403Forbidden;
				await context.Response.WriteAsync("Invalid API Key.");
				return;
			}

			await _next(context);
		}
	}

}
