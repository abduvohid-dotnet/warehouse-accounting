using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAccounting.Data;
using WarehouseAccounting.Models;

namespace WarehouseAccounting.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-all-products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _db.Products.ToListAsync();
            if (products == null || !products.Any())
                return NoContent();
            return Ok(products);
        }

        [HttpGet("get-product/{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (id <= 0)
                return BadRequest();

            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("create-product")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(product.Name))
                return BadRequest();

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("update-product/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id <= 0 || id != product.Id)
                return BadRequest();

            var existingProduct = await _db.Products.FindAsync(id);
            if (existingProduct == null)
                return NotFound();

            existingProduct.Name = product.Name;
            existingProduct.SKU = product.SKU;
            existingProduct.Price = product.Price;
            existingProduct.MinStock = product.MinStock;

            _db.Entry(existingProduct).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("delete-product/{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
                return BadRequest();

            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
