using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAccounting.Data;
using WarehouseAccounting.Models;

namespace WarehouseAccounting.Controllers
{
    [Route("stock-movements")]
    [ApiController]
    public class StockMovementsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public StockMovementsController(AppDbContext db)
        {
            _db = db;
        }

        // 🔹 GET: Barcha harakatlarni olish
        [HttpGet("get-all-stock-movements")]
        public async Task<ActionResult<IEnumerable<StockMovement>>> GetAllStockMovements()
        {
            var stockMovements = await _db.StockMovements
                .Include(sm => sm.Product)
                .Include(sm => sm.Warehouse)
                .ToListAsync();

            return Ok(stockMovements);
        }

        // 🔹 GET: ID bo‘yicha olish
        [HttpGet("get-stock-movement/{id:int}")]
        public async Task<ActionResult<StockMovement>> GetStockMovement(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            var stockMovement = await _db.StockMovements
                .Include(sm => sm.Product)
                .Include(sm => sm.Warehouse)
                .FirstOrDefaultAsync(sm => sm.Id == id);

            if (stockMovement == null)
                return NotFound();

            return Ok(stockMovement);
        }

        [HttpPost("create-stock-movement-in")]
        public async Task<ActionResult> CreateStockMovementIn([FromBody] StockMovement stockMovement)
        {
            return await CreateStockMovement(stockMovement, "IN");
        }

        [HttpPost("create-stock-movement-out")]
        public async Task<ActionResult> CreateStockMovementOut([FromBody] StockMovement stockMovement)
        {
            return await CreateStockMovement(stockMovement, "OUT");
        }

        private async Task<ActionResult> CreateStockMovement(StockMovement stockMovement, string type)
        {
            try
            {
                if (stockMovement == null)
                    return BadRequest();

                if (stockMovement.Quantity <= 0)
                    return BadRequest();

                stockMovement.Id = 0;

                stockMovement.TypeOfMovement = type;
                stockMovement.Date = DateTime.UtcNow;

                stockMovement.Product = null;
                stockMovement.Warehouse = null;

                bool productExists = await _db.Products.AnyAsync(p => p.Id == stockMovement.ProductID);
                if (!productExists)
                    return BadRequest();

                bool warehouseExists = await _db.Warehouses.AnyAsync(w => w.Id == stockMovement.WarehouseID);
                if (!warehouseExists)
                    return BadRequest();

                _db.StockMovements.Add(stockMovement);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStockMovement), new { id = stockMovement.Id }, stockMovement);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
