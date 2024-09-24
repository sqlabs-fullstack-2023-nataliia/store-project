using Newtonsoft.Json;

namespace Basket.Host.Models;

// Define a class to match the JSON response
public class ResponseData
{
    [JsonProperty("accessToken")]
    public string AccessToken { get; set; }
}