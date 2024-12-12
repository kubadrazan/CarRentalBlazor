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

        public async Task<List<RentalBrowser>> GetUsersRentals(User user, int? lastRentalId = null, int pageSize = 15)
        {
            if (pageSize <= 0)
                return new List<RentalBrowser>();

            var query = _context.Rentals.AsQueryable();

            query = query.Where(rental => user.ID == rental.UserID);

            if (lastRentalId != null && lastRentalId > -1)
                query = query.Where(rental => rental.ID > lastRentalId);
            return await query.OrderByDescending(rental => rental.RentDate).Take(pageSize).ToListAsync();
        }

        public async Task<RentalBrowser> GetRentalBrowserAsync(int rentalBrowserId)
        {
            return await _context.Rentals.FirstOrDefaultAsync(r => r.ID == rentalBrowserId);
		}
	}
}
