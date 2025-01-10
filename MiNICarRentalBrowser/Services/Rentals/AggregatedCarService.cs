using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiNICarRentalBrowser.Data;
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

		public AggregatedCarService(IEnumerable<ICarRental> carRentals, ICarRepository carRepository, IServiceProvider serviceProvider, CarRentalServiceFactory carRentalServiceFactory)
		{
			_carRentals = carRentals.ToList();
			_carRepository = carRepository;
			_serviceProvider = serviceProvider;
			_carRentalServiceFactory = carRentalServiceFactory;
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

		public async Task<List<BrandModelDTO>> GetBrandsModelsAsync()
		{
			return await _carRepository.GetBrandsModelsAsync();
		}

		public Task<Car> GetCarDetailsAsync(int apiId, int carId)
		{
			return _carRentalServiceFactory.GetService(apiId).GetCarDetailsAsync(carId);
		}

		public async Task<List<Offer>> GetOffersAsync(int apiId, int carId)
		{
			return await _carRentalServiceFactory.GetService(apiId).GetOffersAsync(carId);
		}

		public Task<string> ConfirmOffer(Guid offerId)
		{
			return _carRentalServiceFactory.GetService(0).ConfirmOffer(offerId);
		}

		public Task<string> ChooseOffer(int apiId, int offerid, string emailAddress)
		{
			return _carRentalServiceFactory.GetService(apiId).ChooseOffer(offerid, emailAddress);
		}

		public Task<Rental> GetRentalAsync(RentalBrowser rentalBrowser)
		{
			return _carRentalServiceFactory.GetService(0).GetRentalAsync(rentalBrowser);
		}

		public Task<Rental> GetRentalAsync(int Id)
		{
			return _carRentalServiceFactory.GetService(0).GetRentalAsync(Id);
		}

		public Task<byte[]> GetCarImage(int Id)
		{
			return _carRentalServiceFactory.GetService(0).GetCarImage(Id);
		}

		public Task<int> GetRentalsCountAsync()
		{
			return _carRentalServiceFactory.GetService(0).GetRentalsCountAsync();
		}

		public Task<List<Rental>> GetRentalsAsync(int? pageInd, int pageSize = 15)
		{
			return _carRentalServiceFactory.GetService(0).GetRentalsAsync(pageInd, pageSize);
		}

		public Task ReturnCarAsync(Rental rental)
		{
			return _carRentalServiceFactory.GetService(0).ReturnCarAsync(rental);
		}

		public Task AcceptCarReturn(int rentalId, string employeeEmail, string acceptationDescription, string carImage)
		{
			return _carRentalServiceFactory.GetService(0).AcceptCarReturn(rentalId, employeeEmail, acceptationDescription, carImage);
		}

		public Task<byte[]> GetImage(int rentalId)
		{
			return _carRentalServiceFactory.GetService(0).GetImage(rentalId);
		}
	}
}
