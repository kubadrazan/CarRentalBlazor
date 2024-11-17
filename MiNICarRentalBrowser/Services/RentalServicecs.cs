using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SharedDataModels;
using SharedDataModels.DTO;

namespace Browser_FrontEnd.Services
{
    public class RentalServicecs
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiA; // Our CarRentalApi

		public RentalServicecs(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
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
				var response = await _httpClient.GetFromJsonAsync<Car>($"{_apiA}/cars/{carId}");
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
				var response = await _httpClient.GetFromJsonAsync<List<Offer>>($"{_apiA}/offers/{carId}");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
				return null;
			}
		}

        public async Task<(List<Car>,int filteredCarsCount)> GetCars(List<string>? brands, List<string>? models, int? lastId, int pageSize)
        {
            var queryParams = CreateQuery( brands, models, lastId, pageSize);

            var urlA = $"{_apiA}/api/Cars?{queryParams}";

            var response = await _httpClient.GetFromJsonAsync<PagedCarsResponse>(urlA);

            if ( response != null && response.Cars != null && response.Cars.Any())
                return (response.Cars, response.TotalCount);

            return (new List<Car>(),0);
        }

        private string CreateQuery( List<string>? brands, List<string>? models, int? lastId, int pageSize)
        {
            var queryParams = new List<string>();

            if (brands != null && brands.Any())
                queryParams.Add($"brands={string.Join(",", brands)}");

            if (models != null && models.Any())
                queryParams.Add($"models={string.Join(",", models)}");

            if (lastId.HasValue)
                queryParams.Add($"lastId={lastId}");

            queryParams.Add($"pageSize={pageSize}");

            return string.Join("&", queryParams); ;
        }
    }
}
