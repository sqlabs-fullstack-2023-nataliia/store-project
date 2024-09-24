using ExceptionHandler;
using Microsoft.EntityFrameworkCore;
using Order.Host.DbContextData;
using Order.Host.DbContextData.Entities;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories;

public class CatalogOrderRepository: ICatalogOrderRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogOrderRepository> _logger;

    public CatalogOrderRepository(ApplicationDbContext dbContext,
        ILogger<CatalogOrderRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task BeginTransaction()
    {
        _logger.LogInformation($"*{GetType().Name}* Starting transaction");
        await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransaction()
    {
        _logger.LogInformation($"*{GetType().Name}* Commiting transaction");
        await _dbContext.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransaction()
    {
        _logger.LogInformation($"*{GetType().Name}* Rolling back transaction");
        await _dbContext.Database.RollbackTransactionAsync();
    }
    public async Task<List<CatalogOrder>> GetOrdersByUserId(string userId)
    {
        IQueryable<CatalogOrder> query = _dbContext.CatalogOrders;
        query = query.Where(item => item.UserId == userId);
        _logger.LogInformation($"*{GetType().Name}* returning orders for user with id: {userId}");
        
        return await query.ToListAsync();
    }

    public async Task<List<CatalogOrder>> GetItems()
    {
        _logger.LogInformation($"*{GetType().Name}* returning all orders");
        return await _dbContext.CatalogOrders.ToListAsync();
    }

    public async Task<CatalogOrder> FindById(int id)
    {
        var catalogOrder = await _dbContext.CatalogOrders.FindAsync(id);
        if (catalogOrder == null)
        {
            _logger.LogError($"*{GetType().Name}* order with id: {id} does not exist");
            throw new NotFoundException($"Order with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning order with id: {id}");

        return catalogOrder;
    }

    public async Task<int?> AddItem(CatalogOrder item)
    {
        var newOrder = await _dbContext.CatalogOrders.AddAsync(item);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"*{GetType().Name}* adding new order for user with id: {item.UserId}");
        return newOrder.Entity.Id;
    }

    public async Task<CatalogOrder> UpdateItem(CatalogOrder item)
    {
        _logger.LogInformation($"*{GetType().Name}* updating order with id: {item.Id}");
        var newOrder = await FindById(item.Id);
        newOrder.Date = item.Date;
        newOrder.TotalPrice = item.TotalPrice;
        newOrder.TotalQuantity = item.TotalQuantity;
        newOrder.UserId = item.UserId;
        newOrder = _dbContext.CatalogOrders.Update(newOrder).Entity;
        await _dbContext.SaveChangesAsync();
        return newOrder;
    }
    
    public async Task<CatalogOrder> RemoveItem(int id)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        _logger.LogInformation($"*{GetType().Name}* Starting transaction");
        try
        {
            var order = await FindById(id);
            _logger.LogInformation($"*{GetType().Name}* removing order with id: {order.Id}");
            IQueryable<OrderItem> query = _dbContext.OrderItems;
            query = query.Where(item => item.OrderId == order.Id);
            foreach (var i in await query.ToListAsync())
            {
                _dbContext.OrderItems.Remove(i);
                _logger.LogInformation($"*{GetType().Name}* removing item with id: {i.Id}");
            }
            await _dbContext.SaveChangesAsync();
            _dbContext.CatalogOrders.Remove(order);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation($"*{GetType().Name}* Commiting transaction");

            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while removing order: {ex.Message}");
            await transaction.RollbackAsync();
            _logger.LogInformation($"*{GetType().Name}* Rolling back transaction");
            throw;
        }
    }
    
}