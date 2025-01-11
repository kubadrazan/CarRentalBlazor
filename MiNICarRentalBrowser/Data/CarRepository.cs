using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedDataModels;
using SharedDataModels.DTO;
using StackExchange.Redis;
using System.Text.Json;
using static MudBlazor.CategoryTypes;
using static MudBlazor.Icons.Custom;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace MiNICarRentalBrowser.Data
{
    public interface ICarRepository
    {
        void UpdateCars(List<CarCache> cars);
        Task<List<CarCache>> GetFilteredCarsAsync(string brands, List<string> models, int pageInd = 1, int pageSize = 1);
        Task<int> GetFilteredCarsCount(string brand, List<string> models);
        Task<List<string>> GetUniqueBrandsAsync();
        Task<List<string>> GetUniqueModelsAsync();
        Task<List<string>> GetUniqueModelsAsync(string brand);
        Task<List<BrandModelDTO>> GetBrandsModelsAsync();
        Task<CarCache?> GetCarCacheAsync(int apiID, int ID);
    }

    public class CarRepository : ICarRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabase _database;

        public CarRepository(IServiceProvider serviceProvider, IDatabase database)
        {
            _serviceProvider = serviceProvider;
            _database = database;
        }

        public void UpdateCars(List<CarCache> cars)
        {
            TimeSpan expire = TimeSpan.FromMinutes(30);
            IBatch batch = _database.CreateBatch();
            foreach (var car in cars)
            {
                string carKey = $"CarCache:{car.SourceApiID}:{car.CarID}";
                string carValue = JsonConvert.SerializeObject(car);
                batch.StringSetAsync(carKey, carValue, expire);

                string brandsKey = "Brands";
                batch.SetAddAsync(brandsKey, car.BrandName);
                batch.KeyExpireAsync(brandsKey, expire);

                string brandKey = $"Brand:{car.BrandName}";
                batch.SetAddAsync(brandKey, car.ModelName);
                batch.KeyExpireAsync(brandKey, expire);

                string modelKey = $"Model:{car.BrandName}:{car.ModelName}";
                batch.SetAddAsync(modelKey, carKey);
                batch.KeyExpireAsync(modelKey, expire);
            }
                
            batch.Execute();
        }

        public async Task<List<CarCache>> GetFilteredCarsAsync(string brand, List<string> models, int pageInd = 1, int pageSize = 1)
        {
            List<CarCache> cars = new List<CarCache>();

            if (models == null || models.Count == 0)
            {
                string brandModelsSet = $"Brand:{brand}";
                var set = await _database.SetMembersAsync(brandModelsSet);
                models = set.Select(s => s.ToString()).ToList();
            }

            string modelsSetKeyPrefix = $"Model:{brand}:";
            foreach (var model in models)
            {
                string modelSetKey = modelsSetKeyPrefix + model;
                var carKeys = await _database.SetMembersAsync(modelSetKey);

                foreach (var carKey  in carKeys)
                {
                    var carValue = await _database.StringGetAsync(carKey.ToString());

                    if (!carValue.IsNullOrEmpty)
                    {
                        var car = JsonConvert.DeserializeObject<CarCache>(carValue);
                        if (car != null)
                        {
                            cars.Add(car);
                        }
                    }
                }
            }

            var query = cars.AsQueryable();
            query = query.OrderBy(c => c.CarID).Skip((pageInd - 1) * pageSize).Take(pageSize);

            return query.ToList();
        }

        public async Task<int> GetFilteredCarsCount(string brand, List<string> models)
        {
            string modelsSetKeyPrefix = $"Model:{brand}:";
            long res = 0;

            if (models == null || models.Count == 0)
            {
                string brandModelsSet = $"Brand:{brand}";
                var set = await _database.SetMembersAsync(brandModelsSet);
                models = set.Select(s => s.ToString()).ToList();
            }

            foreach (var model in models)
            {
                string modelSetKey = modelsSetKeyPrefix + model;
                res += await _database.SetLengthAsync(modelSetKey);
            }
            return (int)res;
        }

        public async Task<List<string>> GetUniqueBrandsAsync()
        {
            string brandsSetKey = "Brands";

            var brands = await _database.SetMembersAsync(brandsSetKey);

            return brands.Select(b => b.ToString()).OrderBy(b => b).ToList();
        }

        public async Task<List<string>> GetUniqueModelsAsync()
        {
            List<string> brands = await GetUniqueBrandsAsync();
            List<string> models = new List<string>();

            foreach (var brand in brands)
            {
                string brandKey = $"Brand:{brand}";
                var res = await _database.SetMembersAsync(brandKey);
                models.AddRange(res.Select(b => b.ToString()));
            }

            return models.OrderBy(m => m).ToList();
        }

        public async Task<List<string>> GetUniqueModelsAsync(string brand)
        {
            string brandKey = $"Brand:{brand}";
            var res = await _database.SetMembersAsync(brandKey);
            return res.Select(b => b.ToString()).OrderBy(m => m).ToList();
        }

        public async Task<List<BrandModelDTO>> GetBrandsModelsAsync()
        {
            var brandModelGroups = new List<BrandModelDTO>();

            var brandKeys = await _database.SetMembersAsync("Brands");

            foreach (var brandKey in brandKeys)
            {
                string brandName = brandKey.ToString();

                var modelKeys = await _database.SetMembersAsync($"Brand:{brandName}");

                foreach (var modelKey in modelKeys)
                {
                    string modelName = modelKey.ToString();

                    brandModelGroups.Add(new BrandModelDTO
                    {
                        BrandName = brandName,
                        ModelName = modelName
                    });
                }
            }

            return brandModelGroups
                .OrderBy(bm => bm.BrandName)
                .ThenBy(bm => bm.ModelName)
                .ToList();
        }

        public async Task<CarCache?> GetCarCacheAsync(int apiID, int ID)
        {
            var jsonCar = await _database.StringGetAsync($"CarChache:{apiID}:{ID}");
            if (jsonCar.IsNullOrEmpty)
                return null;
            return JsonConvert.DeserializeObject<CarCache>(jsonCar);
        }
    }
}
