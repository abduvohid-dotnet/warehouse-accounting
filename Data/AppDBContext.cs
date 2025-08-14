using Microsoft.EntityFrameworkCore;
using WarehouseAccounting.Models;

namespace WarehouseAccounting.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockMovement>()
                .Property(sm => sm.ProductID)
                .HasColumnName("ProductID");

            modelBuilder.Entity<StockMovement>()
                .Property(sm => sm.WarehouseID)
                .HasColumnName("WarehouseID");

            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.Product)
                .WithMany()
                .HasForeignKey(sm => sm.ProductID);

            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.Warehouse)
                .WithMany()
                .HasForeignKey(sm => sm.WarehouseID);
        }
    }
}
