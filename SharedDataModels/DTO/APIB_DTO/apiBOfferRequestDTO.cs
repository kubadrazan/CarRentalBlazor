using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.DTO.APIB_DTO
{
	public class apiBOfferRequestDTO
	{
		public int CarId { get; set; }
		public int DriversLicenceDuration { get; set; }
		public int Age { get; set; }
		public DateTime Start { get; set; }
		public DateTime Return { get; set; }
		public string? ExtraInfo { get; set; }
	}
}
