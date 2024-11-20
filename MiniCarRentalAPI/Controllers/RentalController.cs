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
        [HttpGet("offers/")]
        public async Task<IActionResult> GetCarOffer(
            [FromQuery] int carId,
            [FromQuery] bool isInsurance)
        {
            var offer = await _context.Cars
                .Select(c => new Offer() {
                    ID = new Random().Next(1_000_000),
                    CarId = carId,
                    IsInsurance = isInsurance,
                    Price = isInsurance ? c.InsurancePricePerDay : c.PricePerDay
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

	}
}
