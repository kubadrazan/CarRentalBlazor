using MiNICarRentalBrowser.Data;
using SharedDataModels;

namespace MiNICarRentalBrowser.Services
{
    public class UserServices : IUserService
    {
        private readonly UsersContext _context;

        public UserServices(UsersContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<RentalBrowser> GetRentalBrowserAsync(int rentalBrowserId)
        {
            throw new NotImplementedException();
        }
    }
}
