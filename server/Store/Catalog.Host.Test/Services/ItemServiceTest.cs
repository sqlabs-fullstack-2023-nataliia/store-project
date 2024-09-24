using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.Host.Test.Services;

public class ItemServiceTest
{
    private readonly Mock<IItemRepository> _itemRepo;
    private readonly ICatalogService<Item, ItemDto> _itemService;
    private readonly Mock<ILogger<ItemService>> _logger;
    private readonly int _expectedId = 1;
    private readonly int _givenId = 1;
    private readonly int _notExistId = 2;
    private readonly int _catalogItemsId = 1;
    private readonly int _quantity = 2;
    private readonly string _size = "s";
    private readonly Item _givenItem;
    private readonly Item _expectedItem;
    private readonly ItemDto _itemDto;


    public ItemServiceTest()
    {
        _itemRepo = new Mock<IItemRepository>();
        _logger = new Mock<ILogger<ItemService>>();
        _itemService = new ItemService(_itemRepo.Object, _logger.Object);
        _givenItem = new Item { Id = _expectedId, 
            CatalogItemId = _catalogItemsId, Quantity = _quantity, Size = _size};
        _expectedItem = new Item { Id = _expectedId, 
            CatalogItemId = _catalogItemsId, Quantity = _quantity, Size = _size};
        _itemDto = new ItemDto { CatalogItemId = _catalogItemsId, 
            Quantity = _quantity, Size = _size};
    }

    [Fact]
    public async Task AddItemAsync_Success()
    {
        _itemRepo.Setup(s => 
                s.AddToCatalog(It.IsAny<Item>()))
                .ReturnsAsync(1);

        var result = await _itemService.AddToCatalog(_itemDto);
        result.Should().Be(_expectedId);
    }

    [Fact]
    public async Task UpdateItemAsync_Success()
    {
        _itemRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<Item>()))
            .ReturnsAsync(_givenItem);

        var result = await _itemService.UpdateInCatalog(_givenId, _itemDto);
        result.Should().Be(_expectedItem);
    }

    [Fact]
    public async Task UpdateItemAsync_Failed()
    {
        _itemRepo.Setup(s => 
                    s.UpdateInCatalog(It.IsAny<Item>()))
                .ThrowsAsync(new Exception($"Item with ID: {_notExistId} does not exist"));
        var result = async () => await _itemService.UpdateInCatalog(_notExistId, _itemDto); 
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task FindItemByIdAsync_Success()
    {
        _itemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenItem);

        var result = await _itemService.FindById(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedItem);
    }
    
    [Fact]
    public async Task FindItemByIdAsync_Failed()
    {
        _itemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Item with ID: {_notExistId} does not exist"));

        var result = async () => await _itemService.FindById(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task RemoveItemAsync_Success()
    {
        _itemRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ReturnsAsync(_givenItem);

        var result = await _itemService.RemoveFromCatalog(_givenId);
        result.Should().Be(_expectedItem);
    }
    
    [Fact]
    public async Task RemoveItemAsync_Failed()
    {
        _itemRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Item with ID: {_notExistId} does not exist"));

        var result = async () => await _itemService.RemoveFromCatalog(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task GetItemsAsync_Success()
    {
        var expected = new List<Item>
        {
            _givenItem, _givenItem
        };
        _itemRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<Item>
        {
            _givenItem, _givenItem
        });

        var result = await _itemService.GetCatalog();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetItemsAsync_Failed()
    {
        var expected = new List<Item>();
        _itemRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<Item>());

        var result = await _itemService.GetCatalog();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }



}