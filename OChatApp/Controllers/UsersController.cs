using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OChatApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OChatApp.Areas.Identity.Data;
using OChatApp.Services;

namespace OChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFriends(string userId)
        {
            var userFriends = await _userService.GetUserFriends(userId); 

            return Ok(userFriends);
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(string userId, string targetUserId)
        {
            await _userService.SendFriendRequest(userId, targetUserId);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendRequest(string userId, string requestId)
        {
            await _userService.AcceptFriendRequest(userId, requestId);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RejectFriendRequest(string userId, string requestId)
        {
            await _userService.RejectFriendRequest(userId, requestId);

            return Ok();
        }
        
        public async Task<IActionResult> RemoveFriend(string userId, string targetUserId)
        {
            await _userService.RemoveFriend(userId, targetUserId);

            return Ok();
        }

        public async Task<IActionResult> GetPendingRequests(string userId)
        {
            var requests = await _userService.GetPendingRequests(userId);

            if (requests == null)
                return BadRequest("User has no friend requests.");

            return Ok(requests);
        }

    }
}
