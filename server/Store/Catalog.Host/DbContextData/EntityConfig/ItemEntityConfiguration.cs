using Catalog.Host.DbContextData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.DbContextData.EntityConfig;

public class ItemEntityConfiguration: IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("stock");
        builder.HasKey(stock => stock.Id);
        builder.Property(stock => stock.Id)
            .HasColumnName("id")
            .UseIdentityColumn()
            .IsRequired();

        builder.HasOne(stock => stock.CatalogItem)
            .WithMany()
            .HasForeignKey(stock => stock.CatalogItemId);

        builder.Property(stock => stock.CatalogItemId)
            .HasColumnName("catalog_item_id")
            .IsRequired();

        builder.Property(stock => stock.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(stock => stock.Size)
            .HasColumnName("size")
            .HasMaxLength(50)
            .IsRequired();
    }
}