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
    }
}
