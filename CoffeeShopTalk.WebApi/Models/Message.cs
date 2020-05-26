using System;

namespace CoffeeShopTalk.WebApi.Models
{
    public class Message
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}