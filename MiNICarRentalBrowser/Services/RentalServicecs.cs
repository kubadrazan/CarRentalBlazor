using Microsoft.Extensions.Options;
using MiNICarRentalBrowser.Data;

namespace Browser_FrontEnd.Services
{
    public class RentalServicecs
	{
		private readonly HttpClient _httpClient;
		private readonly string _url;
		public RentalServicecs(HttpClient httpClient, IConfiguration configuration)
		{
			{
				_httpClient = httpClient;
				_url = configuration.GetValue<string>("ApiSettings:Url");
			}
		}

        public async Task<List<Car>> GetCarsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Car>>($"{_url}/cars");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching car data: {ex.Message}");
                return null;
            }
        }

        public async Task<Car> GetCarDetailsAsync(int carId)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<Car>($"{_url}/cars/{carId}");
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
				var response = await _httpClient.GetFromJsonAsync<List<Offer>>($"{_url}/offers/{carId}");
				return response;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
				return null;
			}
		}
	}
}
