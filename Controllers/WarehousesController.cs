using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAccounting.Models;
using WarehouseAccounting.Data;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace WarehouseAccounting.Controllers
{
    [Route("warehouses")]
    [ApiController]

    public class WarehousesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public WarehousesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-all-warehouses")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetAllWarehouses()
        {
            var warehouses = await _db.Warehouses.ToListAsync();
            return Ok(warehouses);
        }

        [HttpGet("get-warehouse/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            var warehouse = await _db.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound();

            return Ok(warehouse);
        }

        [HttpPost("create-warehouse")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Warehouse>> CreateWarehouse([FromBody] Warehouse warehouse)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _db.Warehouses.Add(warehouse);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.Id }, warehouse);
        }

        [HttpPut("update-warehouse/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateWarehouse(int id, [FromBody] Warehouse warehouse)
        {
            if (id != warehouse.Id) return BadRequest();

            _db.Entry(warehouse).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("delete-warehouse/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteWarehouse(int id)
        {
            var warehouse = await _db.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound();

            _db.Warehouses.Remove(warehouse);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}