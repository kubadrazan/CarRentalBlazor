using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories
{
	public class ReturnFactory(TimeProvider timeProvider)
	{
		public Return CreateReturn(int rentalId)
		{
			return new Return
			{
				ReturnDate = timeProvider.GetUtcNow().DateTime,
				RentalID = rentalId
            };
		}
	}
}
