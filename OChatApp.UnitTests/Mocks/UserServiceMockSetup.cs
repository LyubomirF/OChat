using Moq;
using OChat.Infrastructure.Exceptions;
using OChat.Infrastructure.Repositories.Interfaces;
using OChatApp.UnitTests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OChatApp.UnitTests.Mocks
{
    using static OChat.Infrastructure.Exceptions.ExceptionMessages;

    class UserServiceMockSetup
    {
        public Mock<IUserRepository> CreateMock_GetUserFriends(Guid userId)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository
                .Setup(x => x.GetUserWithFriendsAsync(userId))
                .ReturnsAsync(() =>
                {
                    var user = Database.Users.SingleOrDefault(x => x.Id == userId);

                    if (user == null)
                        throw new NotFoundException(USER_NOT_FOUND);

                    return user;
                });

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_SendFriendRequest(Guid userId, Guid targetUserId)
        {
            var user = Database.Users.SingleOrDefault(x => x.Id == userId);
            var targetUser = Database.Users.SingleOrDefault(x => x.Id == targetUserId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetEntityByIdAsync(userId))
                .ReturnsAsync(user);

            userRepository
                .Setup(x => x.GetUserWithFriendRequestsAsync(targetUserId))
                .ReturnsAsync(targetUser);

            userRepository.Setup(x => x.SaveEntityAsync(targetUser));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_AcceptFriendRequest(Guid userId, Guid fromUserId)
        {
            var fromUser = Database.Users.SingleOrDefault(x => x.Id == fromUserId);
            var user = Database.Users.SingleOrDefault(x => x.Id == userId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendsAndFriendRequestsAsync(userId))
                .ReturnsAsync(user);

            userRepository
                .Setup(x => x.GetUserWithFriendsAsync(fromUserId))
                .ReturnsAsync(fromUser);

            userRepository.Setup(x => x.SaveEntityAsync(user));
            userRepository.Setup(x => x.SaveEntityAsync(fromUser));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_IgnoreFriendRequest(Guid userId)
        {
            var user = Database.Users.SingleOrDefault(x => x.Id == userId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendRequestsAsync(userId))
                .ReturnsAsync(user);

            userRepository.Setup(x => x.SaveEntityAsync(user));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_RemoveFriend(Guid userId, Guid targetUserId)
        {
            var user = Database.Users.SingleOrDefault(x => x.Id == userId);
            var targetUser = Database.Users.SingleOrDefault(x => x.Id == targetUserId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendsAsync(userId))
                .ReturnsAsync(user);

            userRepository
                .Setup(x => x.GetUserWithFriendsAsync(targetUserId))
                .ReturnsAsync(targetUser);

            userRepository.Setup(x => x.SaveEntityAsync(user));
            userRepository.Setup(x => x.SaveEntityAsync(targetUser));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_GetPendingRequests(Guid userId)
        {
            var user = Database.Users.SingleOrDefault(x => x.Id == userId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithPendingRequestsAsync(userId))
                .ReturnsAsync(user);

            return userRepository;
        }

    }
}
