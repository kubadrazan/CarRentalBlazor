using Microsoft.AspNetCore.Authorization;
using MiNICarRentalBrowser.Services;
using System.Security.Claims;

public class RegistrationHandler : AuthorizationHandler<RegisteredUserRequirement>
{
	private readonly UserValidationService _userValidationService;

	public RegistrationHandler(UserValidationService userValidationService)
	{
		_userValidationService = userValidationService;
	}

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RegisteredUserRequirement requirement)
	{
		var userEmail = context.User.FindFirstValue(ClaimTypes.Email);

		if (!string.IsNullOrEmpty(userEmail) && await _userValidationService.IsUserRegisteredAsync(userEmail))
		{
			context.Succeed(requirement);
		}
	}
}

public class RegisteredUserRequirement : IAuthorizationRequirement { }
