using Catalog.Host.DbContextData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.DbContextData.EntityConfig;

public class ItemBrandEntityConfiguration: IEntityTypeConfiguration<ItemBrand>
{
    public void Configure(EntityTypeBuilder<ItemBrand> builder)
    {
        builder.ToTable("item_brand");
        builder.HasKey(brand => brand.Id);
        builder.Property(brand => brand.Id)
            .HasColumnName("id")
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(brand => brand.Brand)
            .HasColumnName("brand")
            .IsRequired()
            .HasMaxLength(50);
    }
}