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

public class BrandServiceTest
{
    private readonly Mock<ICatalogRepository<ItemBrand>> _brandRepo;
    private readonly ICatalogService<ItemBrand, BrandDto> _brandService;
    private readonly Mock<ILogger<BrandService>> _logger;
    private readonly int _expectedId = 1;
    private readonly int _givenId = 1;
    private readonly int _notExistId = 2;
    private readonly ItemBrand _givenBrand;
    private readonly ItemBrand _expectedBrand;
    private readonly BrandDto _brandDto;
    private readonly string _brandName = "brand-name";


    public BrandServiceTest()
    {
        _brandRepo = new Mock<ICatalogRepository<ItemBrand>>();
        _logger = new Mock<ILogger<BrandService>>();
        _brandService = new BrandService(_brandRepo.Object, _logger.Object);
        _givenBrand = new ItemBrand { Id = _expectedId, Brand = _brandName };
        _expectedBrand = new ItemBrand { Id = _expectedId, Brand = _brandName };
        _brandDto = new BrandDto { Brand = _brandName };
    }

    [Fact]
    public async Task AddBrandAsync_Success()
    {
        _brandRepo.Setup(s => 
                s.AddToCatalog(It.IsAny<ItemBrand>()))
                .ReturnsAsync(1);

        var result = await _brandService.AddToCatalog(_brandDto);
        result.Should().Be(_expectedId);
    }

    [Fact]
    public async Task UpdateBrandAsync_Success()
    {
        _brandRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<ItemBrand>()))
            .ReturnsAsync(_givenBrand);

        var result = await _brandService.UpdateInCatalog(_givenId, _brandDto);
        result.Should().Be(_expectedBrand);
    }

    [Fact]
    public async Task UpdateBrandAsync_Failed()
    {
        {
            _brandRepo.Setup(s => s
                    .UpdateInCatalog(It.IsAny<ItemBrand>()))
                .ThrowsAsync(new Exception($"Brand with ID: {_notExistId} does not exist"));

            var result = async () => await _brandService.UpdateInCatalog(_notExistId, _brandDto);
            await Xunit.Assert.ThrowsAsync<Exception>(result);
        }
    }

    [Fact]
    public async Task FindBrandByIdAsync_Success()
    {
        _brandRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenBrand);

        var result = await _brandService.FindById(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedBrand);
    }
    
    [Fact]
    public async Task FindBrandByIdAsync_Failed()
    {
        _brandRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Brand with ID: {_notExistId} does not exist"));

        var result = async () => await _brandService.FindById(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task RemoveBrandAsync_Success()
    {
        _brandRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ReturnsAsync(_givenBrand);

        var result = await _brandService.RemoveFromCatalog(_givenId);
        result.Should().Be(_expectedBrand);
    }
    
    [Fact]
    public async Task RemoveBrandAsync_Failed()
    {
        _brandRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Brand with ID: {_notExistId} does not exist"));

        var result = async () => await _brandService.RemoveFromCatalog(_notExistId);
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

        var result = await _brandService.GetCatalog();
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

        var result = await _brandService.GetCatalog();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
    
}
