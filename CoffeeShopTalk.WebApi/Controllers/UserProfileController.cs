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

namespace CoffeeShopTalk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IConfiguration _configuration;

        public UserProfileController(IUserProfileService userProfileService, IConfiguration configuration)
        {
            _configuration = configuration;
            _userProfileService = userProfileService;
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromForm] ClientUserProfileUpdateRequest request)
        {
            var profilePicturesFolder = Path.Combine("wwwroot", "profilepictures");
            Directory.CreateDirectory(profilePicturesFolder);
            var fileName = request.UserId.Replace('|', '-') + Path.GetExtension(request.ProfilePicture.FileName);

            using (var stream = new FileStream(Path.Combine(profilePicturesFolder, fileName), FileMode.Create))
                request.ProfilePicture.CopyTo(stream);

            var userIdFromAccessToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            if (userIdFromAccessToken != request.UserId)
                return BadRequest("Invalid user");

            var serverRequest = new Auth0UserProfileUpdateRequest()
            {
                UserId = request.UserId,
                Name = request.Username,
                Picture = Uri.EscapeUriString(_configuration.GetValue<string>("FileSystem:ProfilePicturesPath") + fileName)
            };

            var result = await _userProfileService.Update(serverRequest);

            return Ok(result);
        }
    }
}