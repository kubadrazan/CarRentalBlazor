using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class RentalBrowser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int UserID { get; set; } 

        public int ApiID { get; set; }

        public int SourceApiID { get; set; }

        public User User { get; set; }
    }
}

