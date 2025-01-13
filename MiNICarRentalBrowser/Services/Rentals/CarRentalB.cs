using SharedDataModels.DTO.APIB_DTO;
using SharedDataModels.DTO;
using SharedDataModels;

namespace MiNICarRentalBrowser.Services.Car_Service
{
	public class CarRentalB : ICarRental
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiUrl;
		private readonly int _apiID;

		public CarRentalB(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClient = httpClientFactory.CreateClient("BApiHttpClient");
			_apiUrl = configuration.GetValue<string>("bApiUrl") ?? throw new Exception("No apiB Url!");
			_apiID = 1;
		}
		public async Task<string> ChooseOffer(int offerid, string emailAddress)
		{
			throw new NotImplementedException();
			try
			{
				var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/api/car/rent/{offerid}", emailAddress);
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error Choosing Offer: {ex.Message}");
				return null;
			}
		}

		public async Task<Car> GetCarDetailsAsync(int carId)
		{
			throw new NotImplementedException();
			try
			{
				var car = await _httpClient.GetFromJsonAsync<apiBCarDTO>($"{_apiUrl}/api/car/get/{carId}");
				var result = new Car() { ID = car.id, Model = new Model() { Name = car.carModel, Brand = new Brand() { Name = car.carBrand } }, ProductionYear = 1990 };
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching car data: {ex.Message}");
				return null;
			}
		}

		public Task<byte[]> GetCarImage(int rentalId)
		{
			return null;
		}

		public async Task<List<CarCache>> GetCarsAsync()
		{
			var response = await _httpClient.GetFromJsonAsync<List<apiBCarDTO>>($"{_apiUrl}/api/Car/Get");

			List<CarCache> result = new List<CarCache>();
			// TODO add mapper
			if (response != null)
			{
				foreach (var car in response)
				{
					result.Add(new CarCache() { CarID = car.id, BrandName = car.carBrand, ModelName = car.carModel, ProductionYear = 1990, SourceApiID = _apiID });
				}
			}

			return result;
		}

		public Task<string> GetDescription(int rentalId)
		{
			return null;
		}

		public async Task<List<Offer>> GetOffersAsync(int carId)
		{
			// TODO: Implement this method
			throw new NotImplementedException();
			try
			{
				var response = await _httpClient.GetFromJsonAsync<List<Offer>>($"{_apiUrl}/api/car/createoffer/{carId}/OtherData");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
				return null;
			}
		}

		public async Task<Rental> GetRentalAsync(int rentalId, string email)
		{
			// TODO: Implement this method
			throw new NotImplementedException();
			try
			{
				var response = await _httpClient.GetFromJsonAsync<List<Rental>>($"{_apiUrl}/api/car/getmyrents/{email}/");
				return response.Where(r => r.ID == rentalId).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
				return null;
			}
		}

		public async Task ReturnCarAsync(Rental rental)
		{
			throw new NotImplementedException();

			var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/api/car/return/{rental.ID}", rental);

		}
	}
}