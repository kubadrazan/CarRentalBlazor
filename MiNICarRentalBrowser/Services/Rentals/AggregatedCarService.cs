using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiNICarRentalBrowser.Data;
using MiNICarRentalBrowser.Services.Rentals;
using SharedDataModels;
using SharedDataModels.DTO;

namespace MiNICarRentalBrowser.Services.Car_Service
{
    public class AggregatedCarService
	{
		private readonly List<ICarRental> _carRentals;
		private readonly ICarRepository _carRepository;
		private readonly IServiceProvider _serviceProvider;
		private readonly CarRentalServiceFactory _carRentalServiceFactory;
		private readonly CacheManager _cacheManager;

		public AggregatedCarService(IEnumerable<ICarRental> carRentals, ICarRepository carRepository, IServiceProvider serviceProvider, CarRentalServiceFactory carRentalServiceFactory, CacheManager cacheManager)
		{
			_carRentals = carRentals.ToList();
			_carRepository = carRepository;
			_serviceProvider = serviceProvider;
			_carRentalServiceFactory = carRentalServiceFactory;
			_cacheManager = cacheManager;
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
			await _carRepository.InsertCars(cars);
		}

		public async Task<List<CarCache>> GetFilteredCars(string brand, List<string> models, int? pageInd = 1, int pageSize = 1)
		{
			if (pageInd == null)
				pageInd = 1;

			if (pageInd < 1 || pageSize < 1)
				throw new ArgumentOutOfRangeException();

			return await _carRepository.GetFilteredCarsAsync(brand, models, (int)pageInd, pageSize);
		}

		public async Task<int> GetFilteredCarsCount(string brand, List<string> models)
		{
			return await _carRepository.GetFilteredCarsCount(brand, models);
		}

		public async Task<List<string>> GetUniqueBrands()
		{
			return await _carRepository.GetUniqueBrandsAsync();
		}

		public async Task<List<string>> GetUniqueModels()
		{
			return await _carRepository.GetUniqueModelsAsync();
		}

        public async Task<List<string>> GetUniqueModels(string brand)
        {
            return await _carRepository.GetUniqueModelsAsync(brand);
        }

        public async Task<List<BrandModelDTO>> GetBrandsModelsAsync()
		{
            return await _carRepository.GetBrandsModelsAsync();
		}

		public async Task<Car?> GetCarDetailsAsync(int apiId, int carId)
		{
			var car =  await _cacheManager.GetDetailedCar(apiId, carId);
			if (car != null)
				return car;

			car = await _carRentalServiceFactory.GetService(apiId).GetCarDetailsAsync(carId);

			if (car != null)
				await _cacheManager.SetDetailedCar(apiId, car);

            return car;
		}

		public async Task<List<Offer>> GetOffersAsync(int apiId, int carId)
		{
			return await _carRentalServiceFactory.GetService(apiId).GetOffersAsync(carId);
		}

		public Task<string> ChooseOffer(int apiId, int offerid, string emailAddress)
		{
			return _carRentalServiceFactory.GetService(apiId).ChooseOffer(offerid, emailAddress);
		}

		public Task<Rental> GetRentalAsync(int rentalId, int apiId, string email)
		{
			return _carRentalServiceFactory.GetService(apiId).GetRentalAsync(rentalId, email);
		}

		public Task ReturnCarAsync(Rental rental)
		{
			return _carRentalServiceFactory.GetService(rental.SourceAPI).ReturnCarAsync(rental);
		}

		public Task<byte[]> GetCarImage(Rental rental)
		{
			return _carRentalServiceFactory.GetService(rental.SourceAPI).GetCarImage(rental.ID);
		}

		public Task<string> GetDescription(Rental rental)
		{
			return _carRentalServiceFactory.GetService(rental.SourceAPI).GetDescription(rental.ID);
		}
	}
}
