using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Data;
using SharedDataModels;

namespace MiniCarRentalAPI.Controllers
{
    public class RentalController : ControllerBase
    {
        private readonly CarRentalContext _context;

        public RentalController(CarRentalContext context)
        {
            _context = context;
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

        [HttpGet("offers/{carId}")]
        public async Task<IActionResult> GetCarOffers(int carId)
        {
            var offers = await _context.Cars
                .Select(c => new List<Offer> {
                    new Offer()
                    {
                        ID = new Random().Next(1_000_000),
                        CarId = carId,
                        IsInsurance = true,
                        Price = c.InsurancePricePerDay
                    },
                    new Offer()
                    {
                        ID = new Random().Next(1_000_000),
                        CarId = carId,
                        IsInsurance = false,
                        Price = c.PricePerDay
                    }
                })
                .FirstOrDefaultAsync(c => c[0].CarId == carId);

            if (offers == null)
            {
                return NotFound();
            }

            return Ok(offers);
        }
    }
}
