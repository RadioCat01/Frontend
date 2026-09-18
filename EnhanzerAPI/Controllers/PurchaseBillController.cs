using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnhanzerAPI.DTOs;
using EnhanzerAPI.Data;
using EnhanzerAPI.Models;

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
        private readonly AppDbContext _context;

        public PurchaseBillController(ILogger<PurchaseBillController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("items")]
        public IActionResult ValidateItem([FromBody] PurchaseBillItemDto item)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(item?.Item))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating item");
                return StatusCode(500, new { message = "Error processing item validation", error = ex.Message });
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SavePurchaseOrder([FromBody] CreatePurchaseOrderDto dto)
        {
            if (dto?.Items == null || dto.Items.Count == 0)
            {
                return BadRequest(new { message = "Purchase order must contain at least one item" });
            }

            // Validate all items first
            foreach (var item in dto.Items)
            {
                if (string.IsNullOrWhiteSpace(item.Item) || !AllowedItems.Contains(item.Item))
                {
                    return BadRequest(new { message = $"Invalid item: {item.Item}" });
                }
                if (string.IsNullOrWhiteSpace(item.Batch) || item.Qty <= 0)
                {
                    return BadRequest(new { message = "All items must have valid batch, cost, price, and quantity" });
                }
            }

            try
            {
                var purchaseOrder = new PurchaseOrder
                {
                    CreatedDate = DateTime.UtcNow,
                    TotalCost = dto.Items.Sum(i => i.TotalCost),
                    TotalSelling = dto.Items.Sum(i => i.TotalSelling),
                    TotalItems = dto.Items.Count,
                    Items = dto.Items.Select(i => new PurchaseOrderItem
                    {
                        Item = i.Item,
                        Batch = i.Batch,
                        StandardCost = i.StandardCost,
                        StandardPrice = i.StandardPrice,
                        Qty = i.Qty,
                        DiscountPercent = i.DiscountPercent,
                        TotalCost = i.TotalCost,
                        TotalSelling = i.TotalSelling,
                        CreatedDate = DateTime.UtcNow
                    }).ToList()
                };

                _context.PurchaseOrders.Add(purchaseOrder);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Purchase order saved successfully", id = purchaseOrder.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving purchase order");
                return StatusCode(500, new { message = "Error saving purchase order" });
            }
        }

        [HttpGet("latest")]
        public IActionResult GetLatestPurchaseOrders()
        {
            try
            {
                var latestOrders = _context.PurchaseOrders
                    .OrderByDescending(p => p.CreatedDate)
                    .Take(5)
                    .Select(p => new LatestPurchaseOrderDto
                    {
                        Id = p.Id,
                        NetAmount = p.TotalCost,
                        NoOfItems = p.TotalItems
                    })
                    .ToList();

                return Ok(latestOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest purchase orders");
                return StatusCode(500, new { message = "Error retrieving purchase orders" });
            }
        }

        [HttpGet("oldest-items")]
        public IActionResult GetOldestPurchaseOrderItems()
        {
            try
            {
                var oldestItems = _context.PurchaseOrderItems
                    .OrderBy(i => i.CreatedDate)
                    .Take(10)
                    .Select(i => new OldestPurchaseOrderItemDto
                    {
                        PurchaseOrderId = i.PurchaseOrderId,
                        ItemName = i.Item,
                        NoOfQuantity = i.Qty
                    })
                    .ToList();

                return Ok(oldestItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving oldest purchase order items");
                return StatusCode(500, new { message = "Error retrieving purchase order items" });
            }
        }

        [HttpGet("grouped-items")]
        public IActionResult GetGroupedItems()
        {
            try
            {
                var groupedItems = _context.PurchaseOrderItems
                    .GroupBy(i => i.Item)
                    .Select(g => new GroupedItemDto
                    {
                        ItemName = g.Key,
                        TotalQuantity = g.Sum(i => i.Qty)
                    })
                    .ToList();

                return Ok(groupedItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving grouped items");
                return StatusCode(500, new { message = "Error retrieving grouped items" });
            }
        }
    }
}
