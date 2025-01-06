using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
	public class AcceptReturnRequest
	{
        public AcceptReturnRequest(string employeeEmail, string returnDescription, string base64EncodedCarImage)
        {
            EmployeeEmail = employeeEmail;
            ReturnDescription = returnDescription;
            Base64EncodedCarImage = base64EncodedCarImage;
        }

        public string EmployeeEmail { get; set; }

		public string ReturnDescription { get; set; }

        public string Base64EncodedCarImage { get; set; }
	}
}
