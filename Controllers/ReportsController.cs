using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAccounting.Data;
using WarehouseAccounting.Models;

namespace WarehouseAccounting.Controllers
{
    [Route("reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ReportsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("get-reports-balance")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BalanceReportDto>>> GetBalance()
        {
            var balance = await _db.StockMovements
                .Include(sm => sm.Product)
                .GroupBy(sm => sm.ProductID)
                .Select(g => new BalanceReportDto
                {
                    ProductName = g.First().Product.Name,
                    Quantity = g.Sum(sm => sm.TypeOfMovement == "IN" ? sm.Quantity : -sm.Quantity)
                })
                .ToListAsync();

            return Ok(balance);
        }

        [HttpGet("get-reports-history")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<HistoryReportDto>>> GetHistory(
            DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue)
                startDate = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);

            if (endDate.HasValue)
                endDate = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);

            var query = _db.StockMovements
                .Include(sm => sm.Product)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(sm => sm.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(sm => sm.Date <= endDate.Value);

            var history = await query
                .OrderBy(sm => sm.Date)
                .Select(sm => new HistoryReportDto
                {
                    Date = sm.Date,
                    TypeOfMovement = sm.TypeOfMovement,
                    ProductName = sm.Product.Name,
                    Quantity = sm.Quantity
                })
                .ToListAsync();

            return Ok(history);
        }
    }
}
