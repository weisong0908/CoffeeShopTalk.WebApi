using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoffeeShopTalk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("read:chat_manager")]
    public class ChatManagerController : ControllerBase
    {
        private readonly IConnectedUserRepository _connectedUserRepository;
        private readonly ILogger<ChatManagerController> _logger;

        public ChatManagerController(IConnectedUserRepository connectedUserRepository, ILogger<ChatManagerController> logger)
        {
            _logger = logger;
            _connectedUserRepository = connectedUserRepository;
        }

        [HttpGet("connectedUsers")]
        public async Task<IActionResult> GetConnectedUsers()
        {
            var users = await _connectedUserRepository.GetUsers();

            return Ok(users);
        }
    }
}