using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.Requests.ApiB
{
    public class OfferChoice
    {
        public string client_Id {  get; set; }

        public int offer_Id { get; set; }

        public string platform { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        public string email { get; set; }
    }
}
