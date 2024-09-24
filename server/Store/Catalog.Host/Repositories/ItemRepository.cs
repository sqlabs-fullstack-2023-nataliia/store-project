using Catalog.Host.DbContextData;
using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Models;
using Catalog.Host.Repositories.Interfaces;
using ExceptionHandler;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class ItemRepository: IItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ItemRepository> _logger;

    public ItemRepository(ApplicationDbContext dbContext,
        ILogger<ItemRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task DecreaseItemQuantity(List<OrderItem> items)
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            _logger.LogInformation($"*{GetType().Name}* Starting transaction");
            try
            {
                foreach (var i in items)
                {
                    var item = await FindById(i.ItemId);
                    if (item.Quantity < i.Quantity)
                    {
                        _logger.LogError($"*{GetType().Name}* item with id: {i.ItemId} is not available in stock with quantity: {i.Quantity}");
                        throw new IllegalArgumentException($"Item with id: {i.ItemId} is not available in stock with quantity: {i.Quantity}");
                    }
                    _logger.LogInformation($"*{GetType().Name}* decreasing quantity" +
                                           $" from {item.Quantity} to {item.Quantity - i.Quantity} " +
                                           $"for item with id: {item.Id}");
                    item.Quantity -= i.Quantity; 
                    _dbContext.Items.Update(item);
                }
                
                await _dbContext.SaveChangesAsync(); 
                await transaction.CommitAsync(); 
                _logger.LogInformation($"*{GetType().Name}* Commiting transaction");
                _logger.LogInformation($"*{GetType().Name}* {items.Count} items was updated");
            }
            catch (NotFoundException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation($"*{GetType().Name}* Rolling back transaction");
                _logger.LogError($"*{GetType().Name}* {ex.Message}");
                throw; 
            }
        }
        
    }
    
    public async Task<List<Item>> GetItemsByCatalogItemId(int catalogItemId)
    {
        IQueryable<Item> query = _dbContext.Items;
        query = query.Where(w => w.CatalogItemId == catalogItemId);
        _logger.LogInformation($"*{GetType().Name}* returning items by catalog item id: {catalogItemId}");
        
        return await query.ToListAsync();
    }

    public async Task<List<Item>> GetCatalog()
    {
        _logger.LogInformation($"*{GetType().Name}* returning all items");
        
        return await _dbContext.Items.ToListAsync();
    }

    public async Task<Item> FindById(int id)
    {
        var item = await _dbContext.Items
            .Include(i => i.CatalogItem)
            .FirstOrDefaultAsync(i => i.Id == id);
        
        if (item == null)
        {
            _logger.LogError($"*{GetType().Name}* item with id: {id} does not exist");
            throw new NotFoundException($"Item with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning item with id: {item.Id}");

        return item;
    }

    public async Task<int?> AddToCatalog(Item item)
    {
        await FindCatalogItem(item.CatalogItemId);
        var newItem = await _dbContext.Items.AddAsync(item);
        await _dbContext.SaveChangesAsync();
        int id = newItem.Entity.Id;
        _logger.LogInformation($"*{GetType().Name}* adding new item with id: {id}");
        
        return id;
    }
    
    private async Task<CatalogItem> FindCatalogItem(int id)
    {
        var catalogItem = await _dbContext.CatalogItems.FindAsync(id);
        if (catalogItem == null)
        {
            _logger.LogError($"*{GetType().Name}* catalog item with id: {id} does not exist");
            throw new NotFoundException($"Catalog Item with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning catalog item with id: {catalogItem.Id}");

        return catalogItem;
    }

    public async Task<Item> UpdateInCatalog(Item item)
    {
        var categoryItem = await FindCatalogItem(item.CatalogItemId);
        var newItem = await FindById(item.Id);
        
        newItem.CatalogItemId = categoryItem.Id;
        newItem.Quantity = item.Quantity;
        newItem.Size = item.Size;
        newItem = _dbContext.Items.Update(newItem).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"*{GetType().Name}* updating item with id: {newItem.Id}");
        
        return newItem;
    }

    public async Task<Item> RemoveFromCatalog(int id)
    {
        var item = await FindById(id);
        _dbContext.Items.Remove(item);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"*{GetType().Name}* removing item with id: {item.Id}");
        
        return item;
    }
}