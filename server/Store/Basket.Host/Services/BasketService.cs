using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using ExceptionHandler;
using Newtonsoft.Json;

namespace Basket.Host.Services;

public class BasketService : IBasketService
{
    private readonly ILogger<BasketService> _logger;
    private readonly ICacheService _cacheService;
    private readonly IHttpClientService _httpClient;
    private readonly string _catalogBaseUrl = "http://localhost:5288/catalog-bff-controller";
    private readonly string _orderBaseUrl = "http://localhost:5230/order-api-controller";

    public BasketService(ILogger<BasketService> logger,
        ICacheService cacheService, IHttpClientService httpClient)
    {
        _logger = logger;
        _cacheService = cacheService;
        _httpClient = httpClient;
    }

    public async Task AddItem(string userId, Item item)
    {
        try
        {
            _logger.LogInformation($"*{GetType().Name}* adding {item.Quantity} item with id: {item.ItemId}");
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{_catalogBaseUrl}/items/decrease")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new List<Item>
                {
                    new Item
                        {
                            ItemId = item.ItemId,
                            Quantity = item.Quantity
                        }
                }), Encoding.UTF8, "application/json")
            };
            
            //requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2NmUwN2U0NmMwNGQ5NTgzYWY4MmRlYWYiLCJ1c2VybmFtZSI6Im5hdGFsaWlhIiwiYXVkIjoidGVzdCIsImlzcyI6InRlc3QiLCJpYXQiOjE3MjU5OTYzOTQsImV4cCI6MTczMTE4MDM5NH0.oA9LKm8x_2AgYXF1BOy40pm6X9qhyKUpLmOISF3y_jk");
            
            await _httpClient.SendAsync(requestMessage, userId);
            await _cacheService.AddOrUpdateAsync(userId, item.ItemId, item.Quantity);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public async Task RemoveItem(string userId, Item item)
    {
        try
        {
            _logger.LogInformation($"*{GetType().Name}* request to remove {item.Quantity} items: {item.ItemId}");
            var response = await _httpClient.SendAsync<Task, object>(
                $"{_catalogBaseUrl}/items/increase",
                HttpMethod.Put, new List<Item>()
                {
                    new()
                    {
                        ItemId = item.ItemId, Quantity = item.Quantity
                    }
                }, userId);
            await _cacheService.RemoveOrUpdateAsync(userId, item.ItemId, item.Quantity);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<List<OrderItem>> GetItems(string userId)
    {
        var items = await GetBasketItems(userId);
        _logger.LogInformation($"*{GetType().Name}* found {items.Count} items in the basket");
        List<OrderItem> orderItems = new List<OrderItem>();
        if (items.Count == 0)
        {
            return orderItems;
        }

        orderItems = await GetOrderItems(items, userId);

        _logger.LogInformation($"*{GetType().Name}* items in the basket: {string.Join(", ", orderItems)}");


        return orderItems;
    }

    private async Task<List<OrderItem>> GetOrderItems(List<Item> basketItems, String userId)
    {
        List<OrderItem> res = new List<OrderItem>();
        try
        {
            foreach (var i in basketItems)
            {
                var catalogItem = await _httpClient.SendAsync<ItemModel, object>(
                    $"{_catalogBaseUrl}/items/{i.ItemId}",
                    HttpMethod.Get, null, userId);
                res.Add(new OrderItem()
                {
                    ItemId = i.ItemId,
                    BrandId = catalogItem.CatalogItem.ItemBrandId,
                    Price = catalogItem.CatalogItem.Price,
                    Quantity = i.Quantity,
                    Size = catalogItem.Size,
                    Name = catalogItem.CatalogItem.Name,
                    Image = catalogItem.CatalogItem.Image,
                    StockQuantity = catalogItem.Quantity
                });
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }

        return res;
    }

    private async Task<List<Item>> GetBasketItems(string userId)
    {
        var items = await _cacheService.GetAsync(userId);
        var basketItems = items
            .Select(entry => new Item
            {
                ItemId = int.Parse(entry.Name),
                Quantity = int.Parse(entry.Value),
            })
            .ToList();
        _logger.LogInformation($"*{GetType().Name}* total items in the basket: {basketItems.Count}");
        return basketItems;
    }

    public async Task RemoveAll(string userId)
    {
        try
        {
            var items = await GetBasketItems(userId);
            _logger.LogInformation($"*{GetType().Name}* total items to remove: {items.Count}");
            await _cacheService.RemoveAllAsync(userId);
            await _httpClient.SendAsync<Task, object>(
                $"{_catalogBaseUrl}/items/increase",
                HttpMethod.Post, items, userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<int> CheckoutBasket(string userId)
    {
        try
        {
            var items = await GetItems(userId);
            _logger.LogInformation($"*USER_ID ********  {userId}");
            _logger.LogInformation($"*{GetType().Name}* total items to check-out: {items.Count}");
            var orderId = await _httpClient.SendAsync<int, object>(
                $"{_orderBaseUrl}/orders/{userId}",
                HttpMethod.Post, items, userId);
            _logger.LogInformation($"*{GetType().Name}* check-out completed with order id: {orderId}");
            await RemoveAll(userId);
            return orderId;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}