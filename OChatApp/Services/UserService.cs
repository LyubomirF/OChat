using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OChatApp.Services.Exceptions;
using OChatApp.Repositories;

namespace OChatApp.Services
{
    using static ExceptionMessages;

    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<OChatAppUser>> GetUserFriends(string userId)
        {
            var user = await _userRepository.GetUserWithFriendsAsync(userId, USER_NOT_FOUND);

            if (user.Friends == null || user.Friends.Count == 0)
                throw new EmptyCollectionException("User has no friends.");

            return user.Friends;
        }

        public async Task SendFriendRequest(string userId, string targetUserId)
        {
            var user = await _userRepository.GetByIdAsync(userId, USER_NOT_FOUND);

            var targetUser = await _userRepository.GetUserWithFriendRequestsAsync(targetUserId, TARGET_NOT_FOUND);

            var newFriendRequest = new FriendRequest()
            {
                Status = RequestStatus.Pending,
                FromUser = user
            };

            if (targetUser.FriendRequests == null)
                targetUser.FriendRequests = new List<FriendRequest>();

            targetUser.FriendRequests.Add(newFriendRequest);
            
            await _userRepository.Update(targetUser);
        }

        public async Task AcceptFriendRequest(string userId, string requestId)
        {
            var user = await _userRepository.GetUserWithFriendsAndFriendRequestsAsync(userId, USER_NOT_FOUND);

            var request = user.FriendRequests?
                .SingleOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new FriendRequestException("Request not found.");

            var fromUser = await _userRepository.GetUserWithFriendsAsync(request.FromUser.Id, SENDER_NOT_FOUND);

            if (request.Status != RequestStatus.Pending)
                throw new FriendRequestException("In order to accept a friend request, it has to be pending first.");

            request.Status = RequestStatus.Accepted;

            user.Friends.Add(fromUser);
            fromUser.Friends.Add(user);

            await _userRepository.Update(user);
            await _userRepository.Update(fromUser);
        }

        public async Task RejectFriendRequest(string userId ,string requestId)
        {
            var user = await _userRepository.GetUserWithFriendRequestsAsync(userId, USER_NOT_FOUND);

            var request = user.FriendRequests?
                .SingleOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new FriendRequestException("Request not found.");

            if (request.Status != RequestStatus.Pending)
                throw new FriendRequestException("In order to reject a friend request, it has to be pending first.");

            request.Status = RequestStatus.Rejected;

            await _userRepository.Update(user);
        }

        public async Task RemoveFriend(string userId, string targetUserId)
        {
            var user = await _userRepository.GetUserWithFriendsAsync(userId, USER_NOT_FOUND);

            var targetUser = await _userRepository.GetUserWithFriendsAsync(targetUserId, TARGET_NOT_FOUND);

            user.Friends.Remove(user.Friends.First(x => x.Id == targetUser.Id));
            targetUser.Friends.Remove(targetUser.Friends.First(x => x.Id == user.Id));

            await _userRepository.Update(user);
            await _userRepository.Update(targetUser);
        }

        public async Task<IEnumerable<FriendRequest>> GetPendingRequests(string userId)
        {
            var user = await _userRepository.GetUserWithPendingRequestsAsync(userId, USER_NOT_FOUND);

            if (user.FriendRequests == null || !user.FriendRequests.Any())
                throw new EmptyCollectionException("User does not have pending requests.");

            return user.FriendRequests;
        }

    }
}
