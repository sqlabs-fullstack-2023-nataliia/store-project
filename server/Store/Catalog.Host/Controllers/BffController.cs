using Catalog.Host.Models;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("catalog-bff-controller")]
public class BffController : ControllerBase
{
    private readonly ILogger<BffController> _logger;
    private readonly IBffService _service;

    public BffController(
        ILogger<BffController> logger,
        IBffService service
        )
    {
        _logger = logger;
        _service = service;
    }
    
    [Authorize]
    [HttpPut("items/increase")]
    public async Task<IActionResult> IncreaseItemQuantity(List<OrderItem> items)
    {
        _logger.LogInformation($"*{GetType().Name}* request to increase items quantity stock");
        await _service.IncreaseItemQuantity(items);
        return Ok();
    }
    
    [Authorize]
    [HttpPut("items/decrease")]
    public async Task<IActionResult> DecreaseItemQuantity(List<OrderItem> items)
    {
        _logger.LogInformation($"*{GetType().Name}* request to decrease items quantity stock");
        await _service.DecreaseItemQuantity(items);
        return Ok();
    }

    [HttpGet("items/stock")]
    public async Task<IActionResult> GetItemsStock(int catalogItemId)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get catalog items by catalog-item id");
        var items = await _service.GetItemsByCatalogItemId(catalogItemId);
        return Ok(items);
    }

    [HttpGet("catalog-items")]
    public async Task<IActionResult> GetCatalogItems(int category, int type, String? brand)
    {
        _logger.LogInformation(
            $"*{GetType().Name}* request to get catalog items");
        var catalogItems = await _service.GetCatalogItems(new CatalogFilter() { Category = category, Type = type, Brand = brand });
        return Ok(catalogItems);
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetItems()
    {
        _logger.LogInformation(
            $"*{GetType().Name}* request to get items");
        var catalogItems = await _service.GetItems();
        return Ok(catalogItems);
    }

    [HttpGet("brands")]
    public async Task<ActionResult> GetBrands()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all brands");
        var brands = await _service.GetBrands();
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult> GetTypes()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all types");
        var types = await _service.GetTypes();
        return Ok(types);
    }

    [HttpGet("categories")]
    public async Task<ActionResult> GetCategories()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all categories");
        var categories = await _service.GetCategories();
        return Ok(categories);
    }

    [HttpGet("catalog-items/{id:int}")]
    public async Task<ActionResult> GetCatalogItem(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get catalog item by id: {id}");
        var item = await _service.GetCatalogItem(id);
        return Ok(item);
    }

    [HttpGet("items/{id:int}")]
    public async Task<ActionResult> GetItem(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.GetItem(id);
        return Ok(item);
    }

    [HttpGet("brands/{id}")]
    public async Task<ActionResult> GetBrand(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.GetBrand(id);
        return Ok(item);
    }

    [HttpGet("types/{id}")]
    public async Task<ActionResult> GetType(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.GetType(id);
        return Ok(item);
    }

    [HttpGet("categories/{id}")]
    public async Task<ActionResult> GetCategory(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get category by id: {id}");
        var category = await _service.GetCategory(id);
        return Ok(category);
    }

}