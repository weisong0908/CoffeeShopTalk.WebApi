using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopTalk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatManagerController : ControllerBase
    {
        private readonly CoffeeShopTalkDbContext _dbContext;

        public ChatManagerController(CoffeeShopTalkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("connectedUsers")]
        public async Task<IActionResult> GetConnectedUsers()
        {
            var users = await _dbContext.Users.Include(user => user.Connection).ToListAsync();

            return Ok(users);
        }
    }
}