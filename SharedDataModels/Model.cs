using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharedDataModels
{
	public class Model
    {
        public int ID { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string Name { get; set; }

        public int BrandID { get; set; }

        public Brand Brand { get; set; }

        [JsonIgnore]
        public ICollection<Car> Cars { get; set; }
    }
}
