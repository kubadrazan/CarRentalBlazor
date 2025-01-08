using Microsoft.AspNetCore.Mvc;
using MiNICarRentalBrowser.Data;
using SharedDataModels;

namespace MiNICarRentalBrowser.Services.Car_Service
{
    public class AggregatedCarService
    {
        private readonly List<ICarRental> _carRentals;
        private readonly ICarRepository _carRepository;
        private readonly IServiceProvider _serviceProvider;

        public AggregatedCarService(IEnumerable<ICarRental> carRentals, ICarRepository carRepository, IServiceProvider serviceProvider)
        {
            _carRentals = carRentals.ToList();
            _carRepository = carRepository;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<CarCache>> GetAllCarsAsync()
        {
            var task = _carRentals.Select(service => service.GetCarsAsync());
            var results = await Task.WhenAll(task);
            return results.SelectMany(cars => cars).ToList();
        }

        public async Task UpdateCarsInDBAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var carRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
                var cars = await GetAllCarsAsync();
                await carRepository.UpdateCarsAsync(cars);
            }
        }

        public async Task<List<CarCache>> GetFilteredCars(List<string> brands, List<string> models, int? pageInd = 1, int pageSize = 1)
        {
            if (pageInd == null)
                pageInd = 1;

            if (pageInd < 1 || pageSize < 1)
                throw new ArgumentOutOfRangeException();

            return await _carRepository.GetFilteredCarsAsync(brands, models, (int)pageInd, pageSize);
        }

        public int GetFilteredCarsCount(List<string> brands, List<string> models)
        {
            return _carRepository.GetFilteredCarsCount(brands, models);
        }

        public async Task<List<string>> GetUniqueBrands()
        {
            return await _carRepository.GetUniqueBrandsAsync();
        }

        public async Task<List<string>> GetUniqueModels()
        {
            return await _carRepository.GetUniqueModelsAsync();
        }
    }
}
