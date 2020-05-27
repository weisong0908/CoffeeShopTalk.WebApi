using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeShopTalk.WebApi.UserIdProvider
{
    public class EmailBasedUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirstValue("sub");
        }
    }
}