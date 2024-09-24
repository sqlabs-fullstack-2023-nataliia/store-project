using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Models;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.Host.Test.Services;

public class BffCatalogServiceTest
{
    private readonly Mock<ICatalogItemRepository> _catalogItemRepo;
    private readonly Mock<IItemRepository> _itemRepo;
    private readonly Mock<ICatalogRepository<ItemBrand>> _brandRepo;
    private readonly Mock<ICatalogRepository<ItemType>> _typeRepo;
    private readonly Mock<ICatalogRepository<ItemCategory>> _categoryRepo;
    private readonly IBffService _bffService;
    private readonly Mock<ILogger<BffService>> _logger;
    private readonly int _expectedId = 1;
    private readonly int _givenId = 1;
    private readonly int _notExistId = 2;
    
    private readonly CatalogItem _givenCatalogItem;
    private readonly CatalogItem _expectedCatalogItem;
    private readonly string _name = "catalog-item-name";
    private readonly int _brandId = 1;
    private readonly int _typeId = 1;
    private readonly decimal _price = 100;
    private readonly string _image = "image-1.png";
    private readonly int _categoryId = 1;
    private readonly string _descroption = "catalog item description";
    
    private readonly int _catalogItemId = 1;
    private readonly int _quantity = 2;
    private readonly string _size = "s";
    private readonly Item _givenItem;
    private readonly Item _expectedItem;
    
    private readonly ItemBrand _givenBrand;
    private readonly ItemBrand _expectedBrand;
    private readonly string _brandName = "brand-name";
    
    private readonly ItemType _givenType;
    private readonly ItemType _expectedType;
    private readonly string _typeName = "type-name";
    
    private readonly ItemCategory _givenCategory;
    private readonly ItemCategory _expectedCategory;
    private readonly string _categoryName = "type-name";

    private readonly CatalogFilter _filter;
    private readonly List<OrderItem> _orderItems;

    public BffCatalogServiceTest()
    {
        _catalogItemRepo = new Mock<ICatalogItemRepository>();
        _itemRepo = new Mock<IItemRepository>();
        _brandRepo = new Mock<ICatalogRepository<ItemBrand>>();
        _typeRepo = new Mock<ICatalogRepository<ItemType>>();
        _categoryRepo = new Mock<ICatalogRepository<ItemCategory>>();
        _logger = new Mock<ILogger<BffService>>();
        _bffService = new BffService(_brandRepo.Object, _typeRepo.Object,
            _categoryRepo.Object, _itemRepo.Object, _catalogItemRepo.Object, 
            _logger.Object);
        
        _givenCatalogItem = new CatalogItem { Id = _expectedId, Name = _name, 
            ItemBrandId = _brandId, ItemTypeId = _typeId, Price = _price, 
            Image = _image, ItemCategoryId = _categoryId, Description = _descroption};
        _expectedCatalogItem = new CatalogItem { Id = _expectedId, Name = _name, 
            ItemBrandId = _brandId, ItemTypeId = _typeId, Price = _price, 
            Image = _image, ItemCategoryId = _categoryId, Description = _descroption};
        _givenItem = new Item { Id = _expectedId, 
            CatalogItemId = _catalogItemId, Quantity = _quantity, Size = _size};
        _expectedItem = new Item { Id = _expectedId, 
            CatalogItemId = _catalogItemId, Quantity = _quantity, Size = _size};
        _givenBrand = new ItemBrand { Id = _expectedId, Brand = _brandName };
        _expectedBrand = new ItemBrand { Id = _expectedId, Brand = _brandName };
        _givenType = new ItemType { Id = _expectedId, Type = _typeName };
        _expectedType = new ItemType { Id = _expectedId, Type = _typeName };
        _givenCategory = new ItemCategory { Id = _expectedId, Category = _categoryName };
        _expectedCategory = new ItemCategory { Id = _expectedId, Category = _categoryName };
        // TODO brand
        _filter = new CatalogFilter { Brand = "nike", Category = _categoryId, Type = _typeId };
        _orderItems = new List<OrderItem>() { new OrderItem(){ItemId = 1, Quantity = 1} };
    }
    
    [Fact]
    public async Task IncreaseItemQuantity_Success()
    {
        _itemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenItem);
        _itemRepo.Setup(s => s
                  .UpdateInCatalog(It.IsAny<Item>()))
              .ReturnsAsync(_givenItem);

