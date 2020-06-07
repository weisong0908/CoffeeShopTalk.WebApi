using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopTalk.WebApi.Persistence
{
    public class ConnectedUserRepository : IConnectedUserRepository
    {
        private readonly CoffeeShopTalkDbContext _dbContext;

        public ConnectedUserRepository(CoffeeShopTalkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _dbContext.Users.Include(user => user.Connection).ToListAsync();
        }

        public async Task<User> GetUser(string userId)
        {
            return await _dbContext.Users.Include(u => u.Connection).SingleOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task ConnectUser(HubCallerContext context)
        {
            var user = new User()
            {
                UserId = context.UserIdentifier,
                Username = context.User.Claims.FirstOrDefault(c => c.Type == "https://coffee-shop-talk/user.name").Value,
                ProfilePicture = context.User.Claims.FirstOrDefault(c => c.Type == "https://coffee-shop-talk/user.picture").Value,
                Connection = new Connection()
                {
                    ConnectionId = context.ConnectionId,
                    UserAgent = context.GetHttpContext().Request.Headers["User-Agent"],
                    IsConnected = true
                }
            };

            await _dbContext.Users.AddAsync(user);
        }

        public void UpdateUser(User user, HubCallerContext context)
        {
            user.Connection = new Connection()
            {
                ConnectionId = context.ConnectionId,
                UserAgent = context.GetHttpContext().Request.Headers["User-Agent"],
                IsConnected = true
            };
            _dbContext.Users.Update(user);
        }

        public void DisconnectUser(User user)
        {
            user.Connection.IsConnected = false;
            _dbContext.Users.Update(user);
        }
    }
}