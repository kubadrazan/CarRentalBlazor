using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories
{
	public class ReturnFactory
	{
		public Return CreateReturn(int rentalId, Location location)
		{
			return new Return
			{
				ReturnDate = DateTime.UtcNow,
				RentalID = rentalId,
                Location = location
			};
		}
	}
}
