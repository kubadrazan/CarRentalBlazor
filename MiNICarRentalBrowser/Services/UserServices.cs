using Microsoft.EntityFrameworkCore;
using MiNICarRentalBrowser.Data;
using SharedDataModels;
using System;

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
            return await _context.Rentals.FirstOrDefaultAsync(r => r.ID == rentalBrowserId);

		}
		public async Task<User> GetUser(string userMail)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Email == userMail);

		}
		public async Task AcceptOfferAsync(Rental rental)
		{
            
            var rentalBrowser = new RentalBrowser()
            {
                ApiID = rental.ID,
                UserID = (await GetUser(rental.UserEmail)).ID
			};
			_context.Rentals.Add(rentalBrowser);
			await _context.SaveChangesAsync();

		}

	}
}
