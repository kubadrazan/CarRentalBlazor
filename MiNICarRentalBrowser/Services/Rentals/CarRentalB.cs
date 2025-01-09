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

		public Task AcceptCarReturn(int rentalId, string employeeEmail, string acceptationDescription, string carImage)
		{
			throw new NotImplementedException();
		}

		public Task<string> ChooseOffer(int offerid, string emailAddress)
		{
			throw new NotImplementedException();
		}

		public Task<string> ConfirmOffer(Guid offerId)
		{
			throw new NotImplementedException();
		}

		public Task<List<BrandModelDTO>> GetBrandsModelsNamesAsyc()
		{
			throw new NotImplementedException();
		}

		public Task<Car> GetCarDetailsAsync(int carId)
		{
			throw new NotImplementedException();
		}

		public Task<byte[]> GetCarImage(int Id)
		{
			throw new NotImplementedException();
		}

		public Task<(List<SimpleCarDTO>, int filteredCarsCount)> GetCars(List<string>? brands, List<string>? models, int? pageInd, int pageSize)
		{
			throw new NotImplementedException();
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

		public Task<byte[]> GetImage(int rentalId)
		{
			throw new NotImplementedException();
		}

		public Task<List<Offer>> GetOffersAsync(int carId)
		{
			throw new NotImplementedException();
		}

		public Task<Rental> GetRentalAsync(RentalBrowser rentalBrowser)
		{
			throw new NotImplementedException();
		}

		public Task<Rental> GetRentalAsync(int Id)
		{
			throw new NotImplementedException();
		}

		public Task<List<Rental>> GetRentalsAsync(int? pageInd, int pageSize = 15)
		{
			throw new NotImplementedException();
		}

		public Task<int> GetRentalsCountAsync()
		{
			throw new NotImplementedException();
		}

		public Task<List<string>> GetUniqueBrandNamesAsyc()
		{
			throw new NotImplementedException();
		}

		public Task<List<string>> GetUniqueModelNamesAsyc()
		{
			throw new NotImplementedException();
		}

		public Task ReturnCarAsync(Rental rental)
		{
			throw new NotImplementedException();
		}

	}
}
