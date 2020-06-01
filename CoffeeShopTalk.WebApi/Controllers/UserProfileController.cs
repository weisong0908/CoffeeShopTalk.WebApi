using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
using CoffeeShopTalk.WebApi.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoffeeShopTalk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public UserProfileController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] ClientUserProfileUpdateRequest request)
        {
            var userIdFromAccessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            if (userIdFromAccessToken != request.UserId)
                return BadRequest("Invalid user");

            var accessToken = await GetAccessToken();

            var client = _clientFactory.CreateClient("Auth0 Management API");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var userProfileUpdateRequest = new Auth0UserProfileUpdateRequest()
            {
                Name = request.Username
            };
            var content = new StringContent(JsonSerializer.Serialize(userProfileUpdateRequest), Encoding.UTF8, "application/json");
            var response = await client.PatchAsync("users/" + request.UserId, content);

            response.EnsureSuccessStatusCode();

            return Ok(await response.Content.ReadAsStringAsync());
        }

        private async Task<string> GetAccessToken()
        {
            var client = _clientFactory.CreateClient();

            var nvc = new Dictionary<string, string>();
            nvc.Add("grant_type", "client_credentials");
            nvc.Add("client_id", _configuration.GetValue<string>("Auth0:ClientId"));
            nvc.Add("client_secret", _configuration.GetValue<string>("Auth0:ClientSecret"));
            nvc.Add("audience", _configuration.GetValue<string>("Auth0:Audience"));

            var request = new FormUrlEncodedContent(nvc);
            var response = await client.PostAsync("https://coffee-shop-talk-dev.auth0.com/oauth/token", request);

            var data = await response.Content.ReadAsStringAsync();
            var data2 = JsonSerializer.Deserialize<Auth0ManagementApiAccessToken>(data);
            return data2.AccessToken;
        }
    }
}