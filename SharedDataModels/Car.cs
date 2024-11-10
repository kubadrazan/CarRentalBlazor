using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public Availability Availability { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public float PricePerDay { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public float InsurancePricePerDat { get; set; }

        public int ModelID { get; set; }
        public int LocalizationID { get; set; }

        public Model Model { get; set; }
        public Localization Localization { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
