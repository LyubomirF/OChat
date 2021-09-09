using Moq;
using NUnit.Framework;
using OChat.Domain;
using OChat.Infrastructure.Exceptions;
using OChat.Services;
using OChat.Services.Exceptions;
using OChatApp.UnitTests.Helper;
using OChatApp.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OChatApp.UnitTests
{
    using static ExceptionMessages;

    [TestFixture]
    class UserServiceTests
    {
        private UserServiceMockSetup _mockSetup;

        [SetUp]
        public void SetUp()
        {
            _mockSetup = new UserServiceMockSetup();
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_GetUserFriends_ReturnsCollectionOfFriends))]
        public async Task GetUserFriends_ReturnsCollectionOfFriends(Guid userId)
        {
            //Arrange
            var userRepositoryMock = _mockSetup.CreateMock_GetUserFriends(userId);
            var userService = new UserService(userRepositoryMock.Object);
            var actual = Database.Users.SingleOrDefault(x => x.Id == userId).Friends;

            //Act
            var friends = await userService.GetUserFriends(userId);

            //Assert
            var list = actual
                .Zip(friends, 
                (user1, user2) =>
                user1.Username == user2.Username &&
                user1.Id == user2.Id);

            foreach (var item in list)
                Assert.IsTrue(item);
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_GetUserFriends_ThrowsNotFoundException))]
        public void GetUserFriends_ThrowsNotFoundException(Guid userId)
        {
            //Arrage
            var userRepositoryMock = _mockSetup.CreateMock_GetUserFriends(userId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act/Assert
            Assert.ThrowsAsync<NotFoundException>(() => userService.GetUserFriends(userId));
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_GetUserFriends_ThrowsEmptyCollectionException))]
        public void GetUserFriends_ThrowsEmptyCollectionException(Guid userId)
        {
            //Arrage
            var userRepositoryMock = _mockSetup.CreateMock_GetUserFriends(userId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act/Assert
            Assert.ThrowsAsync<EmptyCollectionException>(() => userService.GetUserFriends(userId), "User has no friends.");
        }


        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_SendFriendRequest_ValidCall))]
        public async Task SendFriendRequest_ValidCall(Guid userId, Guid targetUserId)
        {
            //Arrange
            var userRepositoryMock = _mockSetup.CreateMock_SendFriendRequest(userId, targetUserId);
            var targetUser = Database.Users.SingleOrDefault(x => x.Id == targetUserId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act
            await userService.SendFriendRequest(userId, targetUserId);

            //Assert
            userRepositoryMock.Verify(x => x.SaveEntityAsync(targetUser), Times.Once);
            Assert.That(targetUser.FriendRequests.Count > 0);
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_AcceptFriendRequest_ValidCall))]
        public async Task AcceptFriendRequest_ValidCall(Guid userId, Guid requestId, Guid fromUserId)
        {
            //Arrange
            var userRepositoryMock = _mockSetup.CreateMock_AcceptFriendRequest(userId, fromUserId);
            var user = Database.Users.SingleOrDefault(x => x.Id == userId);
            var fromUser = Database.Users.SingleOrDefault(x => x.Id == fromUserId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act
            await userService.AcceptFriendRequest(userId, requestId);

            //Assert
            userRepositoryMock.Verify(x => x.SaveEntityAsync(user), Times.Once);
            userRepositoryMock.Verify(x => x.SaveEntityAsync(fromUser), Times.Once);
            Assert.IsTrue(user.Friends.Any(x => x.Username == fromUser.Username));
            Assert.IsTrue(fromUser.Friends.Any(x => x.Username == user.Username));
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetIntputFor_AcceptFriendRequest_InvalidRequest_ThrowsNotFoundException))]
        public void AcceptFriendRequest_InvalidRequest_ThrowsNotFoundException(Guid userId, Guid requestId, Guid fromUserId)
        {
            //Arrage
            var userRepositoryMock = _mockSetup.CreateMock_AcceptFriendRequest(userId, fromUserId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act/Assert
            Assert.ThrowsAsync<FriendRequestException>(() => userService.AcceptFriendRequest(userId, requestId), "Request not found.");
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_AcceptFriendRequest_InvalidRequest_ThrowsFriendRequestException))]
        public void AcceptFriendRequest_InvalidRequest_ThrowsFriendRequestException(Guid userId, Guid requestId, Guid fromUserId)
        {
            //Arrage
            var userRepositoryMock = _mockSetup.CreateMock_AcceptFriendRequest(userId, fromUserId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act/Assert
            Assert.ThrowsAsync<FriendRequestException>(() => userService.AcceptFriendRequest(userId, requestId), "In order to reject a friend request, it has to be pending first.");
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_IgnoreFriendRequest_ValidCall))]
        public async Task IgnoreFriendRequest_ValidCall(Guid userId, Guid requestId)
        {
            //Arrange
            var userRepositoryMock = _mockSetup.CreateMock_IgnoreFriendRequest(userId);
            var user = Database.Users.SingleOrDefault(x => x.Id == userId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act
            await userService.IgnoreFriendRequest(userId, requestId);

            //Assert
            userRepositoryMock.Verify(x => x.SaveEntityAsync(user), Times.Once);
            Assert.IsTrue(user.FriendRequests.SingleOrDefault(x => x.Id == requestId).Status == FriendRequestStatus.Ignored);
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_IgnoreFriendRequest_InvalidRequest_ThrowsNotFoundException))]
        public void IgnoreFriendRequest_InvalidRequest_ThrowsNotFoundException(Guid userId, Guid requestId)
        {
            //Arrage
            var userRepositoryMock = _mockSetup.CreateMock_IgnoreFriendRequest(userId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act/Assert
            Assert.ThrowsAsync<FriendRequestException>(() => userService.IgnoreFriendRequest(userId, requestId), "Request not found.");
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_IgnoreFriendRequest_InvalidRequest_ThrowsFriendRequestException))]
        public void IgnoreFriendRequest_InvalidRequest_ThrowsFriendRequestException(Guid userId, Guid requestId)
        {
            //Arrage
            var userRepositoryMock = _mockSetup.CreateMock_IgnoreFriendRequest(userId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act/Assert
            Assert.ThrowsAsync<FriendRequestException>(() => userService.IgnoreFriendRequest(userId, requestId), "In order to ignore a friend request, it has to be pending first.");
        }


        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_RemoveFriend_ValidCall))]
        public async Task RemoveFriend_ValidCall(Guid userId, Guid targetUserId)
        {
            //Arrage
            var userRepositoryMock = _mockSetup.CreateMock_RemoveFriend(userId, targetUserId);
            var user = Database.Users.SingleOrDefault(x => x.Id == userId);
            var targetUser = Database.Users.SingleOrDefault(x => x.Id == targetUserId);
            var userService = new UserService(userRepositoryMock.Object);

            //Act
            await userService.RemoveFriend(userId, targetUserId);

            //Assert
            userRepositoryMock.Verify(x => x.SaveEntityAsync(user), Times.Once);
            userRepositoryMock.Verify(x => x.SaveEntityAsync(targetUser), Times.Once);
            Assert.IsTrue(user.Friends.Count == 1);
            Assert.IsTrue(targetUser.Friends.Count == 1);
        }

        [Test]
        [TestCaseSource(typeof(TestInputs), nameof(TestInputs.GetInputFor_GetPendingRequests_ValidCall))]
        public async Task GetPendingRequests_ValidCall(Guid userId)
        {
            //Arrange
            var userRepositoryMock = _mockSetup.CreateMock_GetPendingRequests(userId);
            var userService = new UserService(userRepositoryMock.Object);
            var user = Database.Users.SingleOrDefault(x => x.Id == userId);

            //Act
            var requests = await userService.GetPendingRequests(userId);

            //Assert
            var list = requests.Zip(
                 user.FriendRequests,
                 (request1, request2) =>
                 request1.Id == request2.Id &&
                 request1.Status == request2.Status);

            foreach (var item in list)
                Assert.IsTrue(item);
        }
    }
}
