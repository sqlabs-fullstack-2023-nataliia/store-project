using Order.Host.DbContextData.Entities;
using Order.Host.Dto;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services;

public class OrderItemService: IOrderService<OrderItem, OrderItemDto>
{
    private readonly IOrderRepository<OrderItem> _itemRepository;
    private readonly ILogger<OrderItemService> _logger;

    public OrderItemService(IOrderRepository<OrderItem> itemRepository,
        ILogger<OrderItemService> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger;
    }
    
    public async Task<List<OrderItem>> GetItems()
    {
        var orderItems = await _itemRepository.GetItems();
        _logger.LogInformation($"*{GetType().Name}* found order items {orderItems.Count}: {string.Join(", ", orderItems)}");

        return orderItems;
    }

    public async Task<OrderItem> FindById(int id)
    {
        var orderItem = await _itemRepository.FindById(id);
        _logger.LogInformation($"*{GetType().Name}* found order item: {orderItem.ToString()}");

        return orderItem;
    }

    public async Task<int?> AddItem(OrderItemDto item)
    {
        var id = await _itemRepository.AddItem(new OrderItem()
        {
            OrderId = item.OrderId,
            ItemId = item.ItemId,
            SubPrice = item.SubPrice,
            Quantity = item.Quantity
        });
        _logger.LogInformation($"*{GetType().Name}* adding new item with id: {id}");
        
        return id;
    }

    public async Task<OrderItem> UpdateItem(int id, OrderItemDto item)
    {
        var orderItem = await _itemRepository.UpdateItem(new OrderItem()
        {
            Id = id,
            OrderId = item.OrderId,
            ItemId = item.ItemId,
            SubPrice = item.SubPrice,
            Quantity = item.Quantity
        });
        _logger.LogInformation($"*{GetType().Name}* updating order item to: {orderItem.ToString()}");

        return orderItem;
    }

    public async Task<OrderItem> RemoveItem(int id)
    {
        var orderItem = await _itemRepository.RemoveItem(id);
        _logger.LogInformation($"*{GetType().Name}* removing order item: {orderItem.ToString()}");

        return orderItem;
    }
}