using SharedDataModels;
using SharedDataModels.DTO;
using SharedDataModels.Requests;
using System.Text;

namespace MiNICarRentalBrowser.Services.Car_Service
{
	public interface ICarRental
	{
		Task<List<CarCache>> GetCarsAsync();
		Task<Car> GetCarDetailsAsync(int carId);
		Task<List<Offer>> GetOffersAsync(int carId);
		Task<string> ChooseOffer(int offerid, string emailAddress);
		Task<Rental> GetRentalAsync(int rentalId, string email);
		Task ReturnCarAsync(Rental rental);
		Task<byte[]> GetCarImage(int rentalId);
		Task<string> GetDescription(int rentalId);
	}
}
