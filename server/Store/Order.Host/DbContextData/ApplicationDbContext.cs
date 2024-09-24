using Microsoft.EntityFrameworkCore;
using Order.Host.DbContextData.Entities;
using Order.Host.DbContextData.EntitiesConfigurations;

namespace Order.Host.DbContextData;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<CatalogOrder> CatalogOrders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CatalogOrderEntityConfiguration());
        builder.ApplyConfiguration(new OrderItemEntityConfiguration());
    }
}