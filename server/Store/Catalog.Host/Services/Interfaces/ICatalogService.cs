namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService<T, TD>
{
    Task<List<T>> GetCatalog();
    Task<T> FindById(int id);
    Task<int?> AddToCatalog(TD item);
    Task<T> UpdateInCatalog(int id, TD item);
    Task<T> RemoveFromCatalog(int id);
}