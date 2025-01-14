using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedDataModels.DTO.APIB_DTO
{
    public class apiBOfferDBDTO
    {
        public int id { get; set; }

        public int priceDay { get; set; }

        public int priceInsurance { get; set; }

        public DateTime expirationDate { get; set; }

        public DateTime whenOfferWasMade { get; set; }

        public int carId { get; set; }

        public apiBCarDTO car { get; set; }
    }
}
