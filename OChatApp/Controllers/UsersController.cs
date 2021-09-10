using Microsoft.AspNetCore.Mvc;
using OChat.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OChatApp.Controllers
{
    using static UsersRoutes;
    using static Services.UserResponses;

    [Route(USERS)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(FRIENDS, Name = nameof(GetFriends))]
        public async Task<IActionResult> GetFriends(Guid userId)
        {
            var userFriends = await _userService.GetUserFriends(userId);

            return Ok(userFriends);
        }

        [HttpPost(REQUEST, Name = nameof(SendFriendRequest))]
        public async Task<IActionResult> SendFriendRequest(Guid userId, Guid targetUserId)
        {
            await _userService.SendFriendRequest(userId, targetUserId);

            return Ok();
        }

        [HttpPost(ACCEPT, Name = nameof(AcceptFriendRequest))]
        public async Task<IActionResult> AcceptFriendRequest(Guid userId, Guid requestId)
        {
            await _userService.AcceptFriendRequest(userId, requestId);

            return Ok();
        }

        [HttpPost(REJECT, Name = nameof(IgnoreFriendRequest))]
        public async Task<IActionResult> IgnoreFriendRequest(Guid userId, Guid requestId)
        {
            await _userService.IgnoreFriendRequest(userId, requestId);

            return Ok();
        }

        [HttpPost(REMOVE, Name = nameof(RemoveFriend))]
        public async Task<IActionResult> RemoveFriend(Guid userId, Guid friendId)
        {
            await _userService.RemoveFriend(userId, friendId);

            return Ok();
        }

        public async Task<IActionResult> GetPendingRequests(Guid userId)
        {
            var requests = await _userService.GetPendingRequests(userId);

            if (requests == null)
                return BadRequest(USER_HAS_NO_REQUESTS);

            return Ok(requests);
        }
    }
}
