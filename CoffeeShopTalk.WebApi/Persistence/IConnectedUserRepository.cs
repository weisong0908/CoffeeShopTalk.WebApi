using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;

namespace CoffeeShopTalk.WebApi.Persistence
{
    public interface IConnectedUserRepository
    {
        Task<IEnumerable<User>> GetConnectedUsers();
    }
}