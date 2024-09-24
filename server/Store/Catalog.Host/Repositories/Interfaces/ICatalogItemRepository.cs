using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Models;

namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository: ICatalogRepository<CatalogItem>
{
    Task<List<CatalogItem>> GetCatalog(CatalogFilter filter);
}