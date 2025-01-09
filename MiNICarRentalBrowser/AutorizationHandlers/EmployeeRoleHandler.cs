
using Microsoft.AspNetCore.Authorization;
using MiNICarRentalBrowser.Services;
using System.Security.Claims;

public class EmployeeRoleHandler : AuthorizationHandler<EmployeeRoleRequirement>
{
	private readonly EmployeeValidationService _employeeValidationService;

	public EmployeeRoleHandler(EmployeeValidationService employeeValidationService)
	{
		_employeeValidationService = employeeValidationService;
	}

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeeRoleRequirement requirement)
	{
		var userEmail = context.User.FindFirstValue(ClaimTypes.Email);

		if (!string.IsNullOrEmpty(userEmail) && await _employeeValidationService.IsUserAnEmployee(userEmail))
		{
			context.Succeed(requirement);
		}
	}
}
public class EmployeeRoleRequirement : IAuthorizationRequirement { }


