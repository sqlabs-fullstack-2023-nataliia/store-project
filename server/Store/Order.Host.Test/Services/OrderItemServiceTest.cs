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

public class OrderItemServiceTest
{
    private readonly Mock<IOrderRepository<OrderItem>> _orderItemRepo;
    private readonly IOrderService<OrderItem, OrderItemDto> _orderItemService;
    private readonly Mock<ILogger<OrderItemService>> _logger;
    private readonly int _expectedId = 1;
    private readonly int _givenId = 1;
    private readonly int _notExistId = 2;
    private readonly int _quantity = 2;
    private readonly decimal _subPrice = 120;
    private readonly int _itemId = 1;
    private readonly int _orderId = 1;
    private readonly OrderItem _givenOrderItem;
    private readonly OrderItem _expectedOrderItem;
    private readonly OrderItemDto _orderItemDto;
    
    public OrderItemServiceTest()
    {
        _orderItemRepo = new Mock<IOrderRepository<OrderItem>>();
        _logger = new Mock<ILogger<OrderItemService>>();
        _orderItemService = new OrderItemService(_orderItemRepo.Object, _logger.Object);
        _givenOrderItem = new OrderItem { Id = _expectedId, OrderId = _orderId, 
            ItemId = _itemId, SubPrice = _subPrice, Quantity = _quantity};
        _expectedOrderItem = new OrderItem { Id = _expectedId, OrderId = _orderId, 
            ItemId = _itemId, SubPrice = _subPrice, Quantity = _quantity };
        _orderItemDto = new OrderItemDto { OrderId = _orderId, 
            ItemId = _itemId, SubPrice = _subPrice, Quantity = _quantity };
    }

    [Fact]
    public async Task AddOrderItemAsync_Success()
    {
        _orderItemRepo.Setup(s => 
                s.AddItem(It.IsAny<OrderItem>()))
                .ReturnsAsync(1);

        var result = await _orderItemService.AddItem(_orderItemDto);
        result.Should().Be(_expectedId);
    }

    [Fact]
    public async Task UpdateOrderItemAsync_Success()
    {
        _orderItemRepo.Setup(s => s
                .UpdateItem(It.IsAny<OrderItem>()))
            .ReturnsAsync(_givenOrderItem);

        var result = await _orderItemService.UpdateItem(_givenId, _orderItemDto);
        result.Should().Be(_expectedOrderItem);
    }

    [Fact]
    public async Task UpdateOrderItemAsync_Failed()
    {
        {
            _orderItemRepo.Setup(s => s
                    .UpdateItem(It.IsAny<OrderItem>()))
                .ThrowsAsync(new Exception($"Order Item with ID: {_notExistId} does not exist"));

            var result = async () => await _orderItemService.UpdateItem(_notExistId, _orderItemDto);
            await Xunit.Assert.ThrowsAsync<Exception>(result);
        }
    }

    [Fact]
    public async Task FindOrderItemByIdAsync_Success()
    {
        _orderItemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(_givenOrderItem);

        var result = await _orderItemService.FindById(_givenId);
        result.Should().NotBeNull();
        result.Should().Be(_expectedOrderItem);
    }
    
    [Fact]
    public async Task FindOrderItemByIdAsync_Failed()
    {
        _orderItemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Order Item with ID: {_notExistId} does not exist"));

        var result = async () => await _orderItemService.FindById(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task RemoveOrderItemAsync_Success()
    {
        _orderItemRepo.Setup(s => s
                .RemoveItem(It.IsAny<int>()))
            .ReturnsAsync(_givenOrderItem);

        var result = await _orderItemService.RemoveItem(_givenId);
        result.Should().Be(_expectedOrderItem);
    }
    
    [Fact]
    public async Task RemoveOrderItemAsync_Failed()
    {
        _orderItemRepo.Setup(s => s
                .RemoveItem(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Order Item with ID: {_notExistId} does not exist"));

        var result = async () => await _orderItemService.RemoveItem(_notExistId);
        await Xunit.Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task GetOrderItemsAsync_Success()
    {
        var expected = new List<OrderItem>
        {
            _givenOrderItem, _givenOrderItem
        };
        _orderItemRepo.Setup(s => s
            .GetItems()).ReturnsAsync(new List<OrderItem>
        {
            _givenOrderItem, _givenOrderItem
        });

        var result = await _orderItemService.GetItems();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }
    
    [Fact]
    public async Task GetOrderItemsAsync_Failed()
    {
        var expected = new List<OrderItem>();
        _orderItemRepo.Setup(s => s
            .GetItems()).ReturnsAsync(new List<OrderItem>());

        var result = await _orderItemService.GetItems();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
}