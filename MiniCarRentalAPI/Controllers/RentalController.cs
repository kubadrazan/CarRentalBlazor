using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Data;
using MiniCarRentalAPI.Services;
using SharedDataModels;

namespace MiniCarRentalAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RentalController : ControllerBase
	{
		private readonly CarRentalContext _context;
		private readonly EmailService _emailService;


		public RentalController(CarRentalContext context, EmailService emailService)
		{
			_context = context;
			_emailService = emailService;
		}

		// generate one offer based on metadata
		// GET: api/Cars/offers/5
		//[HttpGet("offer/")]
		//public async Task<IActionResult> GetCarOffer(
		//	[FromQuery] int carId,
		//	[FromQuery] bool isInsurance,
		//	[FromQuery] int userId)
		//{
		//	var offer = await _context.Cars
		//		.Select(c => new Offer()
		//		{
		//                  OfferHashID = new Random().Next(1_000_000),
		//			CarId = carId,
		//			IsInsurance = isInsurance,
		//			Price = isInsurance ? c.InsurancePricePerDay : c.PricePerDay,
		//			ExpirationDate = DateTime.UtcNow.AddMinutes(10),
		//			UserID = userId
		//		})
		//		.FirstOrDefaultAsync(c => c.CarId == carId);

		//	if (offer == null)
		//	{
		//		return NotFound();
		//	}

		//	return Ok(offer);
		//}


		[HttpGet("offers/{carId}")]
		public async Task<IActionResult> GetCarOffers(int carId
			)
		{
			var car = await _context.Cars.FirstOrDefaultAsync(c => c.ID == carId);

			if (car == null)
			{
				return NotFound();
			}


			var offers = new List<Offer> {
					new Offer()
					{
						OfferHashID = new Random().Next(1_000_000),
						CarId = carId,
						IsInsurance = true,
						Price = car.InsurancePricePerDay,
						ExpirationDate = DateTime.UtcNow.AddMinutes(10),
						UserEmail = null
					},
					new Offer()
					{
						OfferHashID = new Random().Next(1_000_000),
						CarId = carId,
						IsInsurance = false,
						Price = car.PricePerDay,
						ExpirationDate = DateTime.UtcNow.AddMinutes(10),
						UserEmail = null
					}
			};


			_context.Offers.Add(offers[0]);
			_context.Offers.Add(offers[1]);

			await _context.SaveChangesAsync();

			return Ok(offers);
		}

		[HttpPut("offers/chooseOffer/{offerId}")]
		public async Task<IActionResult> ChooseOffer(
			 int offerId,
			[FromBody] String emailAddress)
		{
			var offer = await _context.Offers.FirstOrDefaultAsync(f => f.ID == offerId);
			if (offer == null)
			{
				return NotFound();
			}
			offer.UserEmail = emailAddress;

			_context.Offers.Update(offer);
			await _context.SaveChangesAsync();
			var car = await _context.Cars
				//.Include(c => c.Localization)
				.Include(c => c.Model)
				.ThenInclude(m => m.Brand)
				.FirstOrDefaultAsync(c => c.ID == offer.CarId);
			_emailService.SendConfirmationEmail(offer, car);
			return Ok($"Sent offer {offer.OfferHashID} to  '{emailAddress}'.");
		}

		[HttpPut("acceptOffer")]
		public async Task<IActionResult> AcceptOffer([FromBody] int offerId)
		{
			var offer = await _context.Offers.FirstOrDefaultAsync(f => f.OfferHashID == offerId);

			if (offer == null || offer.UserEmail is null)
			{
				return NotFound();
			}

			var rental = new Rental
			{
				RentDate = DateTime.UtcNow,
				CarID = offer.CarId,
				UserEmail = offer.UserEmail,
				SourceAPI = 0,
				PricePerDay = offer.Price,
				IsInsurance = offer.IsInsurance
			};

			_context.Rentals.Add(rental);
			await _context.SaveChangesAsync();

			return Ok(rental);
		}

        [HttpPut("returnCar/{rentalId}")]
        public async Task<IActionResult> ReturnCar(int rentalId,
            [FromBody] String emailAddress,
            [FromBody] float latitude,
            [FromBody] float longitude)
        {
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.ID == rentalId);

            if (rental == null)
            {
                return NotFound();
            }

            var carReturn = new Return
            {
                ReturnDate = DateTime.UtcNow,
                RentalID = rentalId,
				Latitude = latitude,
				Longitude = longitude
            };

            _context.Returns.Add(carReturn);
            await _context.SaveChangesAsync();

            return Ok(carReturn);
        }

        [HttpPut("acceptReturn/{returnId}")]
        public async Task<IActionResult> AcceptReturn(int returnId,
            [FromBody] int employeeId,
            [FromBody] string returnDescription)
        {
            var carReturn = await _context.Returns.FirstOrDefaultAsync(r => r.ID == returnId);

            if (carReturn == null)
            {
                return NotFound();
            }

            var acceptation = new Acceptation
            {
				AcceptationDate = DateTime.UtcNow,
				ReturnID = returnId,
				EmployeeID = employeeId,
				Description = new Description
                {
                    Content = returnDescription
                }
            };

            _context.Acceptations.Add(acceptation);
            await _context.SaveChangesAsync();

            return Ok(acceptation);
        }
    }

}

