using Microsoft.EntityFrameworkCore;
using EnhanzerAPI.Models;

namespace EnhanzerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<LocationDetail> LocationDetails { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LocationDetail>().ToTable("Location_Details");

            // PurchaseOrder configuration
            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(p => p.Items)
                .WithOne(i => i.PurchaseOrder)
                .HasForeignKey(i => i.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // PurchaseOrderItem decimal precision
            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.StandardCost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.StandardPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.TotalCost)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.TotalSelling)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.DiscountPercent)
                .HasPrecision(5, 2);

            // PurchaseOrder decimal precision
            modelBuilder.Entity<PurchaseOrder>()
                .Property(p => p.TotalCost)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(p => p.TotalSelling)
                .HasPrecision(12, 2);
        }
    }
}
