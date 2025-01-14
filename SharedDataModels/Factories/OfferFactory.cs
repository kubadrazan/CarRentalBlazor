using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories
{
	public class OfferFactory(TimeProvider timeProvider)
	{
		private const int OFFER_VALIDITY_TIME_MINS = 10;

		public Offer CreateOffer(Car car, bool addInsurance)
		{
			return new Offer()
			{
				OfferGuid = Guid.NewGuid(),
				CarId = car.ID,
				IsInsurance = addInsurance,
				Price = addInsurance ? car.InsurancePricePerDay : car.PricePerDay,
				ExpirationDate = timeProvider.GetUtcNow().DateTime.AddMinutes(OFFER_VALIDITY_TIME_MINS),
				UserEmail = null
			};
		}
		public List<Offer> CreateOfferList(Car car)
		{
			return new List<Offer>()
			{
				CreateOffer(car, true),
				CreateOffer(car, false)
			};
		}
	}
}
