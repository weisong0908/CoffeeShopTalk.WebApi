using System.Text.Json.Serialization;

namespace CoffeeShopTalk.WebApi.Models.Requests
{
    public class Auth0UserProfileUpdateRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}