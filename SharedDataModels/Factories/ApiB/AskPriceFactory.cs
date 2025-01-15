using SharedDataModels.Requests.ApiB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories.ApiB
{
    public class AskPriceFactory
    {
        const string time_format = "yyyy-MM-ddTHH:mm:ssZ";

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
                age = (_timeProvider.GetUtcNow().DateTime - user.BirthDate).Days/365,
                driversLicenceDuration = (_timeProvider.GetUtcNow().DateTime - user.DrivingLicenseObtainDate).Days / 365,
                start = _timeProvider.GetUtcNow().ToString(time_format, CultureInfo.InvariantCulture),
                Return = _timeProvider.GetUtcNow().AddDays(1).ToString(time_format, CultureInfo.InvariantCulture),
                extraInfo = "no extra info"
            };
        }
    }
}
