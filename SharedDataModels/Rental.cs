using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace SharedDataModels
{
	public class Rental
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // change to none and replace with your own or smth
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime RentDate { get; set; }

        public int CarID { get; set; }
        public int UserID { get; set; } // TODO zaktualizowac email?, id tokenu?
        public int SourceAPI {  get; set; } // TODO don't know what here, ENUM????

        public Car Car { get; set; }

        // for navigation?
        //public Return? Return { get; set; }
    }
}
