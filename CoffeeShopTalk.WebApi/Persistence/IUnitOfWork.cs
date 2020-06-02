using System.Threading.Tasks;

namespace CoffeeShopTalk.WebApi.Persistence
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}