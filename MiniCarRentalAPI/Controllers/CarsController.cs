using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MiniCarRentalAPI.Data;
using NuGet.Versioning;
using SharedDataModels;
using SharedDataModels.DTO;

namespace MiniCarRentalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarRentalContext _context;

        public CarsController(CarRentalContext context)
        {
            _context = context;
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        // GET: api/Cars/brands
        [HttpGet("brands")]
        public async Task<IActionResult> GetUniqueBrands()
        {
            var brands = await _context.Cars
                .Select(c => c.Model.Brand.Name)
                .Distinct()
                .ToListAsync();

            return Ok(brands);
        }

        // GET: api/Cars/models
        [HttpGet("models")]
        public async Task<IActionResult> GetUniqueModels()
        {
            var models = await _context.Cars
                .Select(c => c.Model.Name)
                .Distinct()
                .ToListAsync();

            return Ok(models);
        }

        // GET: api/Cars/brandsModels
        [HttpGet("brandsModels")]
        public async Task<IActionResult> GetBrandsModels()
        {

            var models = await _context.Cars
                .Select(c => c.Model)
                .Distinct()
                .Include(b => b.Brand)
                .Select(b => new BrandModelDTO
                {
                    BrandName = b.Brand.Name,
                    ModelName = b.Name
                })
                .ToListAsync();

            return Ok(models);
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<IActionResult> GetFilteredCars(
            [FromQuery] List<string> brands,
            [FromQuery] List<string> models,
            [FromQuery] int pageInd = 1,
            [FromQuery] int pageSize = 1,
            [FromQuery] bool onlyAvailable = true)
        {
            if (pageInd < 1 || pageSize < 1)
                return NotFound();

            var query = _context.Cars.AsQueryable();

            if (brands != null && brands.Any())
                query = query.Where(car => brands.Contains(car.Model.Brand.Name));

            if (models != null && models.Any())
                query = query.Where(car => models.Contains(car.Model.Name));

            if (onlyAvailable)
                query = query.Where(car => car.Availability == Availability.AVAILABLE);

            int allCount = query.Count();

            List<Car>? cars;
            query = query.OrderBy(car => car.ID);
            query = query.Skip((pageInd - 1) * pageSize);
            cars = await query.Include(c => c.Model)
                .ThenInclude(m => m.Brand).Take(pageSize).ToListAsync();

            var result = new PagedCarsResponse() { Cars = cars, TotalCount = allCount };

            return Ok(result);
        }

        // POST: api/Cars
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCar), new { id = car.ID }, car);
        }

        // PUT: api/Cars/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.ID)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);  // Usunięcie samochodu z bazy danych
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
