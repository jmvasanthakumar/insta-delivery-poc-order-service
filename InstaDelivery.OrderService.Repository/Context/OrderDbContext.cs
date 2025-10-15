using InstaDelivery.OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace InstaDelivery.OrderService.Repository.Context
{
    internal class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
               .Property(o => o.Status)
               .HasConversion(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));

            //modelBuilder.Entity<Order>()
            //    .Property(o => o.DeliveryAddress)
            //    .HasConversion(
            //        v => v != null ? v.ToString() : null,
            //        v => v != null ? Newtonsoft.Json.Linq.JObject.Parse(v) : null
            //    );
        }
    }
}