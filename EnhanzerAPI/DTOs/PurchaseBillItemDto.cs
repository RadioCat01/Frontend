namespace EnhanzerAPI.DTOs
{
    public class PurchaseBillItemDto
    {
        public string Item { get; set; } = string.Empty;
        public string Batch { get; set; } = string.Empty;
        public decimal StandardCost { get; set; }
        public decimal StandardPrice { get; set; }
        public int Qty { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalSelling { get; set; }
    }
}
