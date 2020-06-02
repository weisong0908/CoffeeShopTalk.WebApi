using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
using CoffeeShopTalk.WebApi.Models.Requests;
using Microsoft.Extensions.Configuration;

namespace CoffeeShopTalk.WebApi.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public UserProfileService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;

            _client = _clientFactory.CreateClient("Auth0 Management API");
        }

        public async Task<string> Update(ClientUserProfileUpdateRequest clientRequest)
        {
            var accessToken = await GetAccessToken();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var serverRequest = new Auth0UserProfileUpdateRequest()
            {
                Name = clientRequest.Username
            };
            var content = new StringContent(JsonSerializer.Serialize(serverRequest), Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync("api/v2/users/" + clientRequest.UserId, content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> GetAccessToken()
        {
            var nvc = new Dictionary<string, string>();
            nvc.Add("grant_type", "client_credentials");
            nvc.Add("client_id", _configuration.GetValue<string>("Auth0:ClientId"));
            nvc.Add("client_secret", _configuration.GetValue<string>("Auth0:ClientSecret"));
            nvc.Add("audience", _configuration.GetValue<string>("Auth0:Audience"));

            var content = new FormUrlEncodedContent(nvc);
            var response = await _client.PostAsync("oauth/token", content);

            var data = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Auth0ManagementApiAccessToken>(data).AccessToken;
        }
    }
}