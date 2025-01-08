using MiNICarRentalBrowser.Data;
using SharedDataModels;

namespace MiNICarRentalBrowser.Services.Car_Service
{
    public class AggregatedCarService
    {
        private readonly List<ICarRental> _carRentals;
        private readonly ICarRepository _carRepository;

        public AggregatedCarService(IEnumerable<ICarRental> carRentals, ICarRepository carRepository)
        {
            _carRentals = carRentals.ToList();
            _carRepository = carRepository;
        }

        public async Task<List<CarCache>> GetAllCarsAsync()
        {
            var task = _carRentals.Select(service => service.GetCarsAsync());
            var results = await Task.WhenAll(task);
            return results.SelectMany(cars => cars).ToList();
        }

        public async Task UpdateCarsInDBAsync()
        {
            var cars = await GetAllCarsAsync();
            await _carRepository.UpdateCarsAsync(cars);
        }
    }
}
