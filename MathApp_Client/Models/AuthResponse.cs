using System.ComponentModel.DataAnnotations;


namespace MathApp.Models
{
    public class AuthResponse
    {
        [Required]
        public string Token { get; set; }
        
    }
}