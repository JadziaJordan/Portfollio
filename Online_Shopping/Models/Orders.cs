using System.ComponentModel.DataAnnotations;

namespace Cloud_FinalTwo.Models
{
    public class Orders
    {
        [Key]
        public int order_id { get; set; }
        public int order_Number { get; set; }    
        public string emailOrder { get; set; }
        public string ordered_Products { get; set; }
        public string order_total { get; set; }
        public int? order_Qaunt { get; set; }
        public bool isProcessed { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
