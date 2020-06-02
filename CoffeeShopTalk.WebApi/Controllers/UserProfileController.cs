using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
using CoffeeShopTalk.WebApi.Models.Requests;
using CoffeeShopTalk.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoffeeShopTalk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] ClientUserProfileUpdateRequest request)
        {
            var userIdFromAccessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            if (userIdFromAccessToken != request.UserId)
                return BadRequest("Invalid user");

            var result = await _userProfileService.Update(request);

            return Ok(result);
        }
    }
}