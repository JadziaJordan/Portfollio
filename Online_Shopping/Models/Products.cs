using System.ComponentModel.DataAnnotations;

namespace Cloud_FinalTwo.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }

        public string ProductName { get; set; }
        public int Proudct_Qaunt { get; set; }
        public string Artist { get; set; }
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public bool ProductAvailibility { get; set; }
        public string ImageURL { get; set; }
        public string ProductCategory { get; set; }
    }
}
