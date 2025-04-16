using System.ComponentModel.DataAnnotations;

namespace Cloud_FinalTwo.Models
{
    public class Proccessing
    {
        [Key]
        public int Process_ID { get; set; }
        public string shopperID { get; set; }
        public string pocessedProducts { get; set; }
        public string processedTotal { get; set; }
        public int? numberItems { get; set; }
        public bool isProcessed { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
