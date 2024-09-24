using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController: ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ICatalogService<ItemCategory, CategoryDto> _service;

    public CategoryController(ICatalogService<ItemCategory, CategoryDto> service,
        ILogger<CategoryController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    [HttpGet("categories")]
    public async Task<ActionResult> GetCategories()
    {
        _logger.LogInformation($"*{GetType().Name}* request to get all categories");
        var categories = await _service.GetCatalog();
        return Ok(categories);
    }

    [HttpGet("categories/{id}")]
    public async Task<ActionResult> GetCategoryById(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to get category by id: {id}");
        var category = await _service.FindById(id);
        return Ok(category);
    }

    [HttpPost("categories")]
    public async Task<ActionResult> AddCategory(CategoryDto category)
    {
        _logger.LogInformation($"*{GetType().Name}* request to add new category with name: {category.Category}");
        var categoryId = await _service.AddToCatalog(category);
        return Ok(categoryId);
    }

    [HttpPut("categories/{id}")]
    public async Task<ActionResult> UpdateCategory(int id, CategoryDto category)
    {
        _logger.LogInformation($"*{GetType().Name}* request to update category with id: {id}");
        var updatedCategory = await _service.UpdateInCatalog(id, category);
        return Ok(updatedCategory);
    }

    [HttpDelete("categories/{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        _logger.LogInformation($"*{GetType().Name}* request to remove category with id: {id}");
        var category = await _service.RemoveFromCatalog(id);
        return Ok(category);
    }
}