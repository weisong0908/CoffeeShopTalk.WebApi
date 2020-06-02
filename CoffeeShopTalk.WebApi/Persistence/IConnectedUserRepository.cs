using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeShopTalk.WebApi.Persistence
{
    public interface IConnectedUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(string userId);
        Task ConnectUser(HubCallerContext context);
        void UpdateUser(User user, HubCallerContext context);
        void DisconnectUser(User user);
    }
}