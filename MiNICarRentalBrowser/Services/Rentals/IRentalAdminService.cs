using SharedDataModels;

namespace MiNICarRentalBrowser.Services.Rentals
{
	public interface IRentalAdminService
	{
		Task<List<Rental>> GetRentalsAsync(int? pageInd, int pageSize = 15);
		Task AcceptCarReturn(int rentalId, string employeeEmail, string acceptationDescription, string carImage);
		Task<string> ConfirmOffer(Guid offerId);
		Task<int> GetRentalsCountAsync();
	}
}
