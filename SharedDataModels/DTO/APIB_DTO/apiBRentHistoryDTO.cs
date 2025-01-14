using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SharedDataModels.DTO.APIB_DTO;

namespace SharedDataModels.DTO.APIB_DTO
{
    public class apiBRentHistoryDTO
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        public string client_Id { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        [JsonPropertyName("email")]
        public string UserEmail { get; set; }

        public string platform { get; set; }

        [JsonPropertyName("rentDate")]
        public DateTime RentDate { get; set; }

        public int offerId { get; set; }

        public apiBOfferDBDTO offer { get; set; }

        public bool isReturned { get; set; }

        public bool isReadyToReturn { get; set; }

        [JsonPropertyName("rentState")]
        public apiBRentStateDTO RentalStatus { get; set; }
    }
}
