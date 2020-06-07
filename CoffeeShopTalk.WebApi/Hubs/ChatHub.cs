using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
using CoffeeShopTalk.WebApi.Persistence;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeShopTalk.WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConnectedUserRepository _connectedUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IConnectedUserRepository connectedUserRepository, IUnitOfWork unitOfWork)
        {
            _connectedUserRepository = connectedUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task SendMessage(Message message)
        {
            message.Time = DateTime.Now;
            message.Sender = Context.User.Claims.FirstOrDefault(c => c.Type == "https://coffee-shop-talk/user.name").Value;

            var recipients = new List<string>() { Context.UserIdentifier };
            recipients.Add(message.RecipientId);

            var connectionInfo = await _connectedUserRepository.GetUser(Context.UserIdentifier);

            await Clients.Users(recipients).SendAsync("ReceiveMessage", message, connectionInfo);
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _connectedUserRepository.GetUser(Context.UserIdentifier);

            if (user == null)
                await _connectedUserRepository.ConnectUser(Context);
            else
                _connectedUserRepository.UpdateUser(user, Context);

            await _unitOfWork.CompleteAsync();

            var users = await _connectedUserRepository.GetUsers();

            await Clients.All.SendAsync("OnConnected", users);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _connectedUserRepository.GetUser(Context.UserIdentifier);

            _connectedUserRepository.DisconnectUser(user);

            await _unitOfWork.CompleteAsync();

            var users = await _connectedUserRepository.GetUsers();

            await Clients.All.SendAsync("OnDisconnected", users, exception);
        }
    }
}