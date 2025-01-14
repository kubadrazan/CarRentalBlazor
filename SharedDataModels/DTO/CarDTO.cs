using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedDataModels.DTO
{
    public class CarDTO
    {
        public int ID { get; set; }

        public string ModelName { get; set; }

        public string BrandName { get; set; }

        public int ProductionYear { get; set; }

        public int HorsePower { get; set; }

        public string FuelType { get; set; }

        public string Drive { get; set; }

        public string Transmission { get; set; }

        public int DoorsNumber { get; set; }

        public string Colour { get; set; }

        public int Availability { get; set; }

        public float PricePerDay { get; set; }

        public float InsurancePricePerDay { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
    }
}
