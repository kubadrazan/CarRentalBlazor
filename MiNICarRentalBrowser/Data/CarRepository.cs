using SharedDataModels;

namespace MiNICarRentalBrowser.Data
{
    public interface ICarRepository
    {
        Task UpdateCarsAsync(List<CarCache> cars);
    }

    public class CarRepository : ICarRepository
    {
        private readonly UsersContext _context;

        public CarRepository(UsersContext context)
        {
            _context = context;
        }

        public async Task UpdateCarsAsync(List<CarCache> cars)
        {
            _context.CarsCache.RemoveRange(_context.CarsCache);

            await _context.CarsCache.AddRangeAsync(cars);

            await _context.SaveChangesAsync();
        }
    }
}
