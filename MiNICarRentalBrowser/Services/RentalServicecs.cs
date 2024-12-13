using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SharedDataModels;
using SharedDataModels.DTO;
using System.Net.Mail;
using System.Text.Json;
using System.Text;
using Azure;
using MiNICarRentalBrowser.Services;

namespace Browser_FrontEnd.Services
{
	public class RentalServicecs
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiA; // Our CarRentalApi
		private readonly IUserService _userService;

		public RentalServicecs(HttpClient httpClient, IUserService userService, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_userService = userService;
			_apiA = configuration.GetValue<string>("ApiUrls:ApiRentalA");
		}

		public async Task<List<string>> GetUniqueBrandNamesAsyc()
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<List<string>>($"{_apiA}/api/Cars/brands");
				return response ?? new List<string>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching car data: {ex.Message}");
				return new List<string>();
			}
		}

		public async Task<List<string>> GetUniqueModelNamesAsyc()
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<List<string>>($"{_apiA}/api/Cars/models");
				return response ?? new List<string>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching car data: {ex.Message}");
				return new List<string>();
			}
		}

		public async Task<List<Model>> GetBrandsModelsNamesAsyc()
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<List<Model>>(
					$"{_apiA}/api/Cars/brandsModels"
					);
				return response ?? new List<Model>();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching car data: {ex.Message}");
				return new List<Model>();
			}
		}

		public async Task<Car> GetCarDetailsAsync(int carId)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<Car>($"{_apiA}/api/cars/{carId}");
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
				var response = await _httpClient.GetFromJsonAsync<List<Offer>>($"{_apiA}/api/Rental/offers/{carId}");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
				return null;
			}
		}

		public async Task<string> ConfirmOffer(int offerId)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync<int>($"{_apiA}/api/Rental/offers/acceptOffer", offerId);
				await _userService.AcceptOfferAsync(await response.Content.ReadFromJsonAsync<Rental>());

				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error confirming data: {ex.Message}");
				return null;
			}
		}

		// todo change name to choose offer
		public async Task<string> SendMail(int offerid, string emailAddress)
		{
			try
			{
				var content = new StringContent(JsonSerializer.Serialize(emailAddress), Encoding.UTF8, "application/json");
				var response = await _httpClient.PutAsync($"{_apiA}/api/Rental/offers/chooseOffer/{offerid}", content);
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error sending email");
				throw;
			}
		}

		public async Task<(List<Car>, int filteredCarsCount)> GetCars(List<string>? brands, List<string>? models, int? lastId, int pageSize)
		{
			var queryParams = CreateQuery(brands, models, lastId, pageSize);

			var urlA = $"{_apiA}/api/Cars?{queryParams}";

			var response = await _httpClient.GetFromJsonAsync<PagedCarsResponse>(urlA);

			if (response != null && response.Cars != null && response.Cars.Any())
				return (response.Cars, response.TotalCount);

			return (new List<Car>(), 0);
		}

		private string CreateQuery(List<string>? brands, List<string>? models, int? lastId, int pageSize)
		{
			var queryParams = new List<string>();

			if (brands != null && brands.Any())
				foreach (var brand in brands)
					queryParams.Add($"brands={brand}");

			if (models != null && models.Any())
				foreach (var model in models)
					queryParams.Add($"models={model}");

			if (lastId.HasValue)
				queryParams.Add($"lastId={lastId}");

			queryParams.Add($"pageSize={pageSize}");

			return string.Join("&", queryParams);
		}

		public async Task<Rental> GetRentalAsync(RentalBrowser rentalBrowser)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<Rental>($"{_apiA}/api/rentals/{rentalBrowser.ID}");
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
				var response = await _httpClient.GetFromJsonAsync<Rental>($"{_apiA}/api/rental/rentals/{Id}");
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
                var response = await _httpClient.GetFromJsonAsync<int>($"{_apiA}/api/rental/rentals/count");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching rental data: {ex.Message}");
                return -1;
            }
        }

        public async Task<List<Rental>> GetRentalsAsync(int? lastId = null, int pageSize = 15, bool lowerThanId = false)
        {
            var queryParams = new List<string>();

            if (lastId.HasValue)
                queryParams.Add($"lastId={lastId}");

            queryParams.Add($"pageSize={pageSize}");

			queryParams.Add($"lowerThanId={lowerThanId.ToString().ToLower()}");

			string url = $"{_apiA}/api/rental/rentals?{string.Join("&", queryParams)}";

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
			var returnRequest = new ReturnCarRequest(rental, 0, 0);
			var response = await _httpClient.PutAsJsonAsync<ReturnCarRequest>($"{_apiA}/api/Rental/rentals/returnCar/{rental.ID}", returnRequest);
		}
	}
}
