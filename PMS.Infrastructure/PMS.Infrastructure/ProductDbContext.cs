using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using PMS.Domain;

namespace PMS.Infrastructure
{
    public class ProductDbContext : DbContextWithTriggers
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("pms");
        }
    }
}