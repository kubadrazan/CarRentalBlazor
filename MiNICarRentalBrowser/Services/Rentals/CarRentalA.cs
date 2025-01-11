using SharedDataModels;
using SharedDataModels.DTO;
using SharedDataModels.Requests;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace MiNICarRentalBrowser.Services.Car_Service
{
	public class CarRentalA : ICarRental //Our api
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiUrl;
		private readonly int _apiID;
		private readonly IUserService _userService;

		public CarRentalA(IHttpClientFactory httpClientFactory, IConfiguration configuration, IUserService userService)
		{
			_httpClient = httpClientFactory.CreateClient("ApiKeyClient");
			_apiID = 0;
#if DEBUG
			_apiUrl = configuration.GetValue<string>("ApiUrls:ApiRentalA") ?? throw new Exception("No ApiRentalA Url in configuration file!");
#else
			_apiUrl = configuration.GetValue<string>("aApiUrl") ?? throw new Exception("No apiA Url in configuration file!");
#endif
			_userService = userService;
		}

		public async Task<List<CarCache>> GetCarsAsync()
		{
			var response = await _httpClient.GetFromJsonAsync<List<SimpleCarDTO>>($"{_apiUrl}/api/Cars/allAvailable");

			List<CarCache> result = new List<CarCache>();
			// TODO add mapper
			if (response != null)
			{
				foreach (var car in response)
				{
					result.Add(new CarCache() { CarID = car.ID, BrandName = car.BrandName, ModelName = car.ModelName, ProductionYear = car.ProductionYear, SourceApiID = _apiID });
				}
			}

			return result;
		}

		public async Task<Car> GetCarDetailsAsync(int carId)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<Car>($"{_apiUrl}/api/cars/{carId}");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching car data: {ex.Message}");
				return null;
			}
		}

		public async Task<List<Offer>> GetOffersAsync(int carId)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<List<Offer>>($"{_apiUrl}/api/Rental/offers/{carId}");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
				return null;
			}
		}

		public async Task<string> ConfirmOffer(Guid offerId)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync<Guid>($"{_apiUrl}/api/Rental/offers/acceptOffer", offerId);
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error confirming data: {ex.Message}");
				return null;
			}
		}

		public async Task<string> ChooseOffer(int offerid, string emailAddress)
		{
			try
			{
				var content = new StringContent(JsonSerializer.Serialize(emailAddress), Encoding.UTF8, "application/json");
				var response = await _httpClient.PutAsync($"{_apiUrl}/api/Rental/offers/chooseOffer/{offerid}", content);
				if (!response.IsSuccessStatusCode)
				{
					return "Error";
                }
                await _userService.AddRentalAsync(await response.Content.ReadFromJsonAsync<Rental>());
                return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error sending email");
				throw;
			}
		}

		public async Task<Rental> GetRentalAsync(RentalBrowser rentalBrowser)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<Rental>($"{_apiUrl}/api/rentals/{rentalBrowser.ID}");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching rental data: {ex.Message}");
				return null;
			}
		}

		public async Task<Rental> GetRentalAsync(int Id)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<Rental>($"{_apiUrl}/api/rental/rentals/{Id}");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching rental data: {ex.Message}");
				return null;
			}
		}

		public async Task<byte[]> GetCarImage(int Id)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<byte[]>($"{_apiUrl}/api/acceptations/carimage/{Id}");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching rental data: {ex.Message}");
				return null;
			}
		}

		public async Task<int> GetRentalsCountAsync()
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<int>($"{_apiUrl}/api/rental/rentals/count");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching rental data: {ex.Message}");
				return -1;
			}
		}

		public async Task<List<Rental>> GetRentalsAsync(int? pageInd, int pageSize = 15)
		{
			// Building query
			var queryParams = new List<string>();
			queryParams.Add($"pageInd={pageInd}");
			queryParams.Add($"pageSize={pageSize}");
			string url = $"{_apiUrl}/api/Rental/rentals?{string.Join("&", queryParams)}";

			try
			{
				var response = await _httpClient.GetFromJsonAsync<List<Rental>>(url);
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching rental data: {ex.Message}");
				return null;
			}
		}

		public async Task ReturnCarAsync(Rental rental)
		{
			var response = await _httpClient.PutAsJsonAsync<string>($"{_apiUrl}/api/Rental/rentals/returnCar/{rental.ID}", rental.UserEmail);
		}

		public async Task AcceptCarReturn(int rentalId, string employeeEmail, string acceptationDescription, string carImage)
		{
			var acceptReturnRequest = new AcceptReturnRequest(employeeEmail, acceptationDescription, carImage);
			var response = await _httpClient.PutAsJsonAsync<AcceptReturnRequest>($"{_apiUrl}/api/Rental/rentals/acceptReturn/{rentalId}", acceptReturnRequest);
		}

		public async Task<byte[]> GetImage(int rentalId)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<byte[]>($"{_apiUrl}/api/acceptations/carimage/{rentalId}");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching rental data: {ex.Message}");
				return null;
			}

		}

		public async Task<string> GetDescription(int rentalId)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<Description>($"{_apiUrl}/api/acceptations/cardescription/{rentalId}");
				var content = response.Content;
				return content;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching rental data: {ex.Message}");
				return null;
			}
		}
	}
}
