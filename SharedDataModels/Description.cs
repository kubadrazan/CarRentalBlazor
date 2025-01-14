using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class Description
    {
        public int ID { get; set; }

        [StringLength(500, MinimumLength = 1)]
        public string Content { get; set; }

        [JsonIgnore]
        public ICollection<Acceptation> Acceptations { get; set; }
    }
}
