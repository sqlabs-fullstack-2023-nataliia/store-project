using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Host.DbContextData.Entities;

namespace Order.Host.DbContextData.EntitiesConfigurations;

public class CatalogOrderEntityConfiguration: IEntityTypeConfiguration<CatalogOrder>
{
    public void Configure(EntityTypeBuilder<CatalogOrder> builder)
    {
        builder.ToTable("catalog_order");
        builder.HasKey(order => order.Id);
        builder.Property(order => order.Id)
            .HasColumnName("id")
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(order => order.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(order => order.TotalQuantity)
            .HasColumnName("total_quantity");

        builder.Property(order => order.TotalPrice)
            .HasColumnName("total_price");

        builder.Property(order => order.UserId)
            .HasColumnName("user_id")
            .IsRequired();
    }
}