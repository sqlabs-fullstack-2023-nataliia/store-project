using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("catalog-items")]
public class CatalogItemController: ControllerBase
{
    private readonly ILogger<CatalogItemController> _logger;
    private readonly ICatalogService<CatalogItem, CatalogItemDto> _service;

    public CatalogItemController(
        ICatalogService<CatalogItem, CatalogItemDto> service,
        ILogger<CatalogItemController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    [HttpGet("catalog-items")]
    public async Task<ActionResult> GetCatalogItems()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all catalog items");
        var items = await _service.GetCatalog();
        return Ok(items);
    }

    [HttpGet("catalog-items/{id}")]
    public async Task<ActionResult> GetCatalogItemById(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get catalog item by id: {id}");
        var item = await _service.FindById(id);
        return Ok(item);
    }

    [HttpPost("catalog-items")]
    public async Task<ActionResult> AddCatalogItem(CatalogItemDto item)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new catalog item with name: {item.Name}");
        var itemsId = await _service.AddToCatalog(item);
        return Ok(itemsId);
    }

    [HttpPut("catalog-items/{id}")]
    public async Task<ActionResult> UpdateCatalogItem(int id, CatalogItemDto item)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update catalog item with id: {id}");
        var updatedItems = await _service.UpdateInCatalog(id, item);
        return Ok(updatedItems);
    }

    [HttpDelete("catalog-items/{id}")]
    public async Task<ActionResult> DeleteCatalogItem(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to remove catalog item with id: {id}");
        var items = await _service.RemoveFromCatalog(id);
        return Ok(items);
    }
}