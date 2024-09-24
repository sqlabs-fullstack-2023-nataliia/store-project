using Order.Host.DbContextData.Entities;

namespace Order.Host.Repositories.Interfaces;

public interface ICatalogOrderRepository: IOrderRepository<CatalogOrder>
{
    Task<List<CatalogOrder>> GetOrdersByUserId(string userId);
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
}