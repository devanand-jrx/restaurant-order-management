using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RestaurantOrderManagement.Model;

namespace RestaurantOrderManagement.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.ItemName).IsRequired().HasMaxLength(100);

            builder.Property(oi => oi.Quantity).IsRequired();

            builder.Property(oi => oi.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId);
        }
    }
}
