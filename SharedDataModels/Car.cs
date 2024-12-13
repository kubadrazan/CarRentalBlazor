using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SharedDataModels
{
    public enum Availability
    {
        AVAILABLE, NOT_AVAILABLE
    }

    public class Car
    {
        public int ID { get; set; }

        public int ProductionYear { get; set; }

        public int HorsePower {  get; set; }

        public string FuelType { get; set; }

        public string Drive { get; set; }

        public string Transmission {  get; set; }

        public int DoorsNumber { get; set; }

        public string Colour { get; set; }

        public Availability Availability { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public float PricePerDay { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public float InsurancePricePerDay { get; set; }

        public int ModelID { get; set; }

		//public Location Location { get; set; } // TODO
		public float Latitude { get; set; }

		public float Longitude { get; set; }

		public Model Model { get; set; }

        [JsonIgnore]
        public ICollection<Rental> Rentals { get; set; }
    }
}
