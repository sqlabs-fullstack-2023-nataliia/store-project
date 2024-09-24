using Basket.Host.Configurations;
using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Basket.Host.Services;

public class CacheService: ICacheService
{
    private readonly IRedisCacheConnectionService _redisCacheConnectionService;
    private readonly IOptions<RedisConfig> _config;
    private readonly ILogger<ICacheService> _logger;
        
    public CacheService(IRedisCacheConnectionService redisCacheConnectionService, 
        IOptions<RedisConfig> config, ILogger<ICacheService> logger)
    {
        _redisCacheConnectionService = redisCacheConnectionService;
        _config = config;
        _logger = logger;
    }
    public async Task AddOrUpdateAsync(string id, int key, int value)
    {
        var redis = GetRedisDatabase();
        if (redis.HashExists(id, key))
        {
            _logger.LogInformation($"*{GetType().Name}* increasing value with key: {key} by {value}, with id: {id}");
            await redis.HashIncrementAsync(id, key, value);
        }
        else
        {
            _logger.LogInformation($"*{GetType().Name}* setting new key: {key} with value: {value} by id: {id}");
            await redis.HashSetAsync(id, key, value);
        }
    }

    public async Task RemoveOrUpdateAsync(string id, int key, int value)
    {
        var redis = GetRedisDatabase();
        if (await redis.HashGetAsync(id, key) == value)
        {
            _logger.LogInformation($"*{GetType().Name}* deleting key: {key} with value {value}, by id: {id}");
            await redis.HashDeleteAsync(id, key);
        }
        else
        {
            _logger.LogInformation($"*{GetType().Name}* decreasing value with key: {key} by {value}, by id: {id}");
            await redis.HashDecrementAsync(id, key, value);
        }
    }

    public async Task<HashEntry[]> GetAsync(string id)
    {
        var redis = GetRedisDatabase();
        
        var values =  await redis.HashGetAllAsync(id);
        _logger.LogInformation($"*{GetType().Name}* found {values.Length} keys by id: {id}");
        return values;
    }

    public async Task RemoveAllAsync(string id)
    {
        var redis = GetRedisDatabase();
        _logger.LogInformation($"*{GetType().Name}* removing all keys by id: {id}");
        await redis.KeyDeleteAsync(id);
    }

    private IDatabase GetRedisDatabase() => _redisCacheConnectionService.Connection.GetDatabase();
}