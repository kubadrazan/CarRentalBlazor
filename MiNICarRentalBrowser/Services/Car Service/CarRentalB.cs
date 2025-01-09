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
            _httpClient = httpClientFactory.CreateClient("ApiKeyClient");
            //_apiUrl = configuration.GetValue<string>("bApiUrl") ?? throw new Exception("No apiA Url in configuration file!");
            _apiUrl = string.Empty;
            _apiID = 1;
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
    }
}
