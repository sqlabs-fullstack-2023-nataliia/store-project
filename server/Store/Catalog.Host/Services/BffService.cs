using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Models;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using ExceptionHandler;

namespace Catalog.Host.Services;

public class BffService: IBffService
{
    private readonly ICatalogRepository<ItemBrand> _brandRepository;
    private readonly ICatalogRepository<ItemType> _typeRepository;
    private readonly ICatalogRepository<ItemCategory> _categoryRepository;
    private readonly IItemRepository _itemRepository;
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly ILogger<BffService> _logger;
    

    public BffService(ICatalogRepository<ItemBrand> brandRepository,
        ICatalogRepository<ItemType> typeRepository,
        ICatalogRepository<ItemCategory> categoryRepository,
        IItemRepository itemRepository,
        ICatalogItemRepository catalogItemRepository,
        ILogger<BffService> logger)
    {
        _brandRepository = brandRepository;
        _typeRepository = typeRepository;
        _categoryRepository = categoryRepository;
        _catalogItemRepository = catalogItemRepository;
        _itemRepository = itemRepository;
        _logger = logger;
    }
    
    public async Task IncreaseItemQuantity(List<OrderItem> items)
    {
        _logger.LogInformation($"*{GetType().Name}* increasing item quantity for {items.Count} items");
        foreach (var i in items)
        {
            var item = await GetItem(i.ItemId);
            _logger.LogInformation($"*{GetType().Name}* increasing item quantity " +
                                   $"from {item.Quantity} to {item.Quantity + i.Quantity} " +
                                   $"for item with id: {item.Id}");
            item.Quantity = item.Quantity + i.Quantity;
            await _itemRepository.UpdateInCatalog(item);
        }
        _logger.LogInformation($"*{GetType().Name}* item quantity was increased for {items.Count} items");
    }

    public async Task DecreaseItemQuantity(List<OrderItem> items)
    {
        _logger.LogInformation($"*{GetType().Name}* decreasing items quantity for {items.Count} items");
        await _itemRepository.DecreaseItemQuantity(items);
        _logger.LogInformation($"*{GetType().Name}* successfully decreased quantity for {items.Count} items");
    }

    public async Task<List<Item>> GetItemsByCatalogItemId(int catalogItemId)
    {
        var items = await _itemRepository.GetItemsByCatalogItemId(catalogItemId);
        _logger.LogInformation($"*{GetType().Name}* found: {items.Count} items with catalog item id: {catalogItemId}. items: {string.Join(", ", items)}");
        return items;
    }
    
    public async Task<List<CatalogItem>> GetCatalogItems(CatalogFilter filters)
    {
        var catalogItems = await _catalogItemRepository.GetCatalog(filters);
        _logger.LogDebug($"*{GetType().Name}* found {catalogItems.Count} catalog items: {string.Join(", ", catalogItems)}");
        return catalogItems;
    }

    public async Task<List<Item>> GetItems()
    {
        var items = await _itemRepository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {items.Count} items: {string.Join(", ", items)}");
        return items;
    }

    public async Task<List<ItemBrand>> GetBrands()
    {
        var brands = await _brandRepository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {brands.Count} brands: {string.Join(", ", brands)}");
        return brands;
    }

    public async Task<List<ItemType>> GetTypes()
    {
        var types = await _typeRepository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {types.Count} types: {string.Join(", ", types)}");
        return types;
    }

    public async Task<List<ItemCategory>> GetCategories()
    {
        var categories = await _categoryRepository.GetCatalog();
        _logger.LogDebug($"*{GetType().Name}* found {categories.Count} categories: {string.Join(", ", categories)}");
        return categories;
    }

    public async Task<CatalogItem> GetCatalogItem(int id)
    {
        var catalogItem = await _catalogItemRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found catalog item: {catalogItem.ToString()}");
        return catalogItem;
    }
    
    public async Task<Item> GetItem(int id)
    {
        var item = await _itemRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found item: {item.ToString()}");
        return item;
    }

    public async Task<ItemBrand> GetBrand(int id)
    {
        var brand = await _brandRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found brand: {brand.ToString()}");
        return brand;
    }

    public async Task<ItemType> GetType(int id)
    {
        var type = await _typeRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found type: {type.ToString()}");
        return type;
    }
    
    public async Task<ItemCategory> GetCategory(int id)
    {
        var category = await _categoryRepository.FindById(id);
        _logger.LogDebug($"*{GetType().Name}* found category: {category.ToString()}");
        return category;
    }
    
}