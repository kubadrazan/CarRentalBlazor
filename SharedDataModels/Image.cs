using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class Image
    {
        public int ID { get; set; }

        public byte[] Bytes { get; set; }

        public int AcceptationId { get; set; }

        public Acceptation Acceptation { get; set; }
    }
}
