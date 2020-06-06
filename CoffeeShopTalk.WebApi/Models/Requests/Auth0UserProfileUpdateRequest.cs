using System.Text.Json.Serialization;

namespace CoffeeShopTalk.WebApi.Models.Requests
{
    public class Auth0UserProfileUpdateRequest
    {
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("picture")]
        public string Picture { get; set; }
    }
}