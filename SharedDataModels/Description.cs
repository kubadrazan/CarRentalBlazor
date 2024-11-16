using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class Description
    {
        public int ID { get; set; }

        [MaxLength]
        public string Content { get; set; }

        public ICollection<Acceptation> Acceptations { get; set; }
    }
}
