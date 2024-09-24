using System.Net;
using System.Security.Claims;
using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using ExceptionHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Controllers;

// [Authorize(Policy = "ApiScope")]
[Authorize]
[ApiController]
[Route("basket-bff-controller")]
public class BasketBffController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;
    private readonly IBasketService _basketService;

    public BasketBffController(
        ILogger<BasketBffController> logger,
        IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }

    [HttpPost("item")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(Item item, string userId = null)
    {
        if (userId == null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                            ?? User.FindFirst("sub")?.Value;
        }
        _logger.LogInformation($"*{GetType().Name}* request to add item to the basket for user: {userId}");
        await _basketService.AddItem(userId!, item);
        return Ok();
    }

    [HttpDelete("item/{itemId}/{quantity}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(int itemId, int quantity, string userId = null)
    {
        if (userId == null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        _logger.LogInformation($"*{GetType().Name}* request to remove items from the basket for user: {userId}");
        await _basketService.RemoveItem(userId!, new() { ItemId = itemId, Quantity = quantity });
        return Ok();
    }

    [HttpGet("items")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(string userId = null)
    {
        if (userId == null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        _logger.LogInformation($"*{GetType().Name}* request to get basket items for user: {userId}");
        var response = await _basketService.GetItems(userId!);
        return Ok(response);
    }

    [HttpDelete("items")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteAll(string userId = null)
    {
        if (userId == null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        _logger.LogInformation($"*{GetType().Name}* request to remove all items from the basket for user: {userId}");
        await _basketService.RemoveAll(userId!);
        return Ok();
    }

    [HttpPost("items/checkout")]
    public async Task<IActionResult> CheckoutBasket(string userId = null)
    {
        if (userId == null)
        {
            userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        _logger.LogInformation($"*{GetType().Name}* request to check-out basket for user: {userId}");
        var orderId = await _basketService.CheckoutBasket(userId);
        return Ok(orderId);
    }


}




