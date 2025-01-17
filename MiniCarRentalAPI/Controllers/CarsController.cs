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
using MiniCarRentalAPI.Services;
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
        private readonly CarService _carService;

        public CarsController(CarRentalContext context, CarService carService)
        {
            _context = context;
            _carService = carService;
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _carService.GetCarWithSubDataAsync(_context, id);

            if (car == null) return NotFound();

            return Ok(car);
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
