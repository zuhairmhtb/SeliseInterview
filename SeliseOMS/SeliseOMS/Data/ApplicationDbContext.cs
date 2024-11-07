using Microsoft.EntityFrameworkCore;
using SeliseOMS.Models.Orders;
using System.Data;

namespace SeliseOMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> Lines { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(e => {
                e.ToTable("Order");
            });

            modelBuilder.Entity<OrderLine>(e => {
                e.ToTable("OrderLine");
            });
            modelBuilder.Entity<Product>(e => {
                e.ToTable("Product");
            });
        }
    }
}
