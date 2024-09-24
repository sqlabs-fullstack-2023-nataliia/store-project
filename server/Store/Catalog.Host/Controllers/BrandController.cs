using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("brands")]
public class BrandController: ControllerBase
{
    private readonly ILogger<BrandController> _logger;
    private readonly ICatalogService<ItemBrand, BrandDto> _service;

    public BrandController(ICatalogService<ItemBrand, BrandDto> service,
        ILogger<BrandController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("brands")]
    public async Task<ActionResult> GetBrands()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all brands");
        var brands = await _service.GetCatalog();
        return Ok(brands);
    }

    [HttpGet("brands/{id}")]
    public async Task<ActionResult> GetBrandById(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get brand by id: {id}");
        var brand = await _service.FindById(id);
        return Ok(brand);
    }

    [HttpPost("brands")]
    public async Task<ActionResult> AddBrand(BrandDto brand)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new brand with name: {brand.Brand}");
        var brandId = await _service.AddToCatalog(brand);
        return Ok(brandId);
    }

    [HttpPut("brands/{id}")]
    public async Task<ActionResult> UpdateBrand(int id, BrandDto brand)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update brand with id: {id}");
        var updatedBrand = await _service.UpdateInCatalog(id, brand);
        return Ok(updatedBrand);
    }

    [HttpDelete("brands/{id}")]
    public async Task<ActionResult> DeleteBrand(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to remove brand with id: {id}");
        var brand = await _service.RemoveFromCatalog(id);
        return Ok(brand);
    }

}