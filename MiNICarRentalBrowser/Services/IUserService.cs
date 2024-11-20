using SharedDataModels;

namespace MiNICarRentalBrowser.Services
{
    public interface IUserService
    {
        Task AddUser(User user);
    }
}
