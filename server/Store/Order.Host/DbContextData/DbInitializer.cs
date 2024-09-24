using Order.Host.DbContextData.Entities;

namespace Order.Host.DbContextData;

public class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.CatalogOrders.Any())
        {
            await context.CatalogOrders.AddRangeAsync(GetPreconfiguredCatalogOrders());

            await context.SaveChangesAsync();

        }
        
        if (!context.OrderItems.Any())
        {
            await context.OrderItems.AddRangeAsync(GetPreconfiguredOrderItems());

            await context.SaveChangesAsync();

        }
    }

    private static IEnumerable<CatalogOrder> GetPreconfiguredCatalogOrders()
    {
        return new List<CatalogOrder>
        {
            new() { Date = DateTime.Now.ToShortDateString(), UserId = "1" },
            new() { Date = DateTime.Now.ToShortDateString(), UserId = "2" },
            new() { Date = DateTime.Now.ToShortDateString(), UserId = "3" },
            new() { Date = DateTime.Now.ToShortDateString(), UserId = "3" },
            new() { Date = DateTime.Now.ToShortDateString(), UserId = "2" }
        };
    }
    
    private static IEnumerable<OrderItem> GetPreconfiguredOrderItems()
    {
        return new List<OrderItem>
        {
            new() {OrderId = 1, Quantity = 2, ItemId = 1},
            new() {OrderId = 1, Quantity = 1, ItemId = 2},
            new() {OrderId = 1, Quantity = 1, ItemId = 3},
            new() {OrderId = 2, Quantity = 2, ItemId = 1},
            new() {OrderId = 3, Quantity = 1, ItemId = 1},
            new() {OrderId = 3, Quantity = 3, ItemId = 2},
            new() {OrderId = 3, Quantity = 1, ItemId = 3},
            new() {OrderId = 4, Quantity = 2, ItemId = 1},
            new() {OrderId = 5, Quantity = 3, ItemId = 1},
        };
    }
}