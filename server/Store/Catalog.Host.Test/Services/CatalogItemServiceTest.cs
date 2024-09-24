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

public class CatalogItemServiceTest
{
    private readonly Mock<ICatalogItemRepository> _catalogItemRepo;
    private readonly ICatalogService<CatalogItem, CatalogItemDto> _catalogItemService;
    private readonly Mock<ILogger<CatalogItemService>> _logger;
    private readonly int _expectedId = 1;
    private readonly int _givenId = 1;
    private readonly int _notExistId = 2;
    
    private readonly CatalogItem _givenCatalogItem;
    private readonly CatalogItem _expectedCatalogItem;
    private readonly CatalogItemDto _catalogItemDto;
    private readonly string _name = "catalog-item-name";
    private readonly int _brandId = 1;
    private readonly int _typeId = 1;
    private readonly decimal _price = 100;
    private readonly string _image = "image-1.png";
    private readonly int _categoryId = 1;
    private readonly string _descroption = "catalog item description";
    


    public CatalogItemServiceTest()
    {
        _catalogItemRepo = new Mock<ICatalogItemRepository>();
        _logger = new Mock<ILogger<CatalogItemService>>();
        _catalogItemService = new CatalogItemService(_catalogItemRepo.Object, _logger.Object);
        _givenCatalogItem = new CatalogItem { Id = _expectedId, Name = _name, 
            ItemBrandId = _brandId, ItemTypeId = _typeId, Price = _price, 
            Image = _image, ItemCategoryId = _categoryId, Description = _descroption};
        _expectedCatalogItem = new CatalogItem { Id = _expectedId, Name = _name, 
            ItemBrandId = _brandId, ItemTypeId = _typeId, Price = _price, 
            Image = _image, ItemCategoryId = _categoryId, Description = _descroption};
        _catalogItemDto = new CatalogItemDto { Name = _name, 
            ItemBrandId = _brandId, ItemTypeId = _typeId, Price = _price, 
            Image = _image, ItemCategoryId = _categoryId, Description = _descroption};
    }

    [Fact]
    public async Task AddCatalogItemAsync_Success()
    {
        _catalogItemRepo.Setup(s => 
                s.AddToCatalog(It.IsAny<CatalogItem>()))
                .ReturnsAsync(1);

        var result = await _catalogItemService.AddToCatalog(_catalogItemDto);
        result.Should().Be(_expectedId);
    }
    
    [Fact]
    public async Task UpdateCatalogItemAsync_Success()
    {
        _catalogItemRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<CatalogItem>()))
            .ReturnsAsync(_givenCatalogItem);

        var result = await _catalogItemService.UpdateInCatalog(_givenId, _catalogItemDto);
        result.Should().Be(_expectedCatalogItem);
    }

    [Fact]
    public async Task UpdateCatalogItemAsync_Failed()
    {
        _catalogItemRepo.Setup(s => 
                    s.UpdateInCatalog(It.IsAny<CatalogItem>()))
                .ThrowsAsync(new Exception($"CatalogItem with ID: {_notExistId} does not exist"));
        var result = async () => await _catalogItemService.UpdateInCatalog(_notExistId, _catalogItemDto); 
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task FindCatalogItemByIdAsync_Success()
    {
        _catalogItemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenCatalogItem);

        var result = await _catalogItemService.FindById(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedCatalogItem);
    }
    
    [Fact]
    public async Task FindCatalogItemByIdAsync_Failed()
    {
        _catalogItemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"CatalogItem with ID: {_notExistId} does not exist"));

        var result = async () => await _catalogItemService.FindById(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task RemoveCatalogItemAsync_Success()
    {
        _catalogItemRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ReturnsAsync(_givenCatalogItem);

        var result = await _catalogItemService.RemoveFromCatalog(_givenId);
        result.Should().Be(_expectedCatalogItem);
    }
    
    [Fact]
    public async Task RemoveCatalogItemAsync_Failed()
    {
        _catalogItemRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"CatalogItem with ID: {_notExistId} does not exist"));

        var result = async () => await _catalogItemService.RemoveFromCatalog(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        var expected = new List<CatalogItem>
        {
            _givenCatalogItem, _givenCatalogItem
        };
        _catalogItemRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<CatalogItem>
        {
            _givenCatalogItem, _givenCatalogItem
        });

        var result = await _catalogItemService.GetCatalog();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        var expected = new List<CatalogItem>();
        _catalogItemRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<CatalogItem>());

        var result = await _catalogItemService.GetCatalog();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
    
    
}