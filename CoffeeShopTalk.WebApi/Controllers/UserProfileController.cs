using System.Linq;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopTalk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] UserProfileUpdateRequest request)
        {
            var userIdFromAccessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            if (userIdFromAccessToken != request.UserId)
                return BadRequest("Invalid user");

            //TODO: update AUth0 profile using Management API token

            return Ok();
        }
    }
}