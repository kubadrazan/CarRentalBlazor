using SharedDataModels.DTO.APIB_DTO;
using SharedDataModels.DTO;
using SharedDataModels;
using Newtonsoft.Json;
using MiNICarRentalBrowser.Extensions;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using SharedDataModels.Requests.ApiB;
using SharedDataModels.Factories.ApiB;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace MiNICarRentalBrowser.Services.Car_Service
{
	public class CarRentalB : ICarRental
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiUrl;
		private readonly int _apiID;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ReturnRequestFactory _returnRequestFactory;
        private readonly OfferChoiceFactory _offerChoiceFactory;
		private readonly AskPriceFactory _askPriceFactory;

        public CarRentalB(IHttpClientFactory httpClientFactory, IConfiguration configuration, IUserService userService, IMapper mapper, ReturnRequestFactory returnRequestFactory, OfferChoiceFactory offerChoiceFactory, AskPriceFactory askPriceFactory)
		{
			_httpClient = httpClientFactory.CreateClient("BApiHttpClient");
			_apiUrl = configuration.GetValue<string>("bApiUrl") ?? throw new Exception("No apiB Url!");
			_apiID = 1;
			_userService = userService;
			_mapper = mapper;
			_returnRequestFactory = returnRequestFactory;
			_offerChoiceFactory = offerChoiceFactory;
			_askPriceFactory = askPriceFactory;

        }

		public async Task<string> ChooseOffer(int offerid, User? user)
		{
			try
			{
				var offerChoice = _offerChoiceFactory.CreateOfferChoice(offerid, user);
				var response = await _httpClient.PutAsJsonAsync<OfferChoice>($"{_apiUrl}/Car/Rent", offerChoice);
				var rentalId = await response.Content.ReadFromJsonAsync<int>();
                if (!response.IsSuccessStatusCode || rentalId < 0)
                {
                    return "Error";
                }
                await _userService.AddRentalAsync(_apiID, rentalId, user.ID);
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
                var car = await _httpClient.GetFromJsonAsync<apiBCarDTO>($"{_apiUrl}/Car/GetCar/{carId}");
                var result = _mapper.Map<Car>(car);
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

			if (response != null)
			{
				result = _mapper.Map<List<CarCache>>(response);
				result.ForEach(car => car.SourceApiID = _apiID);
			}

			return result;
		}

		public Task<string> GetDescription(int rentalId)
		{
			return null;
		}

		public async Task<List<Offer>> GetOffersAsync(int carId, User? user)
		{

			if (user == null) return null;

            List<Offer> result = new List<Offer>();
            try
			{
				var askPrice = _askPriceFactory.CreateAskPrice(carId, user);
				var response = await _httpClient.PostAsJsonAsync<AskPrice>($"{_apiUrl}/Car/CreateOffer", askPrice);
				var jsonResponse = await response.Content.ReadAsStringAsync();
				var responseDto = JsonConvert.DeserializeObject<apiBOfferDTO>(jsonResponse);
				result.Add(new Offer() { ID = responseDto.Id, Price = responseDto.PriceDay, CarId = carId, IsInsurance = false });
				result.Add(new Offer() { ID = responseDto.Id, Price = responseDto.PriceDay + responseDto.PriceInsurance, CarId = carId, IsInsurance = true });

				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
				return result;
			}
		}

		public async Task<Rental> GetRentalAsync(int rentalId, string email)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<apiBRentHistoryDTO>($"{_apiUrl}/car/GetRent/{rentalId}");
				if (response != null)
				{
					var rental = _mapper.Map<Rental>(response);
					if (response.offer is not null)
                    {
                        rental.Car.InsurancePricePerDay = response.offer.priceInsurance;
                        rental.Car.PricePerDay = response.offer.priceDay;
                    }
					rental.SourceAPI = _apiID;
					return rental;
				}
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
				return null;
			}
		}

		public async Task ReturnCarAsync(Rental rental, User? user)
		{
			try
            {
				var returnRequest = _returnRequestFactory.CreateReturnRequest(rental, user);
                var response = await _httpClient.PutAsJsonAsync<ReturnRequest>($"{_apiUrl}/Car/Return", returnRequest);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching offers data: {ex.Message}");
			}
		}
	}
}