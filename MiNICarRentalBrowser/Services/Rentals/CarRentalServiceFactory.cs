using Microsoft.AspNetCore.Cors.Infrastructure;
using MiNICarRentalBrowser.Services.Car_Service;

namespace MiNICarRentalBrowser.Services
{
	public class CarRentalServiceFactory
	{
		private readonly Dictionary<int, Func<ICarRental>> _serviceMap;

		public CarRentalServiceFactory(IServiceProvider serviceProvider)
		{
			_serviceMap = new Dictionary<int, Func<ICarRental>>
			{
				{ 0, () => serviceProvider.GetRequiredService<CarRentalA>() },
				{ 1, () => serviceProvider.GetRequiredService<CarRentalB>() },
			};
		}

		public ICarRental GetService(int id)
		{
			if (_serviceMap.TryGetValue(id, out var serviceFactory))
			{
				return serviceFactory();
			}

			throw new ArgumentException($"Service with id {id} is not registered", nameof(id));
		}
	}
}
