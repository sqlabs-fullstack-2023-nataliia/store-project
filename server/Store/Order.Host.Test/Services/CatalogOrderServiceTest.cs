using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Host.DbContextData.Entities;
using Order.Host.Dto;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services;
using Order.Host.Services.Interfaces;
using Xunit;

namespace Order.Host.Test.Services;

public class CatalogOrderServiceTest
{
    private readonly Mock<ICatalogOrderRepository> _orderRepo;
    private readonly IOrderService<CatalogOrder, CatalogOrderDto> _orderService;
    private readonly Mock<ILogger<CatalogOrderService>> _logger;
    private readonly int _expectedId = 1;
    private readonly int _givenId = 1;
    private readonly int _notExistId = 2;
    private readonly int _quantity = 2;
    private readonly decimal _price = 120;
    private readonly string _userId = "user";
    private readonly CatalogOrder _givenOrder;
    private readonly CatalogOrder _expectedOrder;
    private readonly CatalogOrderDto _orderDto;
    
    public CatalogOrderServiceTest()
    {
        _orderRepo = new Mock<ICatalogOrderRepository>();
        _logger = new Mock<ILogger<CatalogOrderService>>();
        _orderService = new CatalogOrderService(_orderRepo.Object, _logger.Object);
        _givenOrder = new CatalogOrder { Id = _expectedId, Date = DateTime.Now.ToShortDateString(), 
            TotalQuantity = _quantity, TotalPrice = _price, UserId = _userId};
        _expectedOrder = new CatalogOrder { Id = _expectedId, Date = DateTime.Now.ToShortDateString(), 
            TotalQuantity = _quantity, TotalPrice = _price, UserId = _userId };
        _orderDto = new CatalogOrderDto { Date = DateTime.Now.ToShortDateString(), 
            TotalQuantity = _quantity, TotalPrice = _price, UserId = _userId };
    }

    [Fact]
    public async Task AddOrderAsync_Success()
    {
        _orderRepo.Setup(s => 
                s.AddItem(It.IsAny<CatalogOrder>()))
                .ReturnsAsync(1);

        var result = await _orderService.AddItem(_orderDto);
        result.Should().Be(_expectedId);
    }

    [Fact]
    public async Task UpdateOrderAsync_Success()
    {
        _orderRepo.Setup(s => s
                .UpdateItem(It.IsAny<CatalogOrder>()))
            .ReturnsAsync(_givenOrder);

        var result = await _orderService.UpdateItem(_givenId, _orderDto);
        result.Should().Be(_expectedOrder);
    }

    [Fact]
    public async Task UpdateOrderAsync_Failed()
    {
        {
            _orderRepo.Setup(s => s
                    .UpdateItem(It.IsAny<CatalogOrder>()))
                .ThrowsAsync(new Exception($"Order with ID: {_notExistId} does not exist"));

            var result = async () => await _orderService.UpdateItem(_notExistId, _orderDto);
            await Xunit.Assert.ThrowsAsync<Exception>(result);
        }
    }

    [Fact]
    public async Task FindOrderByIdAsync_Success()
    {
        _orderRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenOrder);

        var result = await _orderService.FindById(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedOrder);
    }
    
    [Fact]
    public async Task FindOrderByIdAsync_Failed()
    {
        _orderRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Order with ID: {_notExistId} does not exist"));

        var result = async () => await _orderService.FindById(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task RemoveOrderAsync_Success()
    {
        _orderRepo.Setup(s => s
                .RemoveItem(It.IsAny<int>()))
            .ReturnsAsync(_givenOrder);

        var result = await _orderService.RemoveItem(_givenId);
        result.Should().Be(_expectedOrder);
    }
    
    [Fact]
    public async Task RemoveOrderAsync_Failed()
    {
        _orderRepo.Setup(s => s
                .RemoveItem(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Order with ID: {_notExistId} does not exist"));

        var result = async () => await _orderService.RemoveItem(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task GetOrdersAsync_Success()
    {
        var expected = new List<CatalogOrder>
        {
            _givenOrder, _givenOrder
        };
        _orderRepo.Setup(s => s
            .GetItems()).ReturnsAsync(new List<CatalogOrder>
        {
            _givenOrder, _givenOrder
        });

        var result = await _orderService.GetItems();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetOrdersAsync_Failed()
    {
        var expected = new List<CatalogOrder>();
        _orderRepo.Setup(s => s
            .GetItems()).ReturnsAsync(new List<CatalogOrder>());

        var result = await _orderService.GetItems();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
}