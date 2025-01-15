using SharedDataModels;

namespace MiNICarRentalBrowser.Services
{
	public interface IUserService
	{
		Task AddUserAsync(User user);

		Task AddRentalAsync(int sourceApiId, int rentalId, int userId);
		Task<User> GetUserAsync(string userMail);

		Task<List<RentalBrowser>> GetUsersRentalsAsync(string email, int? pageInd, int pageSize = 15);

		int GetUsersRentalsCount(string email);
	}
}
