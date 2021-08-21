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
using OChatApp;
using Microsoft.AspNetCore.Authorization;

namespace OChatApp.Controllers
{
    using static UsersRoutes;
    using static Services.UserResponses;

    [Route(USERS)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet(FRIENDS, Name = nameof(GetFriends))]
        public async Task<IActionResult> GetFriends(string userId)
        {
            var userFriends = await _userService.GetUserFriends(userId);

            return Ok(userFriends);
        }

        [HttpPost(REQUEST, Name = nameof(SendFriendRequest))]
        public async Task<IActionResult> SendFriendRequest(string userId, string targetUserId)
        {
            await _userService.SendFriendRequest(userId, targetUserId);

            return Ok();
        }

        [HttpPost(ACCEPT, Name = nameof(AcceptFriendRequest))]
        public async Task<IActionResult> AcceptFriendRequest(string userId, string requestId)
        {
            await _userService.AcceptFriendRequest(userId, requestId);

            return Ok();
        }

        [HttpPost(REJECT, Name = nameof(RejectFriendRequest))]
        public async Task<IActionResult> RejectFriendRequest(string userId, string requestId)
        {
            await _userService.RejectFriendRequest(userId, requestId);

            return Ok();
        }

        [HttpPost(REMOVE, Name = nameof(RemoveFriend))]
        public async Task<IActionResult> RemoveFriend(string userId, string friendId)
        {
            await _userService.RemoveFriend(userId, friendId);

            return Ok();
        }

        public async Task<IActionResult> GetPendingRequests(string userId)
        {
            var requests = await _userService.GetPendingRequests(userId);

            if (requests == null)
                return BadRequest(USER_HAS_NO_REQUESTS);

            return Ok(requests);
        }
    }
}
