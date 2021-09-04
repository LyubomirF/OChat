using Moq;
using OChatApp.Areas.Identity.Data;
using OChatApp.Repositories;
using OChatApp.Repositories.Exceptions;
using OChatApp.UnitTests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OChatApp.UnitTests.Mocks
{
    using static OChatApp.Services.Exceptions.ExceptionMessages;

    class UserServiceMockSetup
    {
        private readonly Database _db;

        public UserServiceMockSetup()
            => _db = new Database();

        public Mock<IUserRepository> CreateMock_GetUserFriends(string userId)
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository
                .Setup(x => x.GetUserWithFriendsAsync(userId, USER_NOT_FOUND))
                .ReturnsAsync(() =>
                {
                    var user = _db.Users.SingleOrDefault(x => x.Id == userId);

                    if (user == null)
                        throw new NotFoundException(USER_NOT_FOUND);

                    return user;
                });

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_SendFriendRequest(string userId, string targetUserId)
        {
            var user = _db.Users.SingleOrDefault(x => x.Id == userId);
            var targetUser = _db.Users.SingleOrDefault(x => x.Id == targetUserId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetByIdAsync(userId, USER_NOT_FOUND))
                .ReturnsAsync(user);

            userRepository
                .Setup(x => x.GetUserWithFriendRequestsAsync(targetUserId, TARGET_NOT_FOUND))
                .ReturnsAsync(targetUser);

            userRepository.Setup(x => x.Update(targetUser));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_AcceptFriendRequest(string userId, string fromUserId)
        {
            var fromUser = _db.Users.SingleOrDefault(x => x.Id == fromUserId);
            var user = _db.Users.SingleOrDefault(x => x.Id == userId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendsAndFriendRequestsAsync(userId, USER_NOT_FOUND))
                .ReturnsAsync(user);

            userRepository
                .Setup(x => x.GetUserWithFriendsAsync(fromUserId, SENDER_NOT_FOUND))
                .ReturnsAsync(fromUser);

            userRepository.Setup(x => x.Update(user));
            userRepository.Setup(x => x.Update(fromUser));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_RejectFriendRequest(string userId)
        {
            var user = _db.Users.SingleOrDefault(x => x.Id == userId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendRequestsAsync(userId, USER_NOT_FOUND))
                .ReturnsAsync(user);

            userRepository.Setup(x => x.Update(user));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_RemoveFriend(string userId, string targetUserId)
        {
            var user = _db.Users.SingleOrDefault(x => x.Id == userId);
            var targetUser = _db.Users.SingleOrDefault(x => x.Id == targetUserId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendsAsync(userId, USER_NOT_FOUND))
                .ReturnsAsync(user);

            userRepository
                .Setup(x => x.GetUserWithFriendsAsync(targetUserId, TARGET_NOT_FOUND))
                .ReturnsAsync(targetUser);

            userRepository.Setup(x => x.Update(user));
            userRepository.Setup(x => x.Update(targetUser));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_GetPendingRequests(string userId)
        {
            var user = _db.Users.SingleOrDefault(x => x.Id == userId);

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithPendingRequestsAsync(userId, USER_NOT_FOUND))
                .ReturnsAsync(user);

            return userRepository;
        }

    }
}
