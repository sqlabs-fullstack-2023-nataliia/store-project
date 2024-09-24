using Catalog.Host.DbContextData;
using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Repositories.Interfaces;
using ExceptionHandler;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class BrandsRepository: ICatalogRepository<ItemBrand>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<BrandsRepository> _logger;

    public BrandsRepository(ApplicationDbContext dbContext,
        ILogger<BrandsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<List<ItemBrand>> GetCatalog()
    {
        _logger.LogInformation($"*{GetType().Name}* returning all brands from catalog");
        
        return await _dbContext.ItemBrands.ToListAsync();
    }

    public async Task<ItemBrand> FindById(int id)
    {
        var brand = await _dbContext.ItemBrands.FindAsync(id);
        if (brand == null)
        {
            _logger.LogError($"*{GetType().Name}* brand with id: {id} does not exist");
            throw new NotFoundException($"Brand with ID: {id} does not exist");
        }
        _logger.LogInformation($"*{GetType().Name}* returning brand with id: {brand.Id}");
        
        return brand;
    }

    public async Task<int?> AddToCatalog(ItemBrand item)
    {
        var newBrand = await _dbContext.ItemBrands.AddAsync(item);
        await _dbContext.SaveChangesAsync();
        int id = newBrand.Entity.Id;
        _logger.LogInformation($"*{GetType().Name}* adding new brand with id: {id}");
        
        return id;
    }

    public async Task<ItemBrand> UpdateInCatalog(ItemBrand item)
    {
        var newBrand = await FindById(item.Id);
        newBrand.Brand = item.Brand;
        newBrand = _dbContext.ItemBrands.Update(newBrand).Entity;
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"*{GetType().Name}* updating brand with id: {newBrand.Id}");
        
        return newBrand;
    }

    public async Task<ItemBrand> RemoveFromCatalog(int id)
    {
        var brand = await FindById(id);
        _dbContext.ItemBrands.Remove(brand);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"*{GetType().Name}* removing brand with id: {brand.Id}");
        
        return brand;
    }
}