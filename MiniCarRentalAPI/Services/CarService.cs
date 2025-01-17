using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Data;
using SendGrid.Helpers.Mail;
using SharedDataModels;
using System;

namespace MiniCarRentalAPI.Services
{
    public class CarService
    {

        public CarService()
        {
        }

        public async Task<Car?> GetCarWithSubDataAsync(CarRentalContext context, int carId)
            => await context.Cars
                .Include(c => c.Model)
                .ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync(c => c.ID == carId);

        public async Task<bool> ChangeCarToAvailableAsync(CarRentalContext context, int carId)
        {
            var car = await context.Cars.FirstOrDefaultAsync(c => c.ID == carId);

            if (car == null)
            {
                return false;
            }

            car.Availability = Availability.AVAILABLE;

            return true;
        }

        public async Task<bool> ChangeCarToUnavailableAsync(CarRentalContext context, int carId)
        {
            var car = await context.Cars.FirstOrDefaultAsync(c => c.ID == carId);

            if (car == null || car.Availability != Availability.AVAILABLE)
            {
                return false;
            }

            car.Availability = Availability.NOT_AVAILABLE;

            return true;
        }
    }
}
