using System.Collections.Generic;

namespace CoffeeShopTalk.WebApi.Models
{
    public class User
    {
        public string UserId { get; set; }
        public IList<Connection> Connection { get; set; }
    }
}