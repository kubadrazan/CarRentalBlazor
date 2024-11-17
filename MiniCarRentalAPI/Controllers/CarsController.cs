using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MiniCarRentalAPI.Data;
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
                .Include(c => c.Localization)
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(c => c.ID == id);
            //.FindAsync(id);

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
            //var models = await _context.Brands
            //    .Include(b => b.Models)
            //    .Select(b => new
            //    {
            //        b.Name,
            //        Models = b.Models.Select(m => m.Name)
            //    })
            //    .ToListAsync();

            var models = await _context.Models
                .Include(m => m.Brand)
                .ToListAsync();


            return Ok(models);
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<IActionResult> GetFilteredCars(
            [FromQuery] List<string> brands,
            [FromQuery] List<string> models,
            [FromQuery] int? lastId = null,
            [FromQuery] int pageSize = 5)
        {
            var query = _context.Cars.AsQueryable();

            if (brands != null && brands.Any())
                query = query.Where(car => brands.Contains(car.Model.Brand.Name));

            if (models != null && models.Any())
                query = query.Where(car => models.Contains(car.Model.Name));

            int allCount = query.Count();

            if (lastId.HasValue)
                query = query.Where(car => car.ID > lastId.Value);

            var cars = await query.OrderBy(car => car.ID).Include(c => c.Model)
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
