using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class RentalBrowser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // change to none and replace with your own or smth
        public int ID { get; set; }

        public DateTime RentDate { get; set; }

        public int CarID { get; set; }
        public int UserID { get; set; } 
        public int SourceAPI { get; set; }

        public User User { get; set; }
    }
}

