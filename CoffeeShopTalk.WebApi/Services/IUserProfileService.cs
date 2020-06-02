using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models.Requests;

namespace CoffeeShopTalk.WebApi.Services
{
    public interface IUserProfileService
    {
        Task<string> Update(ClientUserProfileUpdateRequest request);
    }
}