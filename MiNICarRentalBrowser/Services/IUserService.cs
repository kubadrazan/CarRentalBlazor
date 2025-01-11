using SharedDataModels;

namespace MiNICarRentalBrowser.Services
{
    public interface IUserService
    {
        Task AddUserAsync(User user);

        Task AddRentalAsync(Rental rental);

        Task<List<RentalBrowser>> GetUsersRentals(string email, int? pageInd, int pageSize = 15);

        int GetUsersRentalsCount(string email);
    }
}
