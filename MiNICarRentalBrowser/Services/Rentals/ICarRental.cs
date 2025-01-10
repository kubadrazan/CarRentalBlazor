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
		Task<string> ConfirmOffer(Guid offerId);
		Task<string> ChooseOffer(int offerid, string emailAddress);
		Task<Rental> GetRentalAsync(RentalBrowser rentalBrowser);
		Task<Rental> GetRentalAsync(int Id);
		Task<byte[]> GetCarImage(int Id);
		Task<int> GetRentalsCountAsync();
		Task<List<Rental>> GetRentalsAsync(int? pageInd, int pageSize = 15);
		Task ReturnCarAsync(Rental rental);
		Task AcceptCarReturn(int rentalId, string employeeEmail, string acceptationDescription, string carImage);
		Task<byte[]> GetImage(int rentalId);
		Task<string> GetDescription(int rentalId);
	}
}
