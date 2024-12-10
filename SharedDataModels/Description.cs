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

        [MaxLength] // todo
        public string Content { get; set; }

        [JsonIgnore]
        public ICollection<Acceptation> Acceptations { get; set; }
    }
}
