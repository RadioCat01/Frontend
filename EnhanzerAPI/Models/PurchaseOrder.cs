namespace EnhanzerAPI.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalSelling { get; set; }
        public int TotalItems { get; set; }

        // Navigation property
        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}
