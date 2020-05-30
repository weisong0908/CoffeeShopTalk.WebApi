using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
using CoffeeShopTalk.WebApi.Persistence;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopTalk.WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly CoffeeShopTalkDbContext _dbContext;

        public ChatHub(CoffeeShopTalkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(Message message)
        {
            message.Time = DateTime.Now;
            message.Sender = Context.User.Claims.FirstOrDefault(c => c.Type == "https://coffee-shop-talk-stg/user.name").Value;

            var recipients = new List<string>() { Context.UserIdentifier };

            if (recipients.Contains(message.RecipientId))
                recipients.Add(message.RecipientId);

            var connectionInfo = _dbContext.Users.Include(u => u.Connection).SingleOrDefault(u => u.UserId == Context.UserIdentifier);

            await Clients.Users(recipients).SendAsync("ReceiveMessage", message, connectionInfo);
        }

        public override async Task OnConnectedAsync()
        {
            var user = _dbContext.Users.Include(user => user.Connection).SingleOrDefault(user => user.UserId == Context.UserIdentifier);

            if (user == null)
            {
                user = new User()
                {
                    UserId = Context.UserIdentifier,
                    Username = Context.User.Claims.FirstOrDefault(c => c.Type == "https://coffee-shop-talk-stg/user.name").Value,
                    Connection = new Connection()
                    {
                        ConnectionId = Context.ConnectionId,
                        UserAgent = Context.GetHttpContext().Request.Headers["User-Agent"],
                        IsConnected = true
                    }
                };
                _dbContext.Users.Add(user);
            }
            else
            {
                user.Connection = new Connection()
                {
                    ConnectionId = Context.ConnectionId,
                    UserAgent = Context.GetHttpContext().Request.Headers["User-Agent"],
                    IsConnected = true
                };
                _dbContext.Users.Update(user);
            }

            _dbContext.SaveChanges();

            await Clients.All.SendAsync("OnConnected", user);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = _dbContext.Users.Include(user => user.Connection).SingleOrDefault(user => user.UserId == Context.UserIdentifier);

            user.Connection.IsConnected = false;
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();

            await Clients.All.SendAsync("OnDisconnected", user, exception);
        }
    }
}