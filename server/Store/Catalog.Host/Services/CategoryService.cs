using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CategoryService: ICatalogService<ItemCategory, CategoryDto>
{
    private readonly ICatalogRepository<ItemCategory> _categoryRepository;
    private readonly ILogger<CategoryService> _logger;
    

    public CategoryService(ICatalogRepository<ItemCategory> categoryRepository,
        ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }
    public async Task<List<ItemCategory>> GetCatalog()
    {
        var categories = await _categoryRepository.GetCatalog();
        _logger.LogInformation($"*{GetType().Name}* found {categories.Count} categories: {string.Join(", ", categories)}");

        return categories;
    }

    public async Task<ItemCategory> FindById(int id)
    {
        var category = await _categoryRepository.FindById(id);
        _logger.LogInformation($"*{GetType().Name}* found category: {category.ToString()}");

        return category;
    }

    public async Task<int?> AddToCatalog(CategoryDto item)
    {
        int? id = await _categoryRepository.AddToCatalog(new ItemCategory()
        {
            Category = item.Category
        });
        _logger.LogInformation($"*{GetType().Name}* new category with id: {id} was added");

        return id;
    }

    public async Task<ItemCategory> UpdateInCatalog(int id, CategoryDto item)
    {
        var category = await _categoryRepository.UpdateInCatalog(new ItemCategory()
        {
            Id = id,
            Category = item.Category
        });
        _logger.LogInformation($"*{GetType().Name}* category was updated to: {category.ToString()}");

        return category;
    }

    public async Task<ItemCategory> RemoveFromCatalog(int id)
    {
        var category = await _categoryRepository.RemoveFromCatalog(id);
        _logger.LogInformation($"*{GetType().Name}* removed category: {category.ToString()}");

        return category;
    }
}