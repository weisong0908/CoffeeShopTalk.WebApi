using CoffeeShopTalk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopTalk.WebApi.Persistence
{
    public class CoffeeShopTalkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public CoffeeShopTalkDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}