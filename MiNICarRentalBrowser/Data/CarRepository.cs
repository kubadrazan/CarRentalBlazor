using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedDataModels;

namespace MiNICarRentalBrowser.Data
{
    public interface ICarRepository
    {
        Task UpdateCarsAsync(List<CarCache> cars);
        Task<List<CarCache>> GetFilteredCarsAsync(List<string> brands, List<string> models, int pageInd = 1, int pageSize = 1);
        int GetFilteredCarsCount(List<string> brands, List<string> models);
        Task<List<string>> GetUniqueBrandsAsync();
        Task<List<string>> GetUniqueModelsAsync();
    }

    public class CarRepository : ICarRepository
    {
        private readonly UsersContext _context;
        private readonly IServiceProvider _serviceProvider;

        public CarRepository(UsersContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task UpdateCarsAsync(List<CarCache> cars)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.CarsCache.RemoveRange(_context.CarsCache);

                await _context.SaveChangesAsync();

                await _context.CarsCache.AddRangeAsync(cars);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<CarCache>> GetFilteredCarsAsync(List<string> brands, List<string> models, int pageInd = 1, int pageSize = 1)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
                var query = context.CarsCache.AsQueryable();

                if (brands != null && brands.Any())
                    query = query.Where(car => brands.Contains(car.BrandName));

                if (models != null && models.Any())
                    query = query.Where(car => models.Contains(car.ModelName));

                query = query.OrderBy(car => car.CarID);
                query = query.Skip((pageInd - 1) * pageSize);
                query = query.Take(pageSize);

                return await query.ToListAsync();
            }
        }

        public int GetFilteredCarsCount(List<string> brands, List<string> models)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
                var query = context.CarsCache.AsQueryable();

                if (brands != null && brands.Any())
                    query = query.Where(car => brands.Contains(car.BrandName));

                if (models != null && models.Any())
                    query = query.Where(car => models.Contains(car.ModelName));

                return query.Count();
            }
        }

        public async Task<List<string>> GetUniqueBrandsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
                return await context.CarsCache.Select(c => c.BrandName)
                .Distinct().ToListAsync();
            }
        }

        public async Task<List<string>> GetUniqueModelsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
                return await context.CarsCache.Select(c => c.ModelName)
                .Distinct().ToListAsync();
            }
        }
    }
}
