using SharedDataModels;
using SharedDataModels.DTO;

namespace MiNICarRentalBrowser.Services.Car_Service
{
    public class CarRentalA : ICarRental
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly int _apiID;

        public CarRentalA(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("ApiKeyClient");

#if DEBUG
            _apiUrl = configuration.GetValue<string>("ApiUrls:ApiBrowserA") ?? throw new Exception("No apiA Url in configuration file!");
#else
			_apiUrl = configuration.GetValue<string>("aApiUrl") ?? throw new Exception("No apiA Url in Azure key vault!");
#endif
            _apiID = 0;
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
    }
}
