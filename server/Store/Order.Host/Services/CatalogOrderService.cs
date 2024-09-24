using Order.Host.DbContextData.Entities;
using Order.Host.Dto;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services;

public class CatalogOrderService: IOrderService<CatalogOrder, CatalogOrderDto>
{
    private readonly ICatalogOrderRepository _orderRepository;
    private readonly ILogger<CatalogOrderService> _logger;

    public CatalogOrderService(ICatalogOrderRepository orderRepository,
        ILogger<CatalogOrderService> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }
    
    public async Task<List<CatalogOrder>> GetItems()
    {
        var orders =  await _orderRepository.GetItems();
        _logger.LogInformation($"*{GetType().Name}* found {orders.Count}: {string.Join(", ", orders)}");

        return orders;
    }

    public async Task<CatalogOrder> FindById(int id)
    {
        var order = await _orderRepository.FindById(id);
        _logger.LogInformation($"*{GetType().Name}* found order: {order.ToString()}");

        return order;
    }

    public async Task<int?> AddItem(CatalogOrderDto item)
    {
        var orderId = await _orderRepository.AddItem(new CatalogOrder()
        {
            Date = item.Date,
            TotalQuantity = item.TotalQuantity,
            TotalPrice = item.TotalPrice,
            UserId = item.UserId
        });
        _logger.LogInformation($"*{GetType().Name}* added new order with id: {orderId}");

        return orderId;
    }

    public async Task<CatalogOrder> UpdateItem(int id, CatalogOrderDto item)
    {
        var order = await _orderRepository.UpdateItem(new CatalogOrder()
        {
            Id = id,
            Date = item.Date,
            TotalQuantity = item.TotalQuantity,
            TotalPrice = item.TotalPrice,
            UserId = item.UserId
        });
        _logger.LogInformation($"*{GetType().Name}* updated order: {order.ToString()}");

        return order;
    }

    public async Task<CatalogOrder> RemoveItem(int id)
    {
        var order = await _orderRepository.RemoveItem(id);
        _logger.LogInformation($"*{GetType().Name}* removed order: {order.ToString()}");

        return order;
    }
}