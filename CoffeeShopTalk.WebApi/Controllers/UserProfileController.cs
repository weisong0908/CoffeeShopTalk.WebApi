using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoffeeShopTalk.WebApi.Models;
using CoffeeShopTalk.WebApi.Models.Requests;
using CoffeeShopTalk.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoffeeShopTalk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(IUserProfileService userProfileService, IConfiguration configuration, ILogger<UserProfileController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _userProfileService = userProfileService;
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromForm] ClientUserProfileUpdateRequest request)
        {
            var fileName = "";
            if (request.ProfilePicture != null)
            {
                var profilePicturesFolder = Path.Combine("wwwroot", "profilepictures");
                Directory.CreateDirectory(profilePicturesFolder);
                fileName = request.UserId.Replace('|', '-') + "-" + request.ProfilePicture.FileName;

                using (var stream = new FileStream(Path.Combine(profilePicturesFolder, fileName), FileMode.Create))
                    request.ProfilePicture.CopyTo(stream);

                _logger.LogInformation("Profile picture saved to file system: " + fileName);
            }

            var userIdFromAccessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            if (userIdFromAccessToken != request.UserId)
            {
                _logger.LogError($"User {userIdFromAccessToken} is updating info of user {request.UserId}");
                return BadRequest("Invalid user");
            }

            var serverRequest = new Auth0UserProfileUpdateRequest()
            {
                UserId = request.UserId,
                Name = request.Username,
            };

            if (request.ProfilePicture != null)
                serverRequest.Picture = Uri.EscapeUriString(_configuration.GetValue<string>("FileSystem:ProfilePicturesPath") + fileName);

            var result = await _userProfileService.Update(serverRequest);

            return Ok(result);
        }
    }
}