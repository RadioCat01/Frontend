namespace EnhanzerAPI.DTOs
{
    public class CreatePurchaseOrderDto
    {
        public List<PurchaseBillItemDto> Items { get; set; } = new();
    }
}
