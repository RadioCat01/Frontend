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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LocationDetail>().ToTable("Location_Details");
        }
    }
}
