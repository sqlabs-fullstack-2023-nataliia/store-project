using Catalog.Host.DbContextData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.DbContextData.EntityConfig;

public class ItemTypeEntityConfiguration: IEntityTypeConfiguration<ItemType>
{
    public void Configure(EntityTypeBuilder<ItemType> builder)
    {
        builder.ToTable("item_type");
        builder.HasKey(type => type.Id);
        builder.Property(type => type.Id)
            .HasColumnName("id")
            .UseIdentityColumn()
            .IsRequired();

        builder.Property(type => type.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasMaxLength(50);
    }
}