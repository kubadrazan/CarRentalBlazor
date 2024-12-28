using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Data;
using MiniCarRentalAPI.Services;
using SharedDataModels;
using SharedDataModels.Factories;

namespace MiniCarRentalAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RentalController : ControllerBase
	{
		private readonly CarRentalContext _context;
		private readonly EmailService _emailService;
		private readonly OfferFactory _offerFactory;
		private readonly RentalFactory _rentalFactory;
		private readonly ReturnFactory _returnFactory;
		private readonly AcceptationFactory _acceptationFactory;

		public RentalController(CarRentalContext context, EmailService emailService, OfferFactory offerFactory, RentalFactory rentalFactory, ReturnFactory returnFactory, AcceptationFactory acceptationFactory)
		{
			_context = context;
			_emailService = emailService;
			_offerFactory = offerFactory;
			_rentalFactory = rentalFactory;	
			_returnFactory = returnFactory;
			_acceptationFactory = acceptationFactory;
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
		public async Task<IActionResult> GetCarOffers(int carId)
		{
			var car = await _context.Cars.FirstOrDefaultAsync(c => c.ID == carId);

			if (car == null)
			{
				return NotFound();
			}

			var offers = _offerFactory.CreateOfferList(car);

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
				.Include(c => c.Model)
				.ThenInclude(m => m.Brand)
				.FirstOrDefaultAsync(c => c.ID == offer.CarId);
			_emailService.SendConfirmationEmail(offer, car);

			return Ok($"Sent offer {offer.OfferHashID} to  '{emailAddress}'.");
		}

		[HttpPut("offers/acceptOffer")]
		public async Task<IActionResult> AcceptOffer([FromBody] int offerId)
		{
			var offer = await _context.Offers.FirstOrDefaultAsync(f => f.OfferHashID == offerId);

			if (offer == null || offer.UserEmail is null)
			{
				return NotFound();
			}

			var rental = _rentalFactory.CreateRental(offer);

			_context.Rentals.Add(rental);
			await _context.SaveChangesAsync();

			return Ok(rental);
		}

        [HttpPut("rentals/returnCar/{rentalId}")]

        public async Task<IActionResult> ReturnCar(int rentalId,
			[FromBody] ReturnCarRequest request)
        {
			var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.ID == rentalId);

            if (rental == null)
            {
                return NotFound();
            }

			var carReturn = _returnFactory.CreateReturn(rentalId, request.Latitude, request.Longitude);
			rental.RentalStatus = RentalStatus.RETURNED;

            _context.Returns.Add(carReturn);
            await _context.SaveChangesAsync();

            return Ok(carReturn);
        }

        [HttpPut("rentals/acceptReturn/{rentalId}")]
        public async Task<IActionResult> AcceptReturn(int rentalId,
			 [FromBody] AcceptReturnRequest request)
        {
			var carReturn = await _context.Returns.FirstOrDefaultAsync(r => r.RentalID == rentalId);

            if (carReturn == null)
            {
                return NotFound();
            }

			var acceptation = _acceptationFactory.CreateAcceptation(carReturn, request.EmployeeEmail, request.ReturnDescription);

            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.ID == rentalId);

            if (rental == null)
            {
                return NotFound();
            }
            rental.RentalStatus = RentalStatus.CLOSED;

            _context.Acceptations.Add(acceptation);
            await _context.SaveChangesAsync();

            return Ok(acceptation);
        }

		[HttpGet("rentals/{rentalId}")]
		public async Task<IActionResult> GetRental(int rentalId)
		{
			var rental = await _context.Rentals.Include(r => r.Car).Include(r => r.Car.Model)
				.Include(r => r.Car.Model.Brand)
				.FirstOrDefaultAsync(r => r.ID == rentalId);

			if (rental == null)
			{
				return NotFound();
			}
			await _context.SaveChangesAsync();

			return Ok(rental);
		}

        [HttpGet("rentals/count")]
        public async Task<IActionResult> GetRentalsCount()
        {
			var query = _context.Rentals.AsQueryable();

            return Ok(query.Count());
        }

        [HttpGet("rentals")]
        public async Task<IActionResult> GetRentals(
			[FromQuery] int pageInd = 1,
            [FromQuery] int pageSize = 1)
        {
			if (pageInd < 1 || pageSize < 1)
				return NotFound();

			var query = _context.Rentals.AsQueryable();
            List<Rental>? rentals;

			query = query.OrderByDescending(r => r.ID);
			query = query.Skip((pageInd - 1) * pageSize);
            rentals = await query.Include(r => r.Car).Include(r => r.Car.Model).
				Include(r => r.Car.Model.Brand).Take(pageSize).ToListAsync();

            return Ok(rentals);
        }
    }

}

