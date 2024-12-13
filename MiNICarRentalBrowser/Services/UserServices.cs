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

        public async Task<List<RentalBrowser>> GetUsersRentals(string email, int? lastRentalId = null, int pageSize = 15)
        {
            if (pageSize <= 0)
                return new List<RentalBrowser>();

            User? user = _context.Users.Where(user => user.Email == email).FirstOrDefault();

            if (user == null)
                return new List<RentalBrowser>();

            var query = _context.Rentals.AsQueryable();

            query = query.Where(rental => user.ID == rental.UserID);

            if (lastRentalId != null && lastRentalId > -1)
                query = query.Where(rental => rental.ID < lastRentalId);
            return await query.OrderByDescending(rental => rental.ID).Take(pageSize).ToListAsync();
        }

        public async Task<RentalBrowser> GetRentalBrowserAsync(int rentalBrowserId)
        {
            return await _context.Rentals.FirstOrDefaultAsync(r => r.ID == rentalBrowserId);
		}

        public int GetUsersRentalsCount(string email)
        {
            int count = 0;

            User? user = _context.Users.Where(user => user.Email == email).FirstOrDefault();

            if (user == null)
                return count;

            var query = _context.Rentals.AsQueryable();

            query = query.Where(rental => user.ID == rental.UserID);

            return query.Count();
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
