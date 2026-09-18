namespace EnhanzerAPI.Models
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public string Item { get; set; } = string.Empty;
        public string Batch { get; set; } = string.Empty;
        public decimal StandardCost { get; set; }
        public decimal StandardPrice { get; set; }
        public int Qty { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalSelling { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property
        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}
