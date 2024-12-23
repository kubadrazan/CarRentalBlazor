using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories
{
	public class AcceptationFactory
	{
		public Acceptation CreateAcceptation(Return carReturn, string employeeEmail, string returnDescription)
		{
			return new Acceptation
			{
				AcceptationDate = DateTime.UtcNow,
				ReturnID = carReturn.ID,
				EmployeeEmail = employeeEmail,
				Description = new Description
				{
					Content = returnDescription
				}
			};
		}
	}
}
