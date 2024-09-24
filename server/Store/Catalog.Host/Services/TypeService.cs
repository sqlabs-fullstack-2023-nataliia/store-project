using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class TypeService: ICatalogService<ItemType, TypeDto>
{
    private readonly ICatalogRepository<ItemType> _typeRepository;
    private readonly ILogger<TypeService> _logger;
    

    public TypeService(ICatalogRepository<ItemType> typeRepository,
        ILogger<TypeService> logger)
    {
        _typeRepository = typeRepository;
        _logger = logger;
    }
    public async Task<List<ItemType>> GetCatalog()
    {
        var types = await _typeRepository.GetCatalog();
        _logger.LogInformation($"*{GetType().Name}* found {types.Count} types: {string.Join(", ", types)}");

        return types;
    }

    public async Task<ItemType> FindById(int id)
    {
        var type = await _typeRepository.FindById(id);
        _logger.LogInformation($"*{GetType().Name}* found type: {type.ToString()}");

        return type;
    }

    public async Task<int?> AddToCatalog(TypeDto item)
    {
        int? id = await _typeRepository.AddToCatalog(new ItemType()
        {
            Type = item.Type
        });
        _logger.LogInformation($"*{GetType().Name}* new type with id: {id} was added");

        return id;
    }

    public async Task<ItemType> UpdateInCatalog(int id, TypeDto item)
    {
        var type = await _typeRepository.UpdateInCatalog(new ItemType()
        {
            Type = item.Type
        });
        _logger.LogInformation($"*{GetType().Name}* type was updated to: {type.ToString()}");

        return type;
    }

    public async Task<ItemType> RemoveFromCatalog(int id)
    {
        var type = await _typeRepository.RemoveFromCatalog(id);
        _logger.LogInformation($"*{GetType().Name}* removed type: {type.ToString()}");

        return type;
    }
}