using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Host.DbContextData.Entities;

namespace Order.Host.DbContextData.EntitiesConfigurations;

public class OrderItemEntityConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_item");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id)
            .HasColumnName("id")
            .UseIdentityColumn()
            .IsRequired();

        builder.HasOne(item => item.Order)
            .WithMany()
            .HasForeignKey(item => item.OrderId);

        builder.Property(item => item.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(item => item.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(item => item.SubPrice)
            .HasColumnName("sub_price");

        builder.Property(item => item.ItemId)
            .HasColumnName("item_id")
            .IsRequired();
    }
}