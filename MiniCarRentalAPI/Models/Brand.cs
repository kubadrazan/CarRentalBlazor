using System.ComponentModel.DataAnnotations;

namespace MiniCarRentalAPI.Models
{
    public class Brand
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }

        public ICollection<Model> Models { get; set; }

    }
}
