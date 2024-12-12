using SharedDataModels;

namespace MiNICarRentalBrowser.Services
{
    public interface IUserService
    {
        Task AddUserAsync(User user);

        Task<List<RentalBrowser>> GetUsersRentals(string email, int? lastRentalId = null, int pageSize = 15);
    }
}
