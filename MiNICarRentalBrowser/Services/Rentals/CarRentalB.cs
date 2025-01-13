using SharedDataModels.DTO.APIB_DTO;
using SharedDataModels.DTO;
using SharedDataModels;
using Newtonsoft.Json;
using MiNICarRentalBrowser.Extensions;

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
			try
			{
				var car = await _httpClient.GetFromJsonAsync<apiBCarDTO>($"{_apiUrl}/car/getcar/{carId}");
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
			var response = await _httpClient.GetFromJsonAsync<List<apiBCarDTO>>($"{_apiUrl}/Car/Get");

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

		public async Task<List<Offer>> GetOffersAsync(int carId, User? user)
		{

			if (user == null)
			{
				user = new User() { BirthDate = DateTime.Now.AddYears(-20), DrivingLicenseObtainDate = DateTime.Now.AddYears(-2) };
			}
			try
			{
				List<Offer> result = new List<Offer>();
				var offerRequest = new apiBOfferRequestDTO() { Age = user.BirthDate.YearsElapsed(), DriversLicenceDuration = user.DrivingLicenseObtainDate.YearsElapsed(), CarId = carId, Start = DateTime.Now, Return = DateTime.Now.AddDays(1), ExtraInfo = ""};
				var response = await _httpClient.PostAsJsonAsync<apiBOfferRequestDTO>($"{_apiUrl}/car/createoffer", offerRequest);
				var jsonResponse = await response.Content.ReadAsStringAsync();
				var responseDto = JsonConvert.DeserializeObject<apiBOfferDTO>(jsonResponse);
				result.Add(new Offer() { ID = responseDto.Id, Price = responseDto.PriceDay, CarId = carId, IsInsurance = false });
				result.Add(new Offer() { ID = responseDto.Id, Price = responseDto.PriceDay + responseDto.PriceInsurance, CarId = carId, IsInsurance = false });

				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
				return null;
			}
		}

		public async Task<Rental> GetRentalAsync(int rentalId, string email)
		{

			try
			{
				var response = await _httpClient.GetFromJsonAsync<Rental>($"{_apiUrl}/car/GetRent/{rentalId}");
				return response;
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