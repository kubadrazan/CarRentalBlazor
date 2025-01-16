using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniCarRentalAPI.Data;
using MiniCarRentalAPI.Services;
using SharedDataModels;
using SharedDataModels.Factories;
using SharedDataModels.Requests;
using System;

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
        private readonly AzureBlobService _azureBlobService;
        private readonly TimeProvider _timeProvider;
        private readonly OfferService _offerService;
        private readonly CarService _carService;
        private readonly RentalService _rentalService;

        public RentalController(CarRentalContext context, EmailService emailService, OfferFactory offerFactory, RentalFactory rentalFactory, ReturnFactory returnFactory, AcceptationFactory acceptationFactory, AzureBlobService azureBlobService, TimeProvider timeProvider, OfferService offerService, CarService carService, RentalService rentalService)
        {
            _context = context;
            _emailService = emailService;
            _offerFactory = offerFactory;
            _rentalFactory = rentalFactory;
            _returnFactory = returnFactory;
            _acceptationFactory = acceptationFactory;
            _azureBlobService = azureBlobService;
            _timeProvider = timeProvider;
            _offerService = offerService;
            _carService = carService;
            _rentalService = rentalService;
        }

        [HttpGet("offers/{carId}")]
        public async Task<IActionResult> GetCarOffers(int carId)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.ID == carId);

            if (car == null) return NotFound();

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
            if (offer == null) return NotFound();

            if (!(await _offerService.SetUserEmail(offer, emailAddress))) return UnprocessableEntity();

            var car = await _context.Cars
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(c => c.ID == offer.CarId);

            if (car.Availability != Availability.AVAILABLE) return UnprocessableEntity();

            _emailService.SendConfirmationEmail(offer, car);

            var rental = _rentalFactory.CreateRental(offer);

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(rental);
        }

        [HttpPut("offers/acceptOffer")]
        public async Task<IActionResult> AcceptOffer([FromBody] Guid offerGuid)
        {
            if (!(await _offerService.CheckIfOfferVaild(_context, offerGuid)))
            {
                return UnprocessableEntity();
            }

            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.OfferGuid == offerGuid);

            if (rental == null) return NotFound();

            if (!(await _rentalService.StartRental(rental))) return UnprocessableEntity();

            if (!(await _carService.ChangeCarToUnavailable(_context, rental.CarID)))
                return UnprocessableEntity();

            await _context.SaveChangesAsync();

            return Ok(rental);
        }

        [HttpPut("rentals/returnCar/{rentalId}")]
        public async Task<IActionResult> ReturnCar(int rentalId,
            [FromBody] string email)
        {
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.ID == rentalId);

            if (rental == null) return NotFound();

            if (rental.UserEmail != email) return BadRequest();

            var carReturn = _returnFactory.CreateReturn(rentalId);
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

            if (carReturn == null) return NotFound();

            string blobUri = await _azureBlobService.Upload(request.Base64EncodedCarImage);

            var acceptation = _acceptationFactory.CreateAcceptation(carReturn, request.EmployeeEmail, request.ReturnDescription, blobUri);

            var rental = await _context.Rentals
                .Include(r => r.Car)
                .ThenInclude(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(r => r.ID == rentalId);

            if (rental == null) return NotFound();

            rental.RentalStatus = RentalStatus.CLOSED;

            if (!(await _carService.ChangeCarToAvailable(_context, rental.CarID)))
                return UnprocessableEntity();

            _context.Acceptations.Add(acceptation);
            _emailService.SendInvoice(rental);
            await _context.SaveChangesAsync();

            return Ok(acceptation);
        }

        [HttpGet("rentals/{rentalId}/{email}")]
        public async Task<IActionResult> GetRental(int rentalId, string email)
        {
            var rental = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Car.Model)
                .Include(r => r.Car.Model.Brand)
                .FirstOrDefaultAsync(r => r.ID == rentalId && r.UserEmail == email);

            if (rental == null) return NotFound();

            return Ok(rental);
        }

        [HttpGet("rentals/allRentals/{email}")]
        public async Task<IActionResult> GetAllRental(string email)
        {
            var rental = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Car.Model)
                .Include(r => r.Car.Model.Brand)
                .Where(r => r.UserEmail == email)
                .ToListAsync();

            if (rental == null) return NotFound();

            return Ok(rental);
        }

        [HttpGet("rentals/admin/{rentalId}")]
        public async Task<IActionResult> GetRental(int rentalId)
        {
            var rental = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Car.Model)
                .Include(r => r.Car.Model.Brand)
                .FirstOrDefaultAsync(r => r.ID == rentalId);

            if (rental == null) return NotFound();

            return Ok(rental);
        }

        [HttpGet("rentalStatus/{rentalId}")]
        public async Task<IActionResult> GetRentalStatus(int rentalId)
        {
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.ID == rentalId);

            if (rental == null) return NotFound();

            return Ok(rental.RentalStatus);
        }

        [HttpGet("rentals/count")]
        public IActionResult GetRentalsCount()
        {
            var query = _context.Rentals.AsQueryable();

            query = query.Where(r => r.RentalStatus != RentalStatus.NOT_ACCEPTED);

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

            query = query.Where(r => r.RentalStatus != RentalStatus.NOT_ACCEPTED);
            query = query.OrderByDescending(r => r.ID);
            query = query.Skip((pageInd - 1) * pageSize);
            rentals = await query.Include(r => r.Car).Include(r => r.Car.Model).
                Include(r => r.Car.Model.Brand).Take(pageSize).ToListAsync();

            return Ok(rentals);
        }
    }

}

