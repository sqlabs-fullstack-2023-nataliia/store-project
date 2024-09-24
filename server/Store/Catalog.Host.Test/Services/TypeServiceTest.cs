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

public class TypeServiceTest
{
    private readonly Mock<ICatalogRepository<ItemType>> _typeRepo;
    private readonly ICatalogService<ItemType, TypeDto> _typeService;
    private readonly Mock<ILogger<TypeService>> _logger;
    private readonly int _expectedId = 1;
    private readonly int _givenId = 1;
    private readonly int _notExistId = 2;
    private readonly ItemType _givenType;
    private readonly ItemType _expectedType;
    private readonly TypeDto _typeDto;
    private readonly string _typeName = "type-name";


    public TypeServiceTest()
    {
        _typeRepo = new Mock<ICatalogRepository<ItemType>>();
        _logger = new Mock<ILogger<TypeService>>();
        _typeService = new TypeService(_typeRepo.Object, _logger.Object);
        _givenType = new ItemType { Id = _expectedId, Type = _typeName };
        _expectedType = new ItemType { Id = _expectedId, Type = _typeName };
        _typeDto = new TypeDto { Type = _typeName };
    }

    [Fact]
    public async Task AddTypeAsync_Success()
    {
        _typeRepo.Setup(s => 
                s.AddToCatalog(It.IsAny<ItemType>()))
                .ReturnsAsync(1);

        var result = await _typeService.AddToCatalog(_typeDto);
        result.Should().Be(_expectedId);
    }

    [Fact]
    public async Task UpdateTypeAsync_Success()
    {
        _typeRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<ItemType>()))
            .ReturnsAsync(_givenType);

        var result = await _typeService.UpdateInCatalog(_givenId, _typeDto);
        result.Should().Be(_expectedType);
    }

    [Fact]
    public async Task UpdateTypeAsync_Failed()
    {
        _typeRepo.Setup(s => 
                    s.UpdateInCatalog(It.IsAny<ItemType>()))
                .ThrowsAsync(new Exception($"Type with ID: {_notExistId} does not exist"));
        var result = async () => await _typeService.UpdateInCatalog(_notExistId, _typeDto); 
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task FindTypeByIdAsync_Success()
    {
        _typeRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenType);

        var result = await _typeService.FindById(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedType);
    }
    
    [Fact]
    public async Task FindTypeByIdAsync_Failed()
    {
        _typeRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Type with ID: {_notExistId} does not exist"));

        var result = async () => await _typeService.FindById(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task RemoveTypeAsync_Success()
    {
        _typeRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ReturnsAsync(_givenType);

        var result = await _typeService.RemoveFromCatalog(_givenId);
        result.Should().Be(_expectedType);
    }
    
    [Fact]
    public async Task RemoveTypeAsync_Failed()
    {
        _typeRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Type with ID: {_notExistId} does not exist"));

        var result = async () => await _typeService.RemoveFromCatalog(_notExistId);
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

        var result = await _typeService.GetCatalog();
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

        var result = await _typeService.GetCatalog();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
}