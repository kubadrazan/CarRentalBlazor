using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharedDataModels
{
    public class Brand
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Model> Models { get; set; }

    }
}
