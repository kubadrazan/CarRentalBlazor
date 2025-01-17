using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CarsController(CarRentalContext context, CarService carService, IMapper mapper)
        {
            _context = context;
            _carService = carService;
            _mapper = mapper;
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

            if (cars != null && cars.Count > 0)
            {
                carsDTO = _mapper.Map<List<SimpleCarDTO>>(cars);
            }

            return Ok(carsDTO);
        }
    }
}