        await _bffService.IncreaseItemQuantity(_orderItems);
    }
    
    [Fact]
    public async Task IncreaseItemQuantity_Failed()
    {
        _itemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenItem);
        _itemRepo.Setup(s => s
                  .UpdateInCatalog(It.IsAny<Item>()))
            .ThrowsAsync(new Exception($"Item with ID: {_notExistId} does not exist"));

        var result = async () => await _bffService.IncreaseItemQuantity(_orderItems);;
        await Xunit.Assert.ThrowsAsync<Exception>(result);

        
    }
    
    [Fact]
    public async Task DecreaseItemQuantity_Success()
    {
        _itemRepo.Setup(s => s.DecreaseItemQuantity(It.IsAny<List<OrderItem>>()))
            .Returns(Task.CompletedTask);
        await _bffService.DecreaseItemQuantity(_orderItems);
        _itemRepo.Verify(s => s.DecreaseItemQuantity(It.IsAny<List<OrderItem>>()), Times.Once);
    }
    
    [Fact]
    public async Task DecreaseItemQuantity_Failed()
    {
        _itemRepo.Setup(s => s.DecreaseItemQuantity(It.IsAny<List<OrderItem>>()))
            .ThrowsAsync(new Exception($"Item with id: 1 is not available in stock with quantity: 1"));
        await Xunit.Assert.ThrowsAsync<Exception>(() => _bffService.DecreaseItemQuantity(_orderItems));
    }

    
    [Fact]
    public async Task GetItemsByCatalogItemId_Success()
    {
        var expected = new List<Item>
        {
            _givenItem, _givenItem
        };
        _itemRepo.Setup(s => s
            .GetItemsByCatalogItemId(It.IsAny<int>())).ReturnsAsync(new List<Item>
        {
            _givenItem, _givenItem
        });

        var result = await _bffService.GetItemsByCatalogItemId(_catalogItemId);
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetItemsByCatalogItemId_Failed()
    {
        var expected = new List<Item> {};
        _itemRepo.Setup(s => s
            .GetItemsByCatalogItemId(It.IsAny<int>())).ReturnsAsync(new List<Item> { });

        var result = await _bffService.GetItemsByCatalogItemId(_catalogItemId);
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        var expected = new List<CatalogItem>
        {
            _givenCatalogItem, _givenCatalogItem
        };
        _catalogItemRepo.Setup(s => s
            .GetCatalog(It.IsAny<CatalogFilter>())).ReturnsAsync(new List<CatalogItem>
        {
            _givenCatalogItem, _givenCatalogItem
        });

        var result = await _bffService.GetCatalogItems(_filter);
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        var expected = new List<CatalogItem>();
        _catalogItemRepo.Setup(s => s
            .GetCatalog(It.IsAny<CatalogFilter>())).ReturnsAsync(new List<CatalogItem>());

        var result = await _bffService.GetCatalogItems(_filter);
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task FindCatalogItemByIdAsync_Success()
    {
        _catalogItemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenCatalogItem);

        var result = await _bffService.GetCatalogItem(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedCatalogItem);
    }
    
    [Fact]
    public async Task FindCatalogItemByIdAsync_Failed()
    {
        _catalogItemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"CatalogItem with ID: {_notExistId} does not exist"));

        var result = async () => await _bffService.GetCatalogItem(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task FindItemByIdAsync_Success()
    {
        _itemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenItem);

        var result = await _bffService.GetItem(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedItem);
    }
    
    [Fact]
    public async Task FindItemByIdAsync_Failed()
    {
        _itemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Item with ID: {_notExistId} does not exist"));

        var result = async () => await _bffService.GetItem(_notExistId);
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

        var result = await _bffService.GetItems();
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

        var result = await _bffService.GetItems();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task FindBrandByIdAsync_Success()
    {
        _brandRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenBrand);

        var result = await _bffService.GetBrand(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedBrand);
    }
    
    [Fact]
    public async Task FindBrandByIdAsync_Failed()
    {
        _brandRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Brand with ID: {_notExistId} does not exist"));

        var result = async () => await _bffService.GetBrand(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        var expected = new List<ItemBrand>
        {
            _givenBrand, _givenBrand
        };
        _brandRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<ItemBrand>
        {
            _givenBrand, _givenBrand
        });

        var result = await _bffService.GetBrands();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        var expected = new List<ItemBrand>();
        _brandRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<ItemBrand>());

        var result = await _bffService.GetBrands();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task FindTypeByIdAsync_Success()
    {
        _typeRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenType);

        var result = await _bffService.GetType(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedType);
    }
    
    [Fact]
    public async Task FindTypeByIdAsync_Failed()
    {
        _typeRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Type with ID: {_notExistId} does not exist"));

        var result = async () => await _bffService.GetType(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task GetTypesAsync_Success()
    {
        var expected = new List<ItemType>
        {
            _givenType, _givenType
        };
        _typeRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<ItemType>
        {
            _givenType, _givenType
        });

        var result = await _bffService.GetTypes();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        var expected = new List<ItemType>();
        _typeRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<ItemType>());

        var result = await _bffService.GetTypes();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task FindCategoryByIdAsync_Success()
    {
        _categoryRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenCategory);

        var result = await _bffService.GetCategory(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedCategory);
    }
    
    [Fact]
    public async Task FindCategoryByIdAsync_Failed()
    {
        _categoryRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Category with ID: {_notExistId} does not exist"));

        var result = async () => await _bffService.GetCategory(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task GetCategoriesAsync_Success()
    {
        var expected = new List<ItemCategory>
        {
            _givenCategory, _givenCategory
        };
        _categoryRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<ItemCategory>
        {
            _givenCategory, _givenCategory
        });

        var result = await _bffService.GetCategories();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetCategoriesAsync_Failed()
    {
        var expected = new List<ItemCategory>();
        _categoryRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<ItemCategory>());

        var result = await _bffService.GetCategories();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
}