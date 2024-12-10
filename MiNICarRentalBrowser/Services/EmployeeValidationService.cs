using Microsoft.EntityFrameworkCore;
using MiNICarRentalBrowser.Data;

namespace MiNICarRentalBrowser.Services
{
	public class EmployeeValidationService
	{
		private readonly UsersContext _usersContext;

		public EmployeeValidationService(UsersContext usersContext)
		{
			_usersContext = usersContext;
		}

		public async Task<bool> IsUserAnEmployee(string userEmail)
		{
			if (string.IsNullOrEmpty(userEmail)) return false;

			return await _usersContext.Employees.AnyAsync(u => u.Email == userEmail);
		}
	}

}
