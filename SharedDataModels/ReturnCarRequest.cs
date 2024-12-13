using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
	public class ReturnCarRequest
	{
		public ReturnCarRequest(Rental rental, float latitude, float longitude) { 
			EmailAddress = rental.UserEmail;
			Latitude = latitude;
			Longitude = longitude;
		}
		public ReturnCarRequest() { }
		public string EmailAddress { get; set; }
		public float Latitude { get; set; }
		public float Longitude { get; set; }
	}
}
