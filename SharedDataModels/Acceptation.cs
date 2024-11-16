using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class Acceptation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // change to none and replace with your own or smth
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime AcceptationDate { get; set; }

        public int ReturnID { get; set; }

        public int EmployeeID { get; set; }

        public int DescriptionID { get; set; }

        public Return Return { get; set; }

        public Description Description { get; set; }

        public ICollection<Image> Images { get; set; }


        //[FromForm]
        //[NotMapped]
        //public IFormFileCollection Files { get; set; }
    }
}
