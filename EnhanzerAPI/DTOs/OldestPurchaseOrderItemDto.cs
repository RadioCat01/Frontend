namespace EnhanzerAPI.DTOs
{
    public class OldestPurchaseOrderItemDto
    {
        public int PurchaseOrderId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int NoOfQuantity { get; set; }
    }
}
