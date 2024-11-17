using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedDataModels
{
    public class User
    {
        public int ID { get; set; }

        [StringLength(25, MinimumLength = 2,
            ErrorMessage = "First Name should be between 2 and 25 characters.")]
        public string FirstName { get; set; }

        [StringLength(25, MinimumLength = 2,
            ErrorMessage = "Last Name should be between 2 and 25 characters.")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DrivingLicenseObtainDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        // todo add password or some auth

        public int LocalizationID { get; set; }

        public LocalizationBrowser Localization { get; set; }

        [JsonIgnore]
        public ICollection<RentalBrowser> Rentals { get; set; }
    }
}
