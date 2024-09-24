using StackExchange.Redis;

namespace Basket.Host.Services.Interfaces;

public interface ICacheService
{
    Task AddOrUpdateAsync(string id, int key, int value);
    Task RemoveOrUpdateAsync(string id, int key, int value);
    Task<HashEntry[]>GetAsync(string id);
    Task RemoveAllAsync(string id);

}