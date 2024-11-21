using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Data;
using MiniCarRentalAPI.Services;
using SharedDataModels;

namespace MiniCarRentalAPI.Controllers
{
    public class RentalController : ControllerBase
    {
        private readonly CarRentalContext _context;
		private readonly EmailService _emailService;


		public RentalController(CarRentalContext context, EmailService emailService)
        {
            _context = context;
			_emailService = emailService;
		}

        // GET: api/Cars/offers/5
        [HttpGet("offer/")]
        public async Task<IActionResult> GetCarOffer(
            [FromQuery] int carId,
            [FromQuery] bool isInsurance,
            [FromQuery] int userId)
        {
            var offer = await _context.Cars
                .Select(c => new Offer()
                {
                    OfferRadnomID = new Random().Next(1_000_000),
                    CarId = carId,
                    IsInsurance = isInsurance,
                    Price = isInsurance ? c.InsurancePricePerDay : c.PricePerDay,
                    ExpirationDate = DateTime.UtcNow.AddMinutes(10),
                    UserID = userId
                })
                .FirstOrDefaultAsync(c => c.CarId == carId);

            if (offer == null)
            {
                return NotFound();
            }

            return Ok(offer);
        }
        [HttpPost("offers/{offerId}/send-email")]
		public IActionResult SendEmail([FromRoute] int offerId, [FromBody] String emailAddress)
        {
            _emailService.SendConfirmationEmail(offerId, emailAddress);
			return Ok($"Sent offer {offerId} to  '{emailAddress}'.");
		}

        [HttpGet("offers/")]
        public async Task<IActionResult> GetCarOffers(
            [FromQuery] int carId,
            [FromQuery] int userId
            )
        {
            var offers = await _context.Cars
                .Select(c => new List<Offer> {
                    new Offer()
                    {
                        OfferRadnomID = new Random().Next(1_000_000),
                        CarId = carId,
                        IsInsurance = true,
                        Price = c.InsurancePricePerDay,
                        ExpirationDate = DateTime.UtcNow.AddMinutes(10),
                        UserID = userId
                    },
                    new Offer()
                    {
                        OfferRadnomID = new Random().Next(1_000_000),
                        CarId = carId,
                        IsInsurance = false,
                        Price = c.PricePerDay,
                        ExpirationDate = DateTime.UtcNow.AddMinutes(10),
                        UserID = userId
                    }
                })
                .FirstOrDefaultAsync(c => c[0].CarId == carId);

            if (offers == null)
            {
                return NotFound();
            }

            _context.Offers.Add(offers[0]);
            _context.Offers.Add(offers[1]);

            await _context.SaveChangesAsync();

            return Ok(offers);
        }

        [HttpPut("acceptOffer/{offerId}")]
        public async Task<IActionResult> AcceptOffer(int offerId)
        {
            var offer = await _context.Offers.FirstOrDefaultAsync(f => f.OfferRadnomID == offerId);

            if (offer == null)
            {
                return NotFound();
            }

            var rental = new Rental
            {
                RentDate = DateTime.UtcNow,
                CarID = offer.CarId,
                UserID = offer.UserID,
                SourceAPI = 0,
                PricePerDay = offer.Price,
                IsInsurance = offer.IsInsurance
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(rental);
        }

    }

	}
}
