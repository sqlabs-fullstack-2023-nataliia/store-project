using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class ItemService: ICatalogService<Item, ItemDto>
{
    private readonly IItemRepository _itemRepository;
    private readonly ILogger<ItemService> _logger;
    

    public ItemService(IItemRepository itemRepository,
        ILogger<ItemService> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger;
    }
    public async Task<List<Item>> GetCatalog()
    {
        var items = await _itemRepository.GetCatalog();
        _logger.LogInformation($"*{GetType().Name}* found {items.Count} items: {string.Join(", ", items)}");

        return items;
    }

    public async Task<Item> FindById(int id)
    {
        var item = await _itemRepository.FindById(id);
        _logger.LogInformation($"*{GetType().Name}* found item: {item.ToString()}");

        return item;
    }

    public async Task<int?> AddToCatalog(ItemDto item)
    {
        int? id = await _itemRepository.AddToCatalog(new Item()
        {
            CatalogItemId = item.CatalogItemId,
            Quantity = item.Quantity,
            Size = item.Size
        });
        _logger.LogInformation($"*{GetType().Name}* new item with id; {id} was added");

        return id;
    }

    public async Task<Item> UpdateInCatalog(int id, ItemDto item)
    {
        var newItem = await _itemRepository.UpdateInCatalog(new Item()
        {
            Id = id,
            CatalogItemId = item.CatalogItemId,
            Quantity = item.Quantity,
            Size = item.Size
        });
        _logger.LogInformation($"*{GetType().Name}* item was updated to: {newItem.ToString()}");

        return newItem;
    }

    public async Task<Item> RemoveFromCatalog(int id)
    {
        var item = await _itemRepository.RemoveFromCatalog(id);
        _logger.LogInformation($"*{GetType().Name}* removed item: {item.ToString()}");

        return item;
    }
}