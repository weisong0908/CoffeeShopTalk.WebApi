using System.Threading.Tasks;

namespace CoffeeShopTalk.WebApi.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeeShopTalkDbContext _dbContext;

        public UnitOfWork(CoffeeShopTalkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}