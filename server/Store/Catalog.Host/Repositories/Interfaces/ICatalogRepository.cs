namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogRepository<T>
{
    Task<List<T>> GetCatalog();
    Task<T> FindById(int id);
    Task<int?> AddToCatalog(T item);
    Task<T> UpdateInCatalog(T item);
    Task<T> RemoveFromCatalog(int id);
}