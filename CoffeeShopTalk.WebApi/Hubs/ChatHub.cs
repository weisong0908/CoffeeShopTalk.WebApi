using System;
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

            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}