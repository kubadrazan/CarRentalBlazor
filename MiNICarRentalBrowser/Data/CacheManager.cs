using Newtonsoft.Json;
using SharedDataModels;
using SharedDataModels.DTO;
using StackExchange.Redis;

namespace MiNICarRentalBrowser.Data
{
    public class CacheManager
    {
        public readonly IDatabase _db;

        public CacheManager(IDatabase db)
        {
            _db = db;
        }

        public async Task<Car?> GetDetailedCar(int apiId, int carId)
        {
            string carKey = $"DetailedCar:{apiId}:{carId}";

            var carValue = await _db.StringGetAsync(carKey);

            if(!carValue.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<Car>(carValue);
            }
            return null;
        }

        public async Task SetDetailedCar(int apiId, Car car)
        {
            string carKey = $"DetailedCar:{apiId}:{car.ID}";

            await _db.StringSetAsync(carKey, JsonConvert.SerializeObject(car), TimeSpan.FromMinutes(10));
        }

        public async Task<User?> GetUserAsync(string email)
        {
            string userKey = $"User:{email}";

            var userValue = await _db.StringGetAsync(userKey);

            if(!userValue.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<User>(userValue);
            }

            return null;
        }

        public async Task SetUserAsync(User user)
        {
            string userKey = $"User:{user.Email}";

            await _db.StringSetAsync(userKey, JsonConvert.SerializeObject(user), TimeSpan.FromMinutes(10));
        }

        public async Task<List<Offer>> GetOffers(int apiId, int carId, string email)
        {
            string offersKey = $"Offers:{apiId}:{carId}:{email}";

            var offerValue = await _db.StringGetAsync(offersKey);

            if(!offerValue.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<List<Offer>>(offerValue);
            }

            return null;
        }

        public async Task SetOffers(int apiId, List<Offer> offers, string email)
        {
            if (offers == null || offers.Count == 0)
                return;
            string offersKey = $"Offers:{apiId}:{offers.First().CarId}:{email}";
            await _db.StringSetAsync(offersKey, JsonConvert.SerializeObject(offers), TimeSpan.FromMinutes(10));
        }
    }
}
