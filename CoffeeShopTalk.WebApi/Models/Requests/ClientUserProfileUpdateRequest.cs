using Microsoft.AspNetCore.Http;

namespace CoffeeShopTalk.WebApi.Models.Requests
{
    public class ClientUserProfileUpdateRequest
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}