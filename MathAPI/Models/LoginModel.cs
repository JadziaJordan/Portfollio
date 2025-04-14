using System.ComponentModel.DataAnnotations;

namespace MathAPI.Models
{
    // This model represents the structure of login data sent from the client
    public class LoginModel
    {
        // The user's email address. It's required and must be in a valid email format.
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        // The user's password. It's required, but no specific constraints are added here.
        [Required]
        public required string Password { get; set; }
    }
}
