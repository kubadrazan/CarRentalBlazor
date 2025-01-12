using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace SharedDataModels
{
    public enum RentalStatus
    {
        ACTIVE = 0, RETURNED = 1, CLOSED = 2, NOT_ACCEPTED = 3
    }
    public class Rental
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid OfferGuid { get; set; }

        public RentalStatus RentalStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime RentDate { get; set; }

        public int CarID { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string UserEmail { get; set; }
        public int SourceAPI {  get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public float PricePerDay { get; set; }

        public bool IsInsurance { get; set; }

        public Car Car { get; set; }

        public Return? Return { get; set; }
    }
}
