using Catalog.Host.DbContextData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.DbContextData.EntityConfig;

public class CatalogItemEntityConfiguration: IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("catalog_item");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id)
            .HasColumnName("id")
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(item => item.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(item => item.ItemBrand)
            .WithMany()
            .HasForeignKey(item => item.ItemBrandId);
        
        builder.Property(item => item.ItemBrandId)
            .HasColumnName("brand_id");

        builder.HasOne(item => item.ItemType)
            .WithMany()
            .HasForeignKey(item => item.ItemTypeId);

        builder.Property(item => item.ItemTypeId)
            .HasColumnName("type_id");

        builder.Property(item => item.Price)
            .HasColumnName("price")
            .IsRequired();

        builder.Property(item => item.Image)
            .HasColumnName("image")
            .HasMaxLength(150);

        builder.HasOne(item => item.ItemCategory)
            .WithMany()
            .HasForeignKey(item => item.ItemCategoryId);

        builder.Property(item => item.ItemCategoryId)
            .HasColumnName("category_id");

        builder.Property(item => item.Description)
            .HasColumnName("description")
            .HasMaxLength(150);
    }
}