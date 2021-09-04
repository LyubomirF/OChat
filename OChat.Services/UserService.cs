using OChat.Domain;
using OChat.Infrastructure.Repositories.Interfaces;
using OChat.Services.Exceptions;
using OChat.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OChat.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
            => _userRepository = userRepository;

        public async Task<IEnumerable<User>> GetUserFriends(Guid userId)
        {
            var user = await _userRepository.GetUserWithFriendsAsync(userId);

            if (user.Friends == null || user.Friends.Count == 0)
                throw new EmptyCollectionException("User has no friends.");

            return user.Friends;
        }

        public async Task SendFriendRequest(Guid userId, Guid targetUserId)
        {
            var user = await _userRepository.GetEntityByIdAsync(userId);

            var targetUser = await _userRepository.GetUserWithFriendRequestsAsync(targetUserId);

            var newFriendRequest = new FriendRequest()
            {
                Status = FriendRequestStatus.Pending,
                From = user
            };

            targetUser.FriendRequests.Add(newFriendRequest);

            await _userRepository.SaveEntityAsync(targetUser);
        }

        public async Task AcceptFriendRequest(Guid userId, Guid requestId)
        {
            var user = await _userRepository.GetUserWithFriendsAndFriendRequestsAsync(userId);

            var request = user.FriendRequests?
                .SingleOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new FriendRequestException("Request not found.");

            var fromUser = await _userRepository.GetUserWithFriendsAsync(request.From.Id);

            if (request.Status != FriendRequestStatus.Pending)
                throw new FriendRequestException("In order to accept a friend request, it has to be pending first.");

            request.Status = FriendRequestStatus.Accepted;

            user.Friends.Add(fromUser);
            fromUser.Friends.Add(user);

            await _userRepository.SaveEntityAsync(user);
            await _userRepository.SaveEntityAsync(fromUser);
        }

        public async Task IgnoreFriendRequest(Guid userId, Guid requestId)
        {
            var user = await _userRepository.GetUserWithFriendRequestsAsync(userId);

            var request = user.FriendRequests
                .SingleOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new FriendRequestException("Request not found.");

            if (request.Status != FriendRequestStatus.Pending)
                throw new FriendRequestException("In order to ignore a friend request, it has to be pending first.");

            request.Status = FriendRequestStatus.Ignored;

            await _userRepository.SaveEntityAsync(user);
        }

        public async Task RemoveFriend(Guid userId, Guid targetUserId)
        {
            var user = await _userRepository.GetUserWithFriendsAsync(userId);

            var targetUser = await _userRepository.GetUserWithFriendsAsync(targetUserId);

            user.Friends.Remove(user.Friends.First(x => x.Id == targetUser.Id));
            targetUser.Friends.Remove(targetUser.Friends.First(x => x.Id == user.Id));

            await _userRepository.SaveEntityAsync(user);
            await _userRepository.SaveEntityAsync(targetUser);
        }

        public async Task<IEnumerable<FriendRequest>> GetPendingRequests(Guid userId)
        {
            var user = await _userRepository.GetUserWithPendingRequestsAsync(userId);

            if (user.FriendRequests == null || !user.FriendRequests.Any())
                throw new EmptyCollectionException("User does not have pending requests.");

            return user.FriendRequests;
        }
    }
}
