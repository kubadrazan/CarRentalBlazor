using Microsoft.EntityFrameworkCore;
using MiNICarRentalBrowser.Data;

namespace MiNICarRentalBrowser.Services
{
	public class UserValidationService
	{
		private readonly UsersContext _usersContext;

		public UserValidationService(UsersContext usersContext)
		{
			_usersContext = usersContext;
		}

		public async Task<bool> IsUserRegisteredAsync(string userEmail)
		{
			if (string.IsNullOrEmpty(userEmail)) return false;

			return await _usersContext.Users.AnyAsync(u => u.Email == userEmail);
		}
	}

}
