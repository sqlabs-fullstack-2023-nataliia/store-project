using Catalog.Host.DbContextData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.DbContextData.EntityConfig;

public class ItemCategoryEntityConfiguration: IEntityTypeConfiguration<ItemCategory>
{
    public void Configure(EntityTypeBuilder<ItemCategory> builder)
    {
        builder.ToTable("item_category");
        builder.HasKey(category => category.Id);
        builder.Property(category => category.Id)
            .HasColumnName("id")
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(category => category.Category)
            .HasColumnName("category")
            .IsRequired()
            .HasMaxLength(50);
    }

}