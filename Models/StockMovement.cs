using System;
using WarehouseAccounting.Models;

public class StockMovement
{
    public int Id { get; set; }

    public int ProductID { get; set; }
    public Product Product { get; set; }

    public int WarehouseID { get; set; }
    public Warehouse Warehouse { get; set; }

    public string TypeOfMovement { get; set; } // "IN" yoki "OUT"
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public string DocumentNumber { get; set; }
}
