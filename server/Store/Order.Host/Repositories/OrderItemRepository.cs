using ExceptionHandler;
using Microsoft.EntityFrameworkCore;
using Order.Host.DbContextData;
using Order.Host.DbContextData.Entities;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories;

public class OrderItemRepository: IOrderRepository<OrderItem>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<OrderItemRepository> _logger;

    public OrderItemRepository(ApplicationDbContext dbContext,
        ILogger<OrderItemRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<List<OrderItem>> GetItems()
    {
        _logger.LogInformation($"*{GetType().Name}* returning all order items");
        return await _dbContext.OrderItems.ToListAsync();
    }

    public async Task<OrderItem> FindById(int id)
    {
        var orderItem = await _dbContext.OrderItems.FindAsync(id);
        if (orderItem == null)
        {
            _logger.LogError($"*{GetType().Name}* order item with id: {id} does not exist");
            throw new NotFoundException($"Order ItemModel with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning order item with id: {id}");

        return orderItem;
    }

    public async Task<int?> AddItem(OrderItem item)
    {
        await FindOrder(item.OrderId);
        var newOrderItem = await _dbContext.OrderItems.AddAsync(item);
        _logger.LogInformation($"*{GetType().Name}* adding new item with order id: {item.Order}");
        return newOrderItem.Entity.Id;
    }

    private async Task<CatalogOrder> FindOrder(int id)
    {
        var order = await _dbContext.CatalogOrders.FindAsync(id);
        if (order == null)
        {
            _logger.LogError($"*{GetType().Name}* order with id: {id} does not exist");
            throw new NotFoundException($"Order with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning order with id: {order.Id}");

        return order;
    }

    public async Task<OrderItem> UpdateItem(OrderItem item)
    {
        var order = await FindOrder(item.OrderId);
        var newItem = await FindById(item.Id);
        _logger.LogInformation($"*{GetType().Name}* updating order item " +
                               $"with id: {newItem.Id} for order with id: {order.Id}");
        newItem.OrderId = order.Id;
        newItem.Quantity = item.Quantity;
        newItem.ItemId = item.ItemId;
        newItem.SubPrice = item.SubPrice;
        newItem = _dbContext.OrderItems.Update(newItem).Entity;
        await _dbContext.SaveChangesAsync();
        return newItem;

    }

    public async Task<OrderItem> RemoveItem(int id)
    {
        var item = await FindById(id);
        _dbContext.OrderItems.Remove(item);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"*{GetType().Name}* removing order item with id: {item.Id}");
        return item;
    }
}