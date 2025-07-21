using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Prog7311_PartTwo.Models
{
    public class FarmerProfileModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }  // This links to IdentityUser.Id

        [ValidateNever]
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }  // Navigation property
    }
}
