using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using SharedDataModels;
using SharedDataModels.DTO;
using StackExchange.Redis;
using System.Text.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace MiNICarRentalBrowser.Data
{
    public interface ICarRepository
    {
        Task InsertCars(List<CarCache> cars);
        Task<List<CarCache>> GetFilteredCarsAsync(string brands, List<string> models, int pageInd = 1, int pageSize = 1);
        Task<int> GetFilteredCarsCount(string brand, List<string> models);
        Task<List<BrandModelDTO>> GetBrandsModelsAsync();
        Task<CarCache?> GetCarCacheAsync(int apiID, int ID);
    }

    public class CarRepository : ICarRepository
    {
        private readonly IDatabase _database;

        public CarRepository(IDatabase database)
        {
            _database = database;
        }

        private async Task WaitForData()
        {
            for (int i = 0; i < 5; i++)
            {
                if (await AreCarsAvailable())
                    return;
                await Task.Delay(4000);
            }
        }

        public async Task<bool> AreCarsAvailable()
        {
            return await _database.KeyExistsAsync("UpToDate");
        }

        public async Task InsertCars(List<CarCache> cars)
        {
            if (await AreCarsAvailable())
            {
                await _database.KeyDeleteAsync("UpToDate");
                await DeleteSets();
            }

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

                string brandKey = $"Models:{car.BrandName}";
                batch.SetAddAsync(brandKey, car.ModelName);
                batch.KeyExpireAsync(brandKey, expire);

                string modelKey = $"Cars:{car.BrandName}:{car.ModelName}";
                batch.SetAddAsync(modelKey, carKey);
                batch.KeyExpireAsync(modelKey, expire);
            }
            await _database.StringSetAsync("UpToDate", "Cars", expire);
            batch.Execute();
        }

        private async Task DeleteSets()
        {
            var batch = _database.CreateBatch();

            var brandKeys = await _database.SetMembersAsync("Brands");

            foreach (var brandKey in brandKeys)
            {
                string brandName = brandKey.ToString();

                var modelKeys = await _database.SetMembersAsync($"Models:{brandName}");

                foreach (var modelKey in modelKeys)
                {
                    string modelName = modelKey.ToString();

                    batch.KeyDeleteAsync($"Cars:{brandName}:{modelName}");
                }
                batch.KeyDeleteAsync($"Models:{brandName}");
            }
            batch.KeyDeleteAsync("Brands");

            batch.Execute();
        }

        public async Task<List<CarCache>> GetFilteredCarsAsync(string brand, List<string> models, int pageInd = 1, int pageSize = 1)
        {
            if (!await AreCarsAvailable())
                await WaitForData();
            List<CarCache> cars = new List<CarCache>();

            if (models == null || models.Count == 0)
            {
                string brandModelsSet = $"Models:{brand}";
                var set = await _database.SetMembersAsync(brandModelsSet);
                models = set.Select(s => s.ToString()).ToList();
            }

            string modelsSetKeyPrefix = $"Cars:{brand}:";
            foreach (var model in models)
            {
                string modelSetKey = modelsSetKeyPrefix + model;
                var carKeys = await _database.SetMembersAsync(modelSetKey);

                var task = carKeys.Select(carKey => _database.StringGetAsync(carKey.ToString()));
                var carValues = await Task.WhenAll(task);

                foreach (var carValue in carValues)
                {
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
            if (!await AreCarsAvailable())
                await WaitForData();
            string modelsSetKeyPrefix = $"Cars:{brand}:";
            long res = 0;

            if (models == null || models.Count == 0)
            {
                string brandModelsSet = $"Models:{brand}";
                var set = await _database.SetMembersAsync(brandModelsSet);
                models = set.Select(s => s.ToString()).ToList();
            }

            var tasks = models.Select(s => _database.SetLengthAsync(modelsSetKeyPrefix + s));
            var tasksResult = await Task.WhenAll(tasks);
            res += tasksResult.Sum();

            return (int)res;
        }

        public async Task<List<BrandModelDTO>> GetBrandsModelsAsync()
        {
            if (!await AreCarsAvailable())
                await WaitForData();
            var brandModelGroups = new List<BrandModelDTO>();

            var brandKeys = await _database.SetMembersAsync("Brands");

            foreach (var brandKey in brandKeys)
            {
                string brandName = brandKey.ToString();

                var modelKeys = await _database.SetMembersAsync($"Models:{brandName}");

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
            if (!await AreCarsAvailable())
                await WaitForData();
            var jsonCar = await _database.StringGetAsync($"CarChache:{apiID}:{ID}");
            if (jsonCar.IsNullOrEmpty)
                return null;
            return JsonConvert.DeserializeObject<CarCache>(jsonCar);
        }
    }
}
