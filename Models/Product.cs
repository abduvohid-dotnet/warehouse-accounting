using System.ComponentModel.DataAnnotations;

namespace WarehouseAccounting.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int MinStock { get; set; }
    }
}

