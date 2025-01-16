using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Data;
using SendGrid.Helpers.Mail;
using SharedDataModels;
using System;

namespace MiniCarRentalAPI.Services
{
    public class OfferService
    {
        private readonly TimeProvider _timeProvider;
        public OfferService(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public async Task<bool> SetUserEmail(Offer offer, string emailAddress)
        {
            if (offer.ExpirationDate < _timeProvider.GetUtcNow().DateTime ||
                !string.IsNullOrEmpty(offer.UserEmail))
                return false;

            offer.UserEmail = emailAddress;

            return true;
        }

        public async Task<bool> CheckIfOfferVaild(CarRentalContext context, Guid offerGuid)
        {
            var offer = await context.Offers.FirstOrDefaultAsync(f => f.OfferGuid == offerGuid);
            return !(offer == null ||
                offer.ExpirationDate < _timeProvider.GetUtcNow().DateTime ||
                string.IsNullOrEmpty(offer.UserEmail));
        }
    }
}
