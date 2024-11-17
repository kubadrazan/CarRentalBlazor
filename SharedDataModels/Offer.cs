using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
	public class Offer
	{
		public int ID {  get; set; }

		public bool IsInsurance { get; set; }

		public int CarId { get; set; }

		public float Price { get; set; }
	}
}
