using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCarRentalAPI.Models
{
    public class Rental
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // change to none and replace with your own or smth
        public int ID { get; set; }

        public DateTime RentDate { get; set; }

        public int CarID { get; set; }
        public int UserID { get; set; }
        public int SourceAPI {  get; set; } // TODO don't know what here, ENUM????

        public Car Car { get; set; }
    }
}
