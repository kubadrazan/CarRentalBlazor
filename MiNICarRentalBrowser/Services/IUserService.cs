using SharedDataModels;

namespace MiNICarRentalBrowser.Services
{
    public interface IUserService
    {
        Task AddUserAsync(User user);
    }
}
