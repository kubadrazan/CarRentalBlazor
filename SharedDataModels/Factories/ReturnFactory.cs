using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories
{
	public class ReturnFactory
	{
		public Return CreateReturn(int rentalId, float latitude, float longitude)
		{
			return new Return
			{
				ReturnDate = DateTime.Now,
				RentalID = rentalId,
                Location = new Location { Latitude = latitude, Longitude = longitude }
            };
		}
	}
}
