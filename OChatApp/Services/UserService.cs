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
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<OChatAppUser>> GetUserFriends(string userId)
        {
            var user = await _userRepository.GetUserWithFriendsAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            if (user.Friends == null || user.Friends.Count == 0)
                throw new EmptyCollectionException("User has no friends.");

            return user.Friends;
        }

        public async Task SendFriendRequest(string userId, string targetUserId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var targetUser = await _userRepository.GetUserWithFriendRequestsAsync(targetUserId);

            if (targetUserId == null)
                throw new NotFoundException("User not found.");

            var newFriendRequest = new FriendRequest()
            {
                Status = RequestStatus.Pending,
                FromUser = user
            };

            targetUser.FriendRequests.Add(newFriendRequest);

            await _userRepository.Update(targetUser);
        }

        public async Task AcceptFriendRequest(string userId, string requestId)
        {
            var user = await _userRepository.GetUserWithFriendsAndFriendRequestsAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var request = user.FriendRequests
                .Single(r => r.Id == requestId);

            if (request == null)
                throw new NotFoundException("Request not found.");

            var fromUser = await _userRepository.GetUserWithFriendsAsync(request.FromUser.Id);

            if (fromUser == null)
                throw new NotFoundException("Sender was not found.");

            request.Status = RequestStatus.Accepted;

            user.Friends.Add(fromUser);
            fromUser.Friends.Add(user);

            await _userRepository.Update(user);
            await _userRepository.Update(fromUser);
        }

        public async Task RejectFriendRequest(string userId ,string requestId)
        {
            var user = await _userRepository.GetUserWithFriendRequestsAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var request = user.FriendRequests
                .Single(r => r.Id == requestId);

            if (request == null)
                throw new NotFoundException("Request not found.");

            request.Status = RequestStatus.Rejected;

            await _userRepository.Update(user);
        }

        public async Task RemoveFriend(string userId, string targetUserId)
        {
            var user = await _userRepository.GetUserWithFriendsAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var targetUser = await _userRepository.GetUserWithFriendsAsync(targetUserId);

            if (targetUser == null)
                throw new NotFoundException("Target user not found.");

            user.Friends.Remove(targetUser);
            targetUser.Friends.Remove(user);

            await _userRepository.Update(user);
            await _userRepository.Update(targetUser);
        }

        public async Task<IEnumerable<FriendRequest>> GetPendingRequests(string userId)
        {
            var requests = await _userRepository.GetPendingRequestsAsync(userId);

            if (requests == null || !requests.Any())
                throw new EmptyCollectionException("User does not have pending requests.");

            return requests;
        }

    }
}
