namespace xact_ERP_Second.Models
{
    public class Debtor
    {
        public string AccountCode { get; set; }
        public string Name { get; set; }
        public string DeliveryAddress { get; set; }
        public string InvoiceAddress { get; set; }
        public string PostalAddress { get; set; }
        public string AccountHolder { get; set; }
        public int AcoountNumber { get; set; } 
        public string Branch { get; set; }

        public decimal Balance { get; set; } 
        public decimal SalesYearToDate { get; set; } 
        public decimal CostYearToDate { get; set; }  
    }
}
