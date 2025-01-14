using SharedDataModels.DTO.APIB_DTO;
using SharedDataModels.DTO;
using SharedDataModels;
using Newtonsoft.Json;
using MiNICarRentalBrowser.Extensions;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace MiNICarRentalBrowser.Services.Car_Service
{
	public class CarRentalB : ICarRental
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiUrl;
		private readonly int _apiID;
		private readonly IMapper _mapper;

		public CarRentalB(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMapper mapper)
		{
			_httpClient = httpClientFactory.CreateClient("BApiHttpClient");
			_apiUrl = configuration.GetValue<string>("bApiUrl") ?? throw new Exception("No apiB Url!");
			_apiID = 1;
			_mapper = mapper;
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
				var response = await _httpClient.GetFromJsonAsync<apiBRentHistoryDTO>($"{_apiUrl}/car/GetRent/{rentalId}");
				if (response != null)
				{
					var rental = _mapper.Map<Rental>(response);
					rental.Car.InsurancePricePerDay = response.offer.priceInsurance;
					rental.Car.PricePerDay = response.offer.priceDay;
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

		public async Task ReturnCarAsync(Rental rental)
		{
			throw new NotImplementedException();

			var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/api/car/return/{rental.ID}", rental);

		}
	}
}