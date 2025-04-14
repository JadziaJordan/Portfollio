using System.ComponentModel.DataAnnotations;

namespace MathAPI.Models
{
    // This class represents the response object returned after successful authentication (e.g., login or register)
    public class AuthResponse
    {
        // This property holds the Firebase user ID (or later, a JWT token)
        // The [Required] attribute ensures this field must be provided/validated if used in model binding
        [Required]
        public string Token { get; set; }

        // Constructor initializes the Token property
        public AuthResponse(string token)
        {
            this.Token = token;
        }
    }
}
