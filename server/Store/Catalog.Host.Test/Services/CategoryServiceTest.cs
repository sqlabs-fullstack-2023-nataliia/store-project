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

public class CategoryServiceTest
{
    private readonly Mock<ICatalogRepository<ItemCategory>> _categoryRepo;
    private readonly ICatalogService<ItemCategory, CategoryDto> _categoryService;
    private readonly Mock<ILogger<CategoryService>> _logger;
    private readonly int _expectedId = 1;
    private readonly int _givenId = 1;
    private readonly int _notExistId = 2;
    private readonly ItemCategory _givenCategory;
    private readonly ItemCategory _expectedCategory;
    private readonly CategoryDto _categoryDto;
    private readonly string _categoryName = "type-name";


    public CategoryServiceTest()
    {
        _categoryRepo = new Mock<ICatalogRepository<ItemCategory>>();
        _logger = new Mock<ILogger<CategoryService>>();
        _categoryService = new CategoryService(_categoryRepo.Object, _logger.Object);
        _givenCategory = new ItemCategory { Id = _expectedId, Category = _categoryName };
        _expectedCategory = new ItemCategory { Id = _expectedId, Category = _categoryName };
        _categoryDto = new CategoryDto { Category = _categoryName };
    }

    [Fact]
    public async Task AddCategoryAsync_Success()
    {
        _categoryRepo.Setup(s => 
                s.AddToCatalog(It.IsAny<ItemCategory>()))
                .ReturnsAsync(1);

        var result = await _categoryService.AddToCatalog(_categoryDto);
        result.Should().Be(_expectedId);
    }

    [Fact]
    public async Task UpdateCategoryAsync_Success()
    {
        _categoryRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<ItemCategory>()))
            .ReturnsAsync(_givenCategory);

        var result = await _categoryService.UpdateInCatalog(_givenId, _categoryDto);
        result.Should().Be(_expectedCategory);
    }

    [Fact]
    public async Task UpdateCategoryAsync_Failed()
    {
        _categoryRepo.Setup(s => 
                    s.UpdateInCatalog(It.IsAny<ItemCategory>()))
                .ThrowsAsync(new Exception($"Category with ID: {_notExistId} does not exist"));
        var result = async () => await _categoryService.UpdateInCatalog(_notExistId, _categoryDto); 
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task FindCategoryByIdAsync_Success()
    {
        _categoryRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenCategory);

        var result = await _categoryService.FindById(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedCategory);
    }
    
    [Fact]
    public async Task FindCategoryByIdAsync_Failed()
    {
        _categoryRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Category with ID: {_notExistId} does not exist"));

        var result = async () => await _categoryService.FindById(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task RemoveCategoryAsync_Success()
    {
        _categoryRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ReturnsAsync(_givenCategory);

        var result = await _categoryService.RemoveFromCatalog(_givenId);
        result.Should().Be(_expectedCategory);
    }
    
    [Fact]
    public async Task RemoveCategoryAsync_Failed()
    {
        _categoryRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Category with ID: {_notExistId} does not exist"));

        var result = async () => await _categoryService.RemoveFromCatalog(_notExistId);
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

        var result = await _categoryService.GetCatalog();
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

        var result = await _categoryService.GetCatalog();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
}