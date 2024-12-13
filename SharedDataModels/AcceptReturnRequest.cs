using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
	public class AcceptReturnRequest
	{
        public AcceptReturnRequest(string employeeEmail, string returnDescription)
        {
            EmployeeEmail = employeeEmail;
            ReturnDescription = returnDescription;
        }

        public string EmployeeEmail { get; set; }
		public string ReturnDescription { get; set; }
	}
}
