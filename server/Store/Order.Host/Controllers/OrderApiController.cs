using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Host.Models;
using Order.Host.Services.Interfaces;

namespace Order.Host.Controllers;

[Authorize]
[ApiController]
[Route("order-api-controller")]
public class OrderApiController : ControllerBase
{
    private readonly ILogger<OrderApiController> _logger;
    private readonly IOrderApiService _service;

    public OrderApiController(ILogger<OrderApiController> logger,
        IOrderApiService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost("orders/{userId}")]
    public async Task<IActionResult> CreateOrder(List<OrderItemModel> items, string userId)
    {
        _logger.LogInformation($"*{GetType().Name}* request to create new order " +
                               $"with {items.Count} items, for user: {userId}");
        var orderId = await _service.CreateOrder(items, userId);

        return Ok(orderId);
    }

    [HttpGet("orders/{userId}")]
    public async Task<IActionResult> GetOrders(string userId)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all orders for user: {userId}");
        var orders = await _service.GetOrdersByUserId(userId);

        return Ok(orders);
    }
}