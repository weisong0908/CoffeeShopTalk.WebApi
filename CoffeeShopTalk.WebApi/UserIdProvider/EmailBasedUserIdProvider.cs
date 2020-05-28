using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeShopTalk.WebApi.UserIdProvider
{
    public class EmailBasedUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var scope = connection.User.FindFirst(claim => claim.Type == "sub");
            return scope.Value;
        }
    }
}