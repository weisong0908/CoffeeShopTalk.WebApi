using System;

namespace CoffeeShopTalk.WebApi.Models
{
    public class Message
    {
        public string SenderId { get; set; }
        public string Sender { get; set; }
        public string RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}