using SharedDataModels.Requests.ApiB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories.ApiB
{
    public class AskPriceFactory
    {
        private TimeProvider _timeProvider;

        public AskPriceFactory(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;

        }

        public AskPrice CreateAskPrice(int carId, User user)
        {
            return new AskPrice()
            {
                car_Id = carId,
                age = (int)(_timeProvider.GetElapsedTime(user.BirthDate.Ticks).TotalDays / 365),
                driversLicenceDuration = (int)(_timeProvider.GetElapsedTime(user.DrivingLicenseObtainDate.Ticks).TotalDays / 365),
                start = _timeProvider.GetUtcNow().ToString(),
                Return = _timeProvider.GetUtcNow().AddDays(1).ToString(),
                extraInfo = "no extra info"
            };
        }
    }
}
