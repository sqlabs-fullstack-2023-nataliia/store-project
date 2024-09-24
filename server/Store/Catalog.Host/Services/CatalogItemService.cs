using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService: ICatalogService<CatalogItem, CatalogItemDto>
{
    private readonly ICatalogItemRepository _itemRepository;
    private readonly ILogger<CatalogItemService> _logger;
    

    public CatalogItemService(ICatalogItemRepository itemRepository,
        ILogger<CatalogItemService> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger;
    }
    public async Task<List<CatalogItem>> GetCatalog()
    {
        var catalogItems =  await _itemRepository.GetCatalog();
        _logger.LogInformation($"*{GetType().Name}* found {catalogItems.Count} catalog items: {string.Join(", ", catalogItems)}");

        return catalogItems;
    }

    public async Task<CatalogItem> FindById(int id)
    {
        var catalogItem = await _itemRepository.FindById(id);
        _logger.LogInformation($"*{GetType().Name}* found catalog item: {catalogItem.ToString()}");

        return catalogItem;
    }

    public async Task<int?> AddToCatalog(CatalogItemDto item)
    {
        int? id = await _itemRepository.AddToCatalog(new CatalogItem()
        {
            Name = item.Name,
            ItemBrandId = item.ItemBrandId,
            ItemTypeId = item.ItemTypeId,
            Price = item.Price,
            Image = item.Image,
            ItemCategoryId = item.ItemCategoryId,
            Description = item.Description
        });
        _logger.LogInformation($"*{GetType().Name}* new catalog item with id: {id} was created");

        return id;
    }

    public async Task<CatalogItem> UpdateInCatalog(int id, CatalogItemDto item)
    {
        var catalogItem = await _itemRepository.UpdateInCatalog(new CatalogItem()
        {
            Id = id,
            Name = item.Name,
            ItemBrandId = item.ItemBrandId,
            ItemTypeId = item.ItemTypeId,
            Price = item.Price,
            Image = item.Image,
            ItemCategoryId = item.ItemCategoryId,
            Description = item.Description
        });
        _logger.LogInformation($"*{GetType().Name}* catalog item was update to: {catalogItem.ToString()}");

        return catalogItem;
    }

    public async Task<CatalogItem> RemoveFromCatalog(int id)
    {
        var catalogItem = await _itemRepository.RemoveFromCatalog(id);
        _logger.LogInformation($"*{GetType().Name}* removed catalog item: {catalogItem.ToString()}");

        return catalogItem;
    }
}