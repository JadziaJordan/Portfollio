using System;
using System.Collections.Generic;
using System.Text;

namespace xact_ERP_Second.Models
{
    class Stock
    {
        public string StockCode { get; set; }
       
        public string StockName { get; set; }
        public string StockDescription { get; set; }

        public string Brand { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public decimal Cost { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal TotalPurchasedExclVat { get; set; }
        
        public decimal TotalSalesExclVat { get; set; }
        public int QntyPurchased {  get; set; }
        public int QntySold { get; set; }
        public int StockOnHand { get; set; }
        
    }
}
