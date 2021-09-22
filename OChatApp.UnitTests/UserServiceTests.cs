using Moq;
using NUnit.Framework;
using OChat.Core.OChat.Services.InputModels;
using OChat.Core.Services;
using OChat.Core.Services.Exceptions;
using OChat.Domain;
using OChatApp.UnitTests.Mocks;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.UnitTests
{
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
        public async Task GetUserFriends_ReturnsCollectionOfFriends()
        {
            var userRepositoryMock = _mockSetup.CreateMock_GetUserFriends();
            var userService = new UserService(userRepositoryMock.Object);

            var friends = await userService.GetUserFriends(It.IsAny<Guid>());

            Assert.IsTrue(friends.Count() > 0);
        }

        [Test]
        public void GetUserFriends_ThrowsEmptyCollectionException()
        {
            var userRepositoryMock = _mockSetup.CreateMock_GetUserFriends_ThrowsEmptyCollectionException();
            var userService = new UserService(userRepositoryMock.Object);

            Assert.ThrowsAsync<EmptyCollectionException>(() => userService.GetUserFriends(It.IsAny<Guid>()), "User has no friends.");
        }

        [Test]
        public async Task SendFriendRequest_ValidCall()
        {
            var userRepositoryMock = _mockSetup.CreateMock_SendFriendRequest();
            var userService = new UserService(userRepositoryMock.Object);
            var inputModel = new SendFriendRequestModel(It.IsAny<Guid>(), It.IsAny<Guid>());

            await userService.SendFriendRequest(inputModel);

            userRepositoryMock.Verify(x => x.SaveEntityAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public async Task AcceptFriendRequest_ValidCall()
        {
            
            var userRepositoryMock = _mockSetup.CreateMock_AcceptFriendRequest();
            var userService = new UserService(userRepositoryMock.Object);
            var inputModel = new AcceptFriendRequestModel(It.IsAny<Guid>(), Guid.Parse("5b1725f3-0aa3-4bbc-b9c9-df32ffcd6624"));

            await userService.AcceptFriendRequest(inputModel);

            userRepositoryMock.Verify(x => x.SaveEntityAsync(It.IsAny<User>()), Times.Exactly(2));
        }

        [Test]
        public void AcceptFriendRequest_InvalidRequest_ThrowsFriendRequestException_RequestNotFound()
        {
            var userRepositoryMock = _mockSetup.CreateMock_AcceptFriendRequest_ThrowsRequestNotFound();
            var userService = new UserService(userRepositoryMock.Object);
            var inputModel = new AcceptFriendRequestModel(It.IsAny<Guid>(), It.IsAny<Guid>());

            Assert.ThrowsAsync<FriendRequestException>(() => userService.AcceptFriendRequest(inputModel), "Request not found.");
        }

        [Test]
        public void AcceptFriendRequest_ThrowsFriendRequestException_InvalidRequest()
        {
            var userRepositoryMock = _mockSetup.CreateMock_AcceptFriendRequest_ThrowsInvalidRequest();
            var userService = new UserService(userRepositoryMock.Object);
            var inputModel = new AcceptFriendRequestModel(It.IsAny<Guid>(), It.IsAny<Guid>());

            Assert.ThrowsAsync<FriendRequestException>(() =>
                userService.AcceptFriendRequest(inputModel),
                    "In order to reject a friend request, it has to be pending first.");
        }

        [Test]
        public async Task IgnoreFriendRequest_ValidCall()
        {
            var userRepositoryMock = _mockSetup.CreateMock_IgnoreFriendRequest();
            var userService = new UserService(userRepositoryMock.Object);
            var inputModel = new IgnoreFriendRequestModel(It.IsAny<Guid>(), Guid.Parse("6a94057f-37ac-4ab9-891f-90cd319a5975"));

            await userService.IgnoreFriendRequest(inputModel);

            userRepositoryMock.Verify(x => x.SaveEntityAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void IgnoreFriendRequest_ThrowsFriendRequestException_RequestNotFound()
        {
            var userRepositoryMock = _mockSetup.CreateMock_IgnoreFriendRequest_ThrowsFriendRequestException_RequestNotFound();
            var userService = new UserService(userRepositoryMock.Object);
            var inputModel = new IgnoreFriendRequestModel(It.IsAny<Guid>(), Guid.Parse("6a94057f-37ac-4ab9-891f-90cd319a5975"));

            Assert.ThrowsAsync<FriendRequestException>(() => userService.IgnoreFriendRequest(inputModel), "Request not found.");
        }

        [Test]
        public void IgnoreFriendRequest_ThrowsFriendRequestException_InvalidRequest()
        {
            var userRepositoryMock = _mockSetup.CreateMock_IgnoreFriendRequest_ThrowsFriendRequestException_InvalidRequest();
            var userService = new UserService(userRepositoryMock.Object);
            var inputModel = new IgnoreFriendRequestModel(It.IsAny<Guid>(), Guid.Parse("6a94057f-37ac-4ab9-891f-90cd319a5975"));

            Assert.ThrowsAsync<FriendRequestException>(
                () => userService.IgnoreFriendRequest(inputModel), 
                "In order to ignore a friend request, it has to be pending first.");
        }

        [Test]
        public async Task RemoveFriend_ValidCall()
        {
            var userRepositoryMock = _mockSetup.CreateMock_RemoveFriend();
            var userService = new UserService(userRepositoryMock.Object);
            var inputModel = new RemoveFriendModel(Guid.Parse("d23e4848-68b6-4af6-9b2d-848a462c5ae7"), Guid.Parse("fefa61db-1dfe-4d10-9be2-37dd6db78998"));

            await userService.RemoveFriend(inputModel);

            userRepositoryMock.Verify(x => x.SaveEntityAsync(It.IsAny<User>()), Times.Exactly(2));
        }

        [Test]
        public async Task GetPendingRequests_ValidCall()
        {  
            var userRepositoryMock = _mockSetup.CreateMock_GetPendingRequests();
            var userService = new UserService(userRepositoryMock.Object);

            var requests = await userService.GetPendingRequests(It.IsAny<Guid>());

            Assert.IsTrue(requests.Count() > 0);
        }
    }
}
