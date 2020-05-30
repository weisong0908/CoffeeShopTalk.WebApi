using System.Collections.Generic;

namespace CoffeeShopTalk.WebApi.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public Connection Connection { get; set; }
    }
}