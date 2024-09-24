using Basket.Host.Models;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task AddItem(string userId, Item item);
    Task RemoveItem(string userId, Item item);
    Task<List<OrderItem>> GetItems(string userId);
    Task RemoveAll(string userId);
    Task<int> CheckoutBasket(string userId);
}