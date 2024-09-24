using Order.Host.DbContextData.Entities;
using Order.Host.Models;

namespace Order.Host.Services.Interfaces;

public interface IOrderApiService
{
    Task<int?> CreateOrder(List<OrderItemModel> i, string userId);
    Task<List<CatalogOrder>> GetOrdersByUserId(string userId);
}