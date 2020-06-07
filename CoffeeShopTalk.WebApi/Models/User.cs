using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeShopTalk.WebApi.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public Connection Connection { get; set; }
    }
}