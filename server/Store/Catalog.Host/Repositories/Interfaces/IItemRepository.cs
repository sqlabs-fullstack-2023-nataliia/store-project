using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Models;

namespace Catalog.Host.Repositories.Interfaces;

public interface IItemRepository: ICatalogRepository<Item>
{
    Task<List<Item>> GetItemsByCatalogItemId(int catalogItemId);
    Task DecreaseItemQuantity(List<OrderItem> items);
}