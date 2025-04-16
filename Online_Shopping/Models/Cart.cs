using System.ComponentModel.DataAnnotations;

namespace Cloud_FinalTwo.Models
{
    public class Cart
    {
        [Key]
        public int? CartId { get; set; }
        public string? CartUser { get; set; }

        public int? ProductID { get; set; }
        public virtual Products Product { get; set; } 

        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public string? Artist { get; set; }
    }
}
