using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MiniCarRentalAPI.Data;
using SendGrid.Helpers.Mail;
using SharedDataModels;

namespace MiniCarRentalAPI.Services
{
    public class RentalService
    {
        private readonly TimeProvider _timeProvider;
        public RentalService(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public async Task<Rental> GetRentalWithCarAsync(CarRentalContext context, int rentalId)
            => await context.Rentals
                .Include(r => r.Car)
                .ThenInclude(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(r => r.ID == rentalId);

        public async Task<List<Rental>> GetAllUserRentalsWithCarAsync(CarRentalContext context, string email)
            => await context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Car.Model)
                .Include(r => r.Car.Model.Brand)
                .Where(r => r.UserEmail == email)
                .ToListAsync();


        public async Task<bool> StartRental(CarRentalContext context, Guid offerGuid)
        {
            var rental = await context.Rentals.FirstOrDefaultAsync(r => r.OfferGuid == offerGuid);

            if (rental == null)
            {
                return false;
            }

            return await StartRental(rental);
        }

        public async Task<bool> StartRental(Rental rental)
        {
            if (rental.UserEmail is null)
            {
                return false;
            }

            rental.RentalStatus = RentalStatus.ACTIVE;
            rental.RentDate = _timeProvider.GetUtcNow().DateTime;

            return true;
        }
    }
}
