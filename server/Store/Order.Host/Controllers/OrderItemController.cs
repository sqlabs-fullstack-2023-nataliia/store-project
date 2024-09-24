using Microsoft.AspNetCore.Mvc;
using Order.Host.DbContextData.Entities;
using Order.Host.Dto;
using Order.Host.Services.Interfaces;

namespace Order.Host.Controllers;

[ApiController]
[Route("order-item-controller")]
public class OrderItemController: ControllerBase
{
    private readonly ILogger<OrderItemController> _logger;
    private readonly IOrderService<OrderItem, OrderItemDto> _service;

    public OrderItemController(ILogger<OrderItemController> logger,
        IOrderService<OrderItem, OrderItemDto> service)
    {
        _logger = logger;
        _service = service;
    } 
    
    [HttpGet("order-items")]
    public async Task<IActionResult> GetOrderItems()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all order-items");
        var orders = await _service.GetItems();
        return Ok(orders);
    }
    
    [HttpGet("order-items/{id}")]
    public async Task<IActionResult> FindById(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get order-item by id: {id}");
        var order = await _service.FindById(id);
        return Ok(order);
    }
    
    [HttpPost("order-items")]
    public async Task<IActionResult> AddOrder(OrderItemDto orderItem)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new order-item for order with id: {orderItem.OrderId}");
        var orderItemId = await _service.AddItem(orderItem);
        return Ok(orderItemId);
    }
    
    [HttpPut("order-items/{id}")]
    public async Task<IActionResult> UpdateOrder(int id, OrderItemDto orderItem)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update order-item with id: {id}");
        var order = await _service.UpdateItem(id, orderItem);
        return Ok(order);
    }
    
    [HttpDelete("order-items/{id}")]
    public async Task<IActionResult> RemoveOrder(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to remove order-item with id: {id}");
        var order = await _service.RemoveItem(id);
        return Ok(order);
    }
}