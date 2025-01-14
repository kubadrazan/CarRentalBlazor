using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.DTO.APIB_DTO
{
	public class apiBOfferDTO
	{
		public int Id { get; set; }
		public int PriceDay { get; set; }
		public int PriceInsurance { get; set; }
		public DateTime ExpirationDate { get; set; }
		public bool IsSuccess { get; set; }
	}
}
