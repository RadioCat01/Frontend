using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnhanzerAPI.DTOs;

namespace EnhanzerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseBillController : ControllerBase
    {
        private static readonly HashSet<string> AllowedItems = new(StringComparer.OrdinalIgnoreCase)
        {
            "Mango", "Apple", "Banana", "Orange", "Grapes", "Kiwi", "Strawberry"
        };

        private readonly ILogger<PurchaseBillController> _logger;

        public PurchaseBillController(ILogger<PurchaseBillController> logger)
        {
            _logger = logger;
        }

        [HttpPost("items")]
        public IActionResult ValidateItem([FromBody] PurchaseBillItemDto item)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(item.Item))
            {
                return BadRequest(new { message = "Item name is required" });
            }

            if (!AllowedItems.Contains(item.Item))
            {
                return BadRequest(new { message = "Select one of the supported items: Mango, Apple, Banana, Orange, Grapes, Kiwi, or Strawberry" });
            }

            if (string.IsNullOrWhiteSpace(item.Batch))
            {
                return BadRequest(new { message = "Batch is required" });
            }

            // Validate numeric fields
            if (item.Qty <= 0)
            {
                return BadRequest(new { message = "Quantity must be greater than zero" });
            }

            if (item.StandardCost < 0)
            {
                return BadRequest(new { message = "Standard cost cannot be negative" });
            }

            if (item.StandardPrice < 0)
            {
                return BadRequest(new { message = "Standard price cannot be negative" });
            }

            if (item.DiscountPercent < 0 || item.DiscountPercent > 100)
            {
                return BadRequest(new { message = "Discount percent must be between 0 and 100" });
            }

            // Calculate totals (server-side computation)
            item.TotalCost = (item.StandardCost * item.Qty) - (item.StandardCost * item.Qty * item.DiscountPercent / 100);
            item.TotalSelling = item.StandardPrice * item.Qty;

            // Return validated item with computed totals
            return Ok(item);
        }
    }
}
