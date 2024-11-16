using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class Return
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // change to none and replace with your own or smth
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        public int RentalID { get; set; }

        public int LocalizationID { get; set; }

        public Localization Localization { get; set; }

        public Rental Rental { get; set; }

        public Acceptation? Acceptation { get; set; }
    }
}
