using Catalog.Host.DbContextData;
using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Repositories.Interfaces;
using ExceptionHandler;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CategoryRepository: ICatalogRepository<ItemCategory>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(ApplicationDbContext dbContext,
        ILogger<CategoryRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<List<ItemCategory>> GetCatalog()
    {
        _logger.LogInformation($"*{GetType().Name}* returning all categories");
        return await _dbContext.ItemCategories.ToListAsync();
    }

    public async Task<ItemCategory> FindById(int id)
    {
        var category = await _dbContext.ItemCategories.FindAsync(id);
        if (category == null)
        {
            _logger.LogError($"*{GetType().Name}* category with id: {id} does not exist");
            throw new NotFoundException($"Category with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning category with id: {category.Id}");

        return category;
    }

    public async Task<int?> AddToCatalog(ItemCategory item)
    {
        var newCategory = await _dbContext.ItemCategories.AddAsync(item);
        await _dbContext.SaveChangesAsync();
        int id = newCategory.Entity.Id;
        _logger.LogInformation($"*{GetType().Name}* adding new category with id: {id}");
        
        return id;
    }

    public async Task<ItemCategory> UpdateInCatalog(ItemCategory item)
    {
        var newCategory = await FindById(item.Id);
        newCategory.Category = item.Category;
        newCategory = _dbContext.ItemCategories.Update(newCategory).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"*{GetType().Name}* updating category with id: {newCategory.Id}");
        
        return newCategory;
    }

    public async Task<ItemCategory> RemoveFromCatalog(int id)
    {
        var category = await FindById(id);
        _dbContext.ItemCategories.Remove(category);
        await _dbContext.SaveChangesAsync();
        _logger. LogInformation($"*{GetType().Name}* removing category with id: {category.Id}");
        
        return category;
    }
}