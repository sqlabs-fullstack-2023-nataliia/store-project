namespace Basket.Host.Services.Interfaces;

public interface IHttpClientService
{
    Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content, String userId);
    Task SendAsync(HttpRequestMessage request, String userId);
}