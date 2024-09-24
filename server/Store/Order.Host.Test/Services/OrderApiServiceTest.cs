using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Host.DbContextData.Entities;
using Order.Host.Models;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services;
using Order.Host.Services.Interfaces;
using Xunit;

namespace Order.Host.Test.Services;

public class OrderApiServiceTest
{
    private readonly Mock<IOrderRepository<OrderItem>> _orderItemRepo;
    private readonly Mock<ICatalogOrderRepository> _catalogOrderRepo;
    private readonly IOrderApiService _apiService;
    private readonly Mock<ILogger<OrderApiService>> _logger;
    
    private readonly int _expectedOrderId = 1;
    private readonly int _givenOrderId = 1;
    private readonly decimal _price = 170;
    private readonly int _quantity = 2;
    private readonly string _userId = "user";
    private readonly string _wrongUserId = "user-failed";
    
    private readonly CatalogOrder _givenOrder;
    private readonly CatalogOrder _expectedOrder;
    private readonly OrderItemModel _orderItemModel;
    private readonly OrderItemModel _orderItemModel2;

    
    public OrderApiServiceTest()
    {
        _orderItemRepo = new Mock<IOrderRepository<OrderItem>>();
        _catalogOrderRepo = new Mock<ICatalogOrderRepository>();
        _logger = new Mock<ILogger<OrderApiService>>();
        _apiService = new OrderApiService(_catalogOrderRepo.Object, _orderItemRepo.Object, _logger.Object);

        _givenOrder = new CatalogOrder { Id = _givenOrderId, Date = DateTime.Now.ToShortDateString(), 
            TotalQuantity = _quantity, TotalPrice = _price, UserId = _userId};
        _expectedOrder = new CatalogOrder { Id = _expectedOrderId, Date = DateTime.Now.ToShortDateString(), 
            TotalQuantity = _quantity, TotalPrice = _price, UserId = _userId };

        _orderItemModel = new OrderItemModel { };
        _orderItemModel2 = new OrderItemModel { };

    }

    [Fact]
    public async Task CreateOrderAsyncTest_Success()
    {
        _catalogOrderRepo.Setup(s => s.AddItem(It.IsAny<CatalogOrder>())).ReturnsAsync(1);
        _orderItemRepo.SetupSequence(s => s.AddItem(It.IsAny<OrderItem>()))
            .ReturnsAsync(1)
            .ReturnsAsync(2);
        _catalogOrderRepo.Setup(s => s.UpdateItem(It.IsAny<CatalogOrder>())).ReturnsAsync(_givenOrder);
        
        var result = await _apiService.CreateOrder(new List<OrderItemModel> { _orderItemModel, _orderItemModel2 }, _userId);
        
        result.Should().Be(_expectedOrderId);
        _catalogOrderRepo.Verify(s => s.AddItem(It.IsAny<CatalogOrder>()), Times.Once);
        _orderItemRepo.Verify(s => s.AddItem(It.IsAny<OrderItem>()), Times.Exactly(2));
        _catalogOrderRepo.Verify(s => s.UpdateItem(It.IsAny<CatalogOrder>()), Times.Once);

    }

    [Fact]
    public async Task CreateOrderAsyncTest_Failed()
    {
        _catalogOrderRepo.Setup(s => s.AddItem(It.IsAny<CatalogOrder>())).ReturnsAsync((int?)null);
        
        var result = await _apiService.CreateOrder(new List<OrderItemModel> { _orderItemModel, _orderItemModel2 }, _userId);
        
        result.Should().BeNull();
        _catalogOrderRepo.Verify(s => s.AddItem(It.IsAny<CatalogOrder>()), Times.Once);
        _orderItemRepo.Verify(s => s.AddItem(It.IsAny<OrderItem>()), Times.Never);
        _catalogOrderRepo.Verify(s => s.UpdateItem(It.IsAny<CatalogOrder>()), Times.Never);
    }

    
    [Fact]
    public async Task GetOrdersByUserIdAsyncTest_Success()
    {
        var expected = new List<CatalogOrder> { _expectedOrder, _expectedOrder };
        _catalogOrderRepo.Setup(s => 
                s.GetOrdersByUserId(It.IsAny<string>()))
            .ReturnsAsync(new List<CatalogOrder>{ _expectedOrder, _expectedOrder });

        var result = await _apiService.GetOrdersByUserId(_userId);
        result.Should().BeEquivalentTo(expected);
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetOrdersByUserIdAsyncTest_Failed()
    {
        var expected = new List<CatalogOrder>();
        _catalogOrderRepo.Setup(s => 
            s.GetOrdersByUserId(It.IsAny<string>())).ReturnsAsync(new List<CatalogOrder>());

        var result = await _apiService.GetOrdersByUserId(_wrongUserId);
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }
}