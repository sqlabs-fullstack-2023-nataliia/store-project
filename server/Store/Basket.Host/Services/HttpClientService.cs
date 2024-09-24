using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using ExceptionHandler;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace Basket.Host.Services;

public class HttpClientService: IHttpClientService
{
       private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<HttpClientService> _logger;

    public HttpClientService(IHttpClientFactory clientFactory,
        ILogger<HttpClientService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }
    public async Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content, String userId)
    {
        var client = _clientFactory.CreateClient();
        
        var accessToken = await GetClientCredentialsTokenAsync(userId);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var httpMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(url),
            Method = method
        };

        if (content != null)
        {
            httpMessage.Content =
                new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }
        
        var result = await client.SendAsync(httpMessage);
        _logger.LogInformation($"*{GetType().Name}* status code: {result.StatusCode}");
        if (result.IsSuccessStatusCode)
        {
            var resultContent = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TResponse>(resultContent);
            _logger.LogInformation($"*{GetType().Name}* response: {response}");

            return response;
        } 
        if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            var response = await result.Content.ReadAsStringAsync();
            _logger.LogInformation($"*{GetType().Name}* response: {response}");
            throw new IllegalArgumentException(response);
        }
        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            var response = await result.Content.ReadAsStringAsync();
            _logger.LogInformation($"*{GetType().Name}* response: {response}");
            throw new NotFoundException(response);
        }

        return default !;
    }
    
    public async Task SendAsync(HttpRequestMessage request, String userId)
    {
        var client = _clientFactory.CreateClient();
        client.SetBearerToken(await GetClientCredentialsTokenAsync(userId));
        //client.SetBearerToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2NmUwN2U0NmMwNGQ5NTgzYWY4MmRlYWYiLCJ1c2VybmFtZSI6Im5hdGFsaWlhIiwiYXVkIjoidGVzdCIsImlzcyI6InRlc3QiLCJpYXQiOjE3MjU5OTYzOTQsImV4cCI6MTczMTE4MDM5NH0.oA9LKm8x_2AgYXF1BOy40pm6X9qhyKUpLmOISF3y_jk");
        
        var result = await client.SendAsync(request);
        _logger.LogInformation($"*{GetType().Name}* status code: {result.StatusCode}");

        if (!result.IsSuccessStatusCode)
        {
            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                var response = await result.Content.ReadAsStringAsync();
                _logger.LogInformation($"*{GetType().Name}* response: {response}");
                throw new IllegalArgumentException(response);
            }
            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                var response = await result.Content.ReadAsStringAsync();
                _logger.LogInformation($"*{GetType().Name}* response: {response}");
                throw new NotFoundException(response);
            }

            var errorContent = await result.Content.ReadAsStringAsync();
            _logger.LogError($"*{GetType().Name}* Error response: {errorContent}");
            throw new Exception($"HTTP Request failed with status code {result.StatusCode}: {errorContent}");
        }
    }
    
    private async Task<string> GetClientCredentialsTokenAsync(String userId)
    {
        var client = _clientFactory.CreateClient();
    
        var requestUrl = "http://localhost:3001/auth/get-token";

        var requestData = new
        {
            userId = userId
        };
        
        var json = JsonConvert.SerializeObject(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(requestUrl, content);

        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
        {
            var tokenResult = await response.Content.ReadAsStringAsync();
            var tokenObj = JsonConvert.DeserializeObject<TokenResponse>(tokenResult);
            _logger.LogInformation($"*TEST **: {tokenResult}");

            if (tokenObj != null && !string.IsNullOrEmpty(tokenObj.access_token))
            {
                _logger.LogInformation($"*{GetType().Name}* response token: {tokenObj.access_token}");
                return tokenObj.access_token;
            }
        }
        
        _logger.LogError($"*{GetType().Name}* request failed with the following error: {response.ReasonPhrase}");
        throw new Exception($"RequestClientCredentialsTokenAsync failed with the following error: {response.ReasonPhrase}");
    }

}