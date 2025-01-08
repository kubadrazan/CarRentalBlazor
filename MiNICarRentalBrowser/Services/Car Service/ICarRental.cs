using SharedDataModels;

namespace MiNICarRentalBrowser.Services.Car_Service
{
    public interface ICarRental
    {
        Task<List<CarCache>> GetCarsAsync();
    }
}
