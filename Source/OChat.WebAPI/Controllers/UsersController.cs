using Microsoft.AspNetCore.Mvc;
using OChat.Core.OChat.Services.InputModels;
using OChat.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;
using OChat.WebAPI.Models;

namespace OChat.WebAPI.Controllers
{
    using static UsersRoutes;
    using static UserResponses;

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
        public async Task<IActionResult> SendFriendRequest(SendFriendRequest request)
        {
            await _userService.SendFriendRequest(new SendFriendRequestModel(request.UserId, request.TargetUserId));

            return Ok();
        }

        [HttpPost(ACCEPT, Name = nameof(AcceptFriendRequest))]
        public async Task<IActionResult> AcceptFriendRequest(AcceptFriendRequest request)
        {
            await _userService.AcceptFriendRequest(new AcceptFriendRequestModel(request.UserId, request.RequestId));

            return Ok();
        }

        [HttpPost(IGNORE, Name = nameof(IgnoreFriendRequest))]
        public async Task<IActionResult> IgnoreFriendRequest(IgnoreFriendRequest request)
        {
            await _userService.IgnoreFriendRequest(new IgnoreFriendRequestModel(request.UserId, request.RequestId));

            return Ok();
        }

        [HttpPost(REMOVE, Name = nameof(RemoveFriend))]
        public async Task<IActionResult> RemoveFriend(RemoveFriend request)
        {
            await _userService.RemoveFriend(new RemoveFriendModel(request.UserId, request.FriendId));

            return Ok();
        }

        [HttpGet(PENDING, Name = nameof(GetPendingRequests))]
        public async Task<IActionResult> GetPendingRequests(Guid userId)
        {
            var requests = await _userService.GetPendingRequests(userId);

            if (requests == null)
                return BadRequest(USER_HAS_NO_REQUESTS);

            return Ok(requests);
        }
    }
}
