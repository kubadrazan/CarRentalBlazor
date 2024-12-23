using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories
{
	public class RentalFactory
	{
		public Rental CreateRental(Offer offer)
		{
			return new Rental
			{
				RentDate = DateTime.UtcNow,
				CarID = offer.CarId,
				UserEmail = offer.UserEmail,
				SourceAPI = 0,
				PricePerDay = offer.Price,
				IsInsurance = offer.IsInsurance
			};
		}
	}
}
