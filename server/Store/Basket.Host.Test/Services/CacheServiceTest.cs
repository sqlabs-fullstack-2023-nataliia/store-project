using Basket.Host.Configurations;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace Basket.Host.Test.Services;

public class CacheServiceTest
{
    private readonly ICacheService _cacheService;
    private readonly Mock<IOptions<RedisConfig>> _config;
    private readonly Mock<ILogger<CacheService>> _logger;
    private readonly Mock<IRedisCacheConnectionService> _redisCacheConnectionService;
    private readonly Mock<IConnectionMultiplexer> _connectionMultiplexer;
    private readonly Mock<IDatabase> _redisDataBase;

    private readonly string _id = "test";
    private readonly int _key = 1;
    private readonly int _value = 5;
    
    public CacheServiceTest()
    {
        _config = new Mock<IOptions<RedisConfig>>();
        _logger = new Mock<ILogger<CacheService>>();
        _config.Setup(x => x.Value).Returns(new RedisConfig() { CacheTimeout = TimeSpan.Zero });

        _connectionMultiplexer = new Mock<IConnectionMultiplexer>();
        _redisDataBase = new Mock<IDatabase>();

        _connectionMultiplexer
            .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_redisDataBase.Object);

        _redisCacheConnectionService = new Mock<IRedisCacheConnectionService>();
        _redisCacheConnectionService.Setup(x => x.Connection)
            .Returns(_connectionMultiplexer.Object);

        _cacheService = new CacheService(
            _redisCacheConnectionService.Object,
            _config.Object,
            _logger.Object);

    }
    
    [Fact]
    public async Task AddOrUpdateAsync_KeyExists_IncrementValue()
    {
         _redisDataBase.Setup(x => x.HashExists(_id,
             _key, It.IsAny<CommandFlags>())).Returns(true);
         _redisDataBase.Setup(x => x.HashIncrementAsync(_id, _key, _value, It.IsAny<CommandFlags>())).ReturnsAsync(10);
        await _cacheService.AddOrUpdateAsync(_id, _key, _value);

        _redisDataBase.Verify(x => x.HashIncrementAsync(_id, _key, _value, It.IsAny<CommandFlags>()), Times.Once);
    }
    
    [Fact]
    public async Task AddOrUpdateAsync_KeyDoesNotExist_SetNewValue()
    {
        _redisDataBase.Setup(x => x.HashExists(It.Is<RedisKey>(id => id == _id), _key, It.IsAny<CommandFlags>())).Returns(false);
        _redisDataBase.Setup(x => x.HashExists(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<CommandFlags>()))
            .Returns((RedisKey id, RedisValue key, CommandFlags flags) => false);
        await _cacheService.AddOrUpdateAsync(_id, _key, _value);

        _redisDataBase.Verify(x => x.HashSetAsync(_id, _key, _value, It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task RemoveOrUpdateAsync_ValueMatches_DeleteKey()
    {
        _redisDataBase.Setup(x => x.HashGetAsync(_id, _key, It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue)_value);
        await _cacheService.RemoveOrUpdateAsync(_id, _key, _value);

        _redisDataBase.Verify(x => x.HashDeleteAsync(_id, _key, It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task RemoveOrUpdateAsync_ValueDoesNotMatch_DecreaseValue()
    {
        _redisDataBase.Setup(x => x.HashGetAsync(_id, _key, It.IsAny<CommandFlags>()))
            .ReturnsAsync((RedisValue)(_value + 1)); 
        await _cacheService.RemoveOrUpdateAsync(_id, _key, _value);
        
        _redisDataBase.Verify(x => x.HashDecrementAsync(_id, _key, _value, It.IsAny<CommandFlags>()), Times.Once);
    }
    
    [Fact]
    public async Task GetAsync_ReturnsHashEntries_Success()
    {
        var expectedEntries = new HashEntry[]
        {
            new HashEntry("key1", "value1"),
            new HashEntry("key2", "value2"),
        };

        _redisDataBase.Setup(x => x.HashGetAllAsync(_id, It.IsAny<CommandFlags>()))
            .ReturnsAsync(expectedEntries);
        var result = await _cacheService.GetAsync(_id);
        Xunit.Assert.Equal(expectedEntries, result); 
    }
    
    [Fact]
    public async Task GetAsync_ReturnsHashEntries_Failed()
    {
        var expectedEntries = new HashEntry[] {};

        _redisDataBase.Setup(x => x.HashGetAllAsync(_id, It.IsAny<CommandFlags>()))
            .ReturnsAsync(new HashEntry[] {});
        var result = await _cacheService.GetAsync(_id);
        Xunit.Assert.Equal(expectedEntries, result); 
    }
    
    [Fact]
    public async Task RemoveAllAsync_RemovesAllKeys()
    {
        await _cacheService.RemoveAllAsync(_id);
        _redisDataBase.Verify(x => x.KeyDeleteAsync(_id, It.IsAny<CommandFlags>()), Times.Once);
    }




}


    
 

