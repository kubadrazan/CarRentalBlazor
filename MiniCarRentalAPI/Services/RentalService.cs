using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Data;
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
