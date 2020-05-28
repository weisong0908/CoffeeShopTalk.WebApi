using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeShopTalk.WebApi.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            message.Time = DateTime.Now;

            var recipients = new List<string>() { Context.UserIdentifier };

            if (recipients.Contains(message.RecipientId))
                recipients.Add(message.RecipientId);

            await Clients.Users(recipients).SendAsync("ReceiveMessage", message);
        }
    }
}