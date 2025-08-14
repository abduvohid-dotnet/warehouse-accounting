namespace WarehouseAccounting.Models
{
    public class BalanceReportDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public class HistoryReportDto
    {
        public DateTime Date { get; set; }
        public string TypeOfMovement { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}