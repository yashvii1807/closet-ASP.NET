using System.ComponentModel.DataAnnotations;

namespace Closet_ASP.NET.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Subject must be between 5 and 100 characters")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 1000 characters")]
        public string Message { get; set; }
    }
}
