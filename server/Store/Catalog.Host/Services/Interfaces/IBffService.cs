using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Models;

namespace Catalog.Host.Services.Interfaces;

public interface IBffService
{
    Task IncreaseItemQuantity(List<OrderItem> items);
    Task DecreaseItemQuantity(List<OrderItem> items);
    Task<List<Item>> GetItemsByCatalogItemId(int catalogItemId);
    Task<List<CatalogItem>> GetCatalogItems(CatalogFilter filters);
    Task<List<Item>> GetItems();
    Task<List<ItemBrand>> GetBrands();
    Task<List<ItemType>> GetTypes();
    Task<List<ItemCategory>> GetCategories();
    Task<CatalogItem> GetCatalogItem(int id);
    Task<ItemBrand> GetBrand(int id);
    Task<ItemType> GetType(int id);
    Task<ItemCategory> GetCategory(int id);
    Task<Item> GetItem(int id);

}