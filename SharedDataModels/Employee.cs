using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedDataModels
{
    [Index(nameof(Email))]
    public class Employee
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
    }
}
