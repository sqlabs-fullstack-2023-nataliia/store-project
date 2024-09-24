using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("items")]
public class ItemController: ControllerBase
{
    private readonly ILogger<ItemController> _logger;
    private readonly ICatalogService<Item, ItemDto> _service;

    public ItemController(ICatalogService<Item, ItemDto> service,
        ILogger<ItemController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    [HttpGet("items")]
    public async Task<ActionResult> GetItems()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all items");
        var items = await _service.GetCatalog();
        return Ok(items);
    }

    [HttpGet("items/{id}")]
    public async Task<ActionResult> GetItemById(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get item by id: {id}");
        var item = await _service.FindById(id);
        return Ok(item);
    }

    [HttpPost("items")]
    public async Task<ActionResult> AddItem(ItemDto item)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new item with catalog item id: {item.CatalogItemId}");
        var itemsId = await _service.AddToCatalog(item);
        return Ok(itemsId);
    }

    [HttpPut("items/{id}")]
    public async Task<ActionResult> UpdateItem(int id, ItemDto item)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update item with id: {id}");
        var updatedItems = await _service.UpdateInCatalog(id, item);
        return Ok(updatedItems);
    }

    [HttpDelete("items/{id}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to remove item with id: {id}");
        var items = await _service.RemoveFromCatalog(id);
        return Ok(items);
    }
}