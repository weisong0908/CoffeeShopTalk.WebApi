using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
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

        public async Task<IEnumerable<User>> GetConnectedUsers()
        {
            return await _dbContext.Users.Include(user => user.Connection).ToListAsync();
        }
    }
}