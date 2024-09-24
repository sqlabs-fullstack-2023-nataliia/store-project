using Catalog.Host.DbContextData;
using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Models;
using Catalog.Host.Repositories.Interfaces;
using ExceptionHandler;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(ApplicationDbContext dbContext,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<CatalogItem>> GetCatalog(CatalogFilter filter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;
        // if (filter.Brand > 0) query = query.Where(w => w.ItemBrandId == filter.Brand);
        if (filter.Category > 0) query = query.Where(w => w.ItemCategoryId == filter.Category);
        if (filter.Type > 0) query = query.Where(w => w.ItemTypeId == filter.Type);
        if (filter.Brand != null) query = query.Where(w => filter.Brand.ToLower().Contains(w.ItemBrand.Brand.ToLower()));
        
        _logger.LogInformation($"*{GetType().Name}* returning all catalog items " +
                               $"filtered by category: {filter.Category}, type: {filter.Type}, brand: {filter.Brand}");

        return await query.ToListAsync();
    }

    public async Task<List<CatalogItem>> GetCatalog()
    {
        _logger.LogInformation($"*{GetType().Name}* returning all catalog items");
        return await _dbContext.CatalogItems.ToListAsync();
    }

    public async Task<CatalogItem> FindById(int id)
    {
        var item = await _dbContext.CatalogItems.FindAsync(id);
        if (item == null)
        {
            _logger.LogError($"*{GetType().Name}* item with id: {id} does not exist");
            throw new NotFoundException($"Item with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning catalog item by id: {item.Id}");

        return item;
    }

    public async Task<int?> AddToCatalog(CatalogItem item)
    {
        await FindBrand(item.ItemBrandId);
        await FindType(item.ItemTypeId);
        await FindCategory(item.ItemCategoryId);
        var newItem = await _dbContext.CatalogItems.AddAsync(item);
        await _dbContext.SaveChangesAsync();
        int id = newItem.Entity.Id;
        _logger.LogInformation($"*{GetType().Name}* adding new catalog item with id: {id}");

        return id;
    }
    private async Task<ItemBrand> FindBrand(int id)
    {
        var brand = await _dbContext.ItemBrands.FindAsync(id);
        if (brand == null)
        {
            _logger.LogError($"*{GetType().Name}* brand with id: {id} does not exist");
            throw new NotFoundException($"Brand with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning brand with id: {brand.Id}");

        return brand;
    }

    private async Task<ItemType> FindType(int id)
    {
        var type = await _dbContext.ItemTypes.FindAsync(id);
        if (type == null)
        {
            _logger.LogError($"*{GetType().Name}* type with id: {id} does not exist");
            throw new NotFoundException($"Type with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning type with id: {type.Id}");

        return type;
    }

    private async Task<ItemCategory> FindCategory(int id)
    {
        var category = await _dbContext.ItemCategories.FindAsync(id);
        if (category == null)
        {
            _logger.LogError($"*{GetType().Name}* category with id: {id} does not exist");
            throw new NotFoundException($"Category with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning category with id: {id}");

        return category;
    }

    public async Task<CatalogItem> UpdateInCatalog(CatalogItem item)
    {
        var brand = await FindBrand(item.ItemBrandId);
        var type = await FindType(item.ItemTypeId);
        var category = await FindCategory(item.ItemCategoryId);
        var newItem = await FindById(item.Id);

        newItem.ItemBrandId = brand.Id;
        newItem.ItemTypeId = type.Id;
        newItem.ItemCategoryId = category.Id;
        newItem.Name = item.Name;
        newItem.Price = item.Price;
        newItem.Description = item.Description;
        newItem.Image = item.Image;
        newItem = _dbContext.CatalogItems.Update(newItem).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"*{GetType().Name}* updating catalog item " +
                               $"with id: {newItem.Id}, type id: {type.Id}, " +
                               $"brand: {brand.Id}, category: {category.Id}");

        return newItem;
    }

    public async Task<CatalogItem> RemoveFromCatalog(int id)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        _logger.LogInformation($"*{GetType().Name}* Starting transaction");
        try
        {
            var catalogItem = await FindById(id);
            _logger.LogInformation($"*{GetType().Name}* removing catalog item with id: {catalogItem.Id}");
            IQueryable<Item> query = _dbContext.Items;
            query = query.Where(item => item.CatalogItemId == item.Id);
            foreach (var i in await query.ToListAsync())
            {
                _dbContext.Items.Remove(i);
                _logger.LogInformation($"*{GetType().Name}* removing item with id: {i.Id}");
            }
            await _dbContext.SaveChangesAsync();
            _dbContext.CatalogItems.Remove(catalogItem);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation($"*{GetType().Name}* Commiting transaction");

            return catalogItem;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while removing catalog item: {ex.Message}");
            await transaction.RollbackAsync();
            _logger.LogInformation($"*{GetType().Name}* Rolling back transaction");
            throw;
        }
    }

}