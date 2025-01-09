using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels.DTO.APIB_DTO
{
    public class apiBCarDTO
    {
        public int id { get; set; }

        public string licensePlate { get; set; }

        public string carBrand { get; set; }

        public string carModel { get; set; }

        public bool isRented { get; set; }

        public string localization { get; set; }
    }
}
    