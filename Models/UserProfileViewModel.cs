using System.ComponentModel.DataAnnotations;

namespace Closet_ASP.NET.Models
{
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^\d{10,13}$", ErrorMessage = "Phone number must be between 10 to 13 digits")]
        public string Contact { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string JoinDate { get; set; }
    }
}
