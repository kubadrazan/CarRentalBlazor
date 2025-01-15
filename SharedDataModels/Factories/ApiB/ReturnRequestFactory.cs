using SharedDataModels.Requests.ApiB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories.ApiB
{
    public class ReturnRequestFactory
    {
        public ReturnRequest CreateReturnRequest(Rental rental, User? user)
        {
            return new ReturnRequest()
            {
                rent_Id = rental.ID,
                client_Id = user.ID.ToString(),
                platform = "platformMiNICarRental",
                email = user.Email
            };
        }
    }
}
