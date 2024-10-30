using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RestaurantOrderManagement.Enum;
using RestaurantOrderManagement.Model;

namespace RestaurantOrderManagement.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.TableNumber).IsRequired();
            builder.Property(o => o.Status).IsRequired().HasConversion<int>();
            builder.Property(o => o.OrderTime).IsRequired();
            builder.Property(o => o.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);
            // Seed data
            builder.HasData(
                new Order
                {
                    Id = 1,
                    TableNumber = 5,
                    Status = OrderStatus.New,
                    OrderTime = DateTime.UtcNow,
                    TotalAmount = 25.90m
                }
            );
        }
    }
}
