namespace EnhanzerAPI.DTOs
{
    public class PurchaseOrderResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalSelling { get; set; }
        public int TotalItems { get; set; }
        public List<PurchaseBillItemDto> Items { get; set; } = new();
    }
}
