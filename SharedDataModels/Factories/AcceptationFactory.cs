using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories
{
	public class AcceptationFactory(TimeProvider timeProvider)
	{
		public Acceptation CreateAcceptation(Return carReturn, string employeeEmail, string returnDescription, string imageUri)
		{
			return new Acceptation
			{
				AcceptationDate = timeProvider.GetUtcNow().DateTime,
				ReturnID = carReturn.ID,
				EmployeeEmail = employeeEmail,
				Description = new Description
				{
					Content = returnDescription
				},
				ImageAzureBlobUri = imageUri
			};
		}
	}
}
