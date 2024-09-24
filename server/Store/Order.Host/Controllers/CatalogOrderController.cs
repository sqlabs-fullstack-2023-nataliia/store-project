using Microsoft.AspNetCore.Mvc;
using Order.Host.DbContextData.Entities;
using Order.Host.Dto;
using Order.Host.Services.Interfaces;

namespace Order.Host.Controllers;

[ApiController]
[Route("catalog-order-controller")]
public class CatalogOrderController: ControllerBase
{
    private readonly ILogger<CatalogOrderController> _logger;
    private readonly IOrderService<CatalogOrder, CatalogOrderDto> _service;

    public CatalogOrderController(ILogger<CatalogOrderController> logger,
        IOrderService<CatalogOrder, CatalogOrderDto> service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all orders");
        var orders = await _service.GetItems();
        return Ok(orders);
    }
    
    [HttpGet("orders/{id}")]
    public async Task<IActionResult> FindById(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get order by id: {id}");
        var order = await _service.FindById(id);
        return Ok(order);
    }
    
    [HttpPost("orders")]
    public async Task<IActionResult> AddOrder(CatalogOrderDto order)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new order for user: {order.UserId}");
        var orderId = await _service.AddItem(order);
        return Ok(orderId);
    }
    
    [HttpPut("orders/{id}")]
    public async Task<IActionResult> UpdateOrder(int id, CatalogOrderDto catalogOrder)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update order with id: {id}");
        var order = await _service.UpdateItem(id, catalogOrder);
        return Ok(order);
    }
    
    [HttpDelete("orders/{id}")]
    public async Task<IActionResult> RemoveOrder(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to remove order with id: {id}");
        var order = await _service.RemoveItem(id);
        return Ok(order);
    }

}