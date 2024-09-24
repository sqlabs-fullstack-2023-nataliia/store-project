using Catalog.Host.DbContextData.Entities;
using Catalog.Host.DbContextData.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.DbContextData;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<ItemBrand> ItemBrands { get; set; }
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CatalogItemEntityConfiguration());
        builder.ApplyConfiguration(new ItemBrandEntityConfiguration());
        builder.ApplyConfiguration(new ItemCategoryEntityConfiguration());
        builder.ApplyConfiguration(new ItemTypeEntityConfiguration());
        builder.ApplyConfiguration(new ItemEntityConfiguration());
    }
 }