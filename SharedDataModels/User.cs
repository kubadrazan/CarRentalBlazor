using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SharedDataModels
{
    [Index(nameof(Email))]
    public class User
    {
        public int ID { get; set; }

        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "First Name should be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Last Name should be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DrivingLicenseObtainDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [JsonIgnore]
        public ICollection<RentalBrowser> Rentals { get; set; }
    }
}
