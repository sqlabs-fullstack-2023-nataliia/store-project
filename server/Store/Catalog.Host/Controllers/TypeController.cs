using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("types")]
public class TypeController: ControllerBase
{
    private readonly ILogger<TypeController> _logger;
    private readonly ICatalogService<ItemType, TypeDto> _service;

    public TypeController(ICatalogService<ItemType, TypeDto> service,
        ILogger<TypeController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    [HttpGet("types")]
    public async Task<ActionResult> GetTypes()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all types");
        var types = await _service.GetCatalog();
        return Ok(types);
    }

    [HttpGet("types/{id}")]
    public async Task<ActionResult> GetTypeById(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get type by id: {id}");
        var type = await _service.FindById(id);
        return Ok(type);
    }

    [HttpPost("types")]
    public async Task<ActionResult> AddType(TypeDto type)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new type with name: {type.Type}");
        var typeId = await _service.AddToCatalog(type);
        return Ok(typeId);
    }

    [HttpPut("types/{id}")]
    public async Task<ActionResult> UpdateType(int id, TypeDto type)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update type with id: {id}");
        var updatedType = await _service.UpdateInCatalog(id, type);
        return Ok(updatedType);
    }

    [HttpDelete("types/{id}")]
    public async Task<ActionResult> DeleteType(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to remove type with id: {id}");
        var type = await _service.RemoveFromCatalog(id);
        return Ok(type);
    }
}