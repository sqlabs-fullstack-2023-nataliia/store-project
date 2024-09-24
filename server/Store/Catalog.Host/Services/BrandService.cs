using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class BrandService: ICatalogService<ItemBrand, BrandDto>
{
    private readonly ICatalogRepository<ItemBrand> _brandRepository;
    private readonly ILogger<BrandService> _logger;
    

    public BrandService(ICatalogRepository<ItemBrand> brandRepository,
        ILogger<BrandService> logger)
    {
        _brandRepository = brandRepository;
        _logger = logger;
    }
    public async Task<List<ItemBrand>> GetCatalog()
    {
        var brands =  await _brandRepository.GetCatalog();
        _logger.LogInformation($"*{GetType().Name}* found {brands.Count} brands: {string.Join(", ", brands)}");
        
        return brands;
    }

    public async Task<ItemBrand> FindById(int id)
    {
        var brand = await _brandRepository.FindById(id);
        _logger.LogInformation($"*{GetType().Name}* found brand: {brand.ToString()}");
        
        return brand;
    }

    public async Task<int?> AddToCatalog(BrandDto item)
    {
        var id = await _brandRepository.AddToCatalog(new ItemBrand()
        {
            Brand = item.Brand
        });
        _logger.LogInformation($"*{GetType().Name}* new brand with id: {id} was added");

        return id;
    }

    public async Task<ItemBrand> UpdateInCatalog(int id, BrandDto item)
    {
        var brand =  await _brandRepository.UpdateInCatalog(new ItemBrand()
        {
            Id = id,
            Brand = item.Brand
        });
        _logger.LogInformation($"*{GetType().Name}* brand was updated to {brand.ToString()}");

        return brand;
    }

    public async Task<ItemBrand> RemoveFromCatalog(int id)
    {
        var brand =  await _brandRepository.RemoveFromCatalog(id);
        _logger.LogInformation($"*{GetType().Name}* removed brand: {brand.ToString()}");

        return brand;
    }
}