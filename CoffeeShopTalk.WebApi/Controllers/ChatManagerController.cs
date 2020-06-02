using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopTalk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("read:chat_manager")]
    public class ChatManagerController : ControllerBase
    {
        private readonly IConnectedUserRepository _connectedUserRepository;

        public ChatManagerController(IConnectedUserRepository connectedUserRepository)
        {
            _connectedUserRepository = connectedUserRepository;
        }

        [HttpGet("connectedUsers")]
        public async Task<IActionResult> GetConnectedUsers()
        {
            var users = await _connectedUserRepository.GetConnectedUsers();

            return Ok(users);
        }
    }
}