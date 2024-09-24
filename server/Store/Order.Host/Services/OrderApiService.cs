using Order.Host.DbContextData.Entities;
using Order.Host.Models;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services;

public class OrderApiService: IOrderApiService
{
    private readonly ICatalogOrderRepository _catalogOrderRepository;
    private readonly IOrderRepository<OrderItem> _orderItemRepository;
    private readonly ILogger<OrderApiService> _logger;

    public OrderApiService(ICatalogOrderRepository catalogOrderRepository,
        IOrderRepository<OrderItem> orderItemRepository,
        ILogger<OrderApiService> logger)
    {
        _catalogOrderRepository = catalogOrderRepository;
        _orderItemRepository = orderItemRepository;
        _logger = logger;
    }
    public async Task<int?> CreateOrder(List<OrderItemModel> items, string userId)
    {
        using var transaction = _catalogOrderRepository.BeginTransaction();

        try
        {
            CatalogOrder order = new CatalogOrder() { UserId = userId, Date = DateTime.Now.ToShortDateString()};
            int? orderId = await _catalogOrderRepository.AddItem(order);
            _logger.LogInformation($"*{GetType().Name}* creating new order with id: {orderId}");

            decimal totalPrice = items.Sum(item => item.Quantity * item.Price);
            int totalQuantity = items.Sum(item => item.Quantity);

            foreach (var item in items)
            {
                var orderItem = await _orderItemRepository.AddItem(new()
                {
                    OrderId = orderId.Value,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    SubPrice = item.Quantity * item.Price
                });
                _logger.LogInformation($"*{GetType().Name}* creating new order item: {orderItem.ToString()}");
            }

            order.Id = orderId.Value;
            order.TotalPrice = totalPrice;
            order.TotalQuantity = totalQuantity;
            var newOrder = await _catalogOrderRepository.UpdateItem(order);
            _logger.LogInformation($"*{GetType().Name}* new order was added: {order.ToString()}");

            await _catalogOrderRepository.CommitTransaction(); 
            return orderId.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Order was not created because of occurred error: {ex.Message}");
            await _catalogOrderRepository.RollbackTransaction(); 
            return null;
        }
    }

    public async Task<List<CatalogOrder>> GetOrdersByUserId(string userId)
    {
        var orders = await _catalogOrderRepository.GetOrdersByUserId(userId);
        _logger.LogInformation($"*{GetType().Name}* found orders {orders.Count}: {string.Join(", ", orders)}");
        return orders;
    }
}