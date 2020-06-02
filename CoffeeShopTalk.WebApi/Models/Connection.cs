using Microsoft.AspNetCore.SignalR;

namespace CoffeeShopTalk.WebApi.Models
{
    public class Connection
    {
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
        public bool IsConnected { get; set; }
    }
}