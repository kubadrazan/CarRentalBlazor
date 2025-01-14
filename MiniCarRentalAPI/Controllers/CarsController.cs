using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MiniCarRentalAPI.Data;
using NuGet.Versioning;
using PdfSharp;
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
                .Where(c => c.Availability == Availability.AVAILABLE)
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
            [FromQuery] int pageSize = 1)
        {
            if (pageInd < 1 || pageSize < 1)
                return NotFound();

            var query = _context.Cars.AsQueryable();

            if (brands != null && brands.Any())
                query = query.Where(car => brands.Contains(car.Model.Brand.Name));

            if (models != null && models.Any())
                query = query.Where(car => models.Contains(car.Model.Name));

            query = query.Where(car => car.Availability == Availability.AVAILABLE);

            int allCount = query.Count();

            List<Car>? cars;
            query = query.OrderBy(car => car.ID);
            query = query.Skip((pageInd - 1) * pageSize);
            cars = await query.Include(c => c.Model)
                .ThenInclude(m => m.Brand).Take(pageSize).ToListAsync();

            var carsDTO = new List<SimpleCarDTO>();
            // TODO add mapper
            foreach (var car in cars)
            {
                carsDTO.Add(new SimpleCarDTO { ID = car.ID, BrandName = car.Model.Brand.Name, ModelName = car.Model.Name, ProductionYear = car.ProductionYear });
            }

            var result = new PagedCarsResponse() { Cars = carsDTO, TotalCount = allCount };

            return Ok(result);
        }

        // GET: api/Cars/allAvailable
        [HttpGet("allAvailable")]
        public async Task<IActionResult> GetAllAvailableCars()
        {
            var query = _context.Cars.AsQueryable();

            query = query.Where(car => car.Availability == Availability.AVAILABLE);

            int allCount = query.Count();

            List<Car>? cars;
            cars = await query.Include(c => c.Model).ThenInclude(m => m.Brand).ToListAsync();

            List<SimpleCarDTO> carsDTO = new List<SimpleCarDTO>();
            // TODO add mapper
            foreach(var car in cars)
            {
                carsDTO.Add(new SimpleCarDTO { ID = car.ID, BrandName = car.Model.Brand.Name, ModelName = car.Model.Name, ProductionYear = car.ProductionYear });
            }

            return Ok(carsDTO);
        }
    }
}
