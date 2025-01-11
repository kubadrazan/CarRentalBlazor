using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedDataModels;
using SharedDataModels.DTO;
using StackExchange.Redis;
using System.Text.Json;

namespace MiNICarRentalBrowser.Data
{
    public interface ICarRepository
    {
        Task UpdateCarsAsync(List<CarCache> cars);
        Task<List<CarCache>> GetFilteredCarsAsync(List<string> brands, List<string> models, int pageInd = 1, int pageSize = 1);
        int GetFilteredCarsCount(List<string> brands, List<string> models);
        Task<List<string>> GetUniqueBrandsAsync();
        Task<List<string>> GetUniqueModelsAsync();
        Task<List<BrandModelDTO>> GetBrandsModelsAsync();
        Task<CarCache?> GetCarCacheAsync(int ID);
    }

    public class CarRepository : ICarRepository
    {
        private readonly UsersContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabase _database;

        public CarRepository(UsersContext context, IServiceProvider serviceProvider, IDatabase database)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _database = database;
        }

        public async Task UpdateCarsAsync(List<CarCache> cars)
        {
            TimeSpan expire = TimeSpan.FromMinutes(30);
            IBatch batch = _database.CreateBatch();
            foreach (var car in cars)
            {
                if (car.CarID % 13 == 0 && car.CarID < 3000)
                {
                    string carKey = $"Car:{car.CarID}:{car.SourceApiID}";
                    string value = JsonSerializer.Serialize(car);
                    batch.StringSetAsync(carKey, value, expire);

                    string brandsKey = "Brands";
                    batch.SetAddAsync(brandsKey, car.BrandName);

                    string brandKey = $"Brand:{car.BrandName}";
                    batch.SetAddAsync(brandKey, car.ModelName);

                    string modelKey = $"Model:{car.BrandName}:{car.ModelName}";
                    batch.SetAddAsync(modelKey, carKey);
                }
            }
                
            batch.Execute();
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
                .Distinct().OrderBy(c => c).ToListAsync();
            }
        }

        public async Task<List<string>> GetUniqueModelsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
                return await context.CarsCache.Select(c => c.ModelName)
                .Distinct().OrderBy(c => c).ToListAsync();
            }
        }

        public async Task<List<BrandModelDTO>> GetBrandsModelsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
                return await context.CarsCache.GroupBy(c => new { c.ModelName, c.BrandName })
                    .Select(g => new BrandModelDTO { ModelName = g.Key.ModelName, BrandName = g.Key.BrandName })
                    .OrderBy(c => c.BrandName).ThenBy(c => c.ModelName).ToListAsync();
            }
        }

        public async Task<CarCache?> GetCarCacheAsync(int ID)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
                return await context.CarsCache.FindAsync(ID);
            }
        }
    }
}
