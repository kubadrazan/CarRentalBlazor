using Microsoft.EntityFrameworkCore;
using MiNICarRentalBrowser.Data;
using SharedDataModels;
using System;

namespace MiNICarRentalBrowser.Services
{
	public class UserServices : IUserService
	{
		private readonly UsersContext _context;
        private readonly CacheManager _cacheManager;

        public UserServices(UsersContext context, CacheManager cacheManager)
        {
            _context = context;
            _cacheManager = cacheManager;
        }

        public async Task AddUserAsync(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
		}

		public async Task<List<RentalBrowser>> GetUsersRentalsAsync(string email, int? pageInd, int pageSize = 15)
		{
			if (pageInd == null || pageSize < 1 || pageInd < 1)
				return new List<RentalBrowser>();

			User? user = await GetUserAsync(email);

			if (user == null)
				return new List<RentalBrowser>();

			var query = _context.Rentals.AsQueryable();

			query = query.Where(rental => user.ID == rental.UserID);

			query = query.OrderByDescending(rental => rental.ID);

			return await query.Skip(((int)pageInd - 1) * pageSize).Take(pageSize).ToListAsync();
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

		public async Task<User> GetUserAsync(string userMail)
		{
			var user = await _cacheManager.GetUserAsync(userMail);
			if (user != null)
				return user;

			user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userMail);

			if (user != null)
				await _cacheManager.SetUserAsync(user);

            return user;
		}

		public async Task AddRentalAsync(int apiId, int rentalId, int userId)
		{

			var rentalBrowser = new RentalBrowser(userId, rentalId, apiId);
			_context.Rentals.Add(rentalBrowser);
			await _context.SaveChangesAsync();

		}

	}
}
