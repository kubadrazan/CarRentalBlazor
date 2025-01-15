using SharedDataModels.Requests.ApiB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Factories.ApiB
{
    public class OfferChoiceFactory
    {
        public OfferChoice CreateOfferChoice(int offerId, User? user)
        {
            return new OfferChoice()
            {
                client_Id = user.ID.ToString(),
                offer_Id = offerId,
                platform = "platformMiNICarRental",
                name = user.FirstName,
                surname = user.LastName,
                email = user.Email
            };
        }
    }
}
