using Moq;
using OChat.Core.Common.Repositories;
using OChat.Domain;
using System;

namespace OChatApp.UnitTests.Mocks
{
    using static OChat.Infrastructure.Exceptions.ExceptionMessages;

    class UserServiceMockSetup
    {
        public Mock<IUserRepository> CreateMock_GetUserFriends()
        {
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(x => x.GetUserWithFriendsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User() { Friends = { new User(), new User() } });

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_GetUserFriends_ThrowsEmptyCollectionException()
        {
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(x => x.GetUserWithFriendsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User());

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_SendFriendRequest()
        {
            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetEntityByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User());

            userRepository
                .Setup(x => x.GetUserWithFriendRequestsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User() { FriendRequests = { new FriendRequest(), new FriendRequest() } });

            userRepository.Setup(x => x.SaveEntityAsync(It.IsAny<User>()));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_AcceptFriendRequest()
        {
            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendsAndFriendRequestsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User() 
                { 
                    FriendRequests = 
                    { 
                        new FriendRequest() 
                        { 
                            Id = Guid.Parse("5b1725f3-0aa3-4bbc-b9c9-df32ffcd6624"), 
                            From = new User()
                            {
                                Id = Guid.Parse("916f27f6-8ee7-495e-a284-009929ac47b1")
                            },
                            Status = FriendRequestStatus.Pending
                            
                        } 
                    } 
                });


            userRepository
                .Setup(x => x.GetUserWithFriendsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User());

            userRepository.Setup(x => x.SaveEntityAsync(It.IsAny<User>()));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_AcceptFriendRequest_ThrowsRequestNotFound()
        {
            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendsAndFriendRequestsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User()
                {
                    FriendRequests =
                    {
                        new FriendRequest()
                        {
                            Id = Guid.NewGuid(),
                        }
                    }
                });

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_AcceptFriendRequest_ThrowsInvalidRequest()
        {
            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendsAndFriendRequestsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User()
                {
                    FriendRequests =
                    {
                        new FriendRequest()
                        {
                            Id = Guid.NewGuid(),
                            Status = FriendRequestStatus.Accepted
                        }
                    }
                });

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_IgnoreFriendRequest()
        {

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendRequestsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User() { FriendRequests = { new FriendRequest() { Id = Guid.Parse("6a94057f-37ac-4ab9-891f-90cd319a5975") } } });

            userRepository.Setup(x => x.SaveEntityAsync(It.IsAny<User>()));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_IgnoreFriendRequest_ThrowsFriendRequestException_RequestNotFound()
        {

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendRequestsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User() { FriendRequests = { new FriendRequest() { Id = Guid.NewGuid() } } });

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_IgnoreFriendRequest_ThrowsFriendRequestException_InvalidRequest()
        {

            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithFriendRequestsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User() { FriendRequests = { new FriendRequest() { Id = Guid.Parse("6a94057f-37ac-4ab9-891f-90cd319a5975"), Status = FriendRequestStatus.Accepted } } });

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_RemoveFriend()
        {
            var userRepository = new Mock<IUserRepository>();

            userRepository
                .SetupSequence(x => x.GetUserWithFriendsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User() {
                    Id = Guid.Parse("d23e4848-68b6-4af6-9b2d-848a462c5ae7"),
                    Friends = { new User() { Id = Guid.Parse("fefa61db-1dfe-4d10-9be2-37dd6db78998") } }
                })
                .ReturnsAsync(new User()
                {
                    Id = Guid.Parse("fefa61db-1dfe-4d10-9be2-37dd6db78998"),
                    Friends = { new User() { Id = Guid.Parse("d23e4848-68b6-4af6-9b2d-848a462c5ae7") } }
                }); ;

            userRepository.Setup(x => x.SaveEntityAsync(It.IsAny<User>()));

            return userRepository;
        }

        public Mock<IUserRepository> CreateMock_GetPendingRequests()
        {
            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(x => x.GetUserWithPendingRequestsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User() { 
                    FriendRequests = { 
                        new FriendRequest() { 
                            Status = FriendRequestStatus.Pending 
                        },
                        new FriendRequest() {
                            Status = FriendRequestStatus.Pending
                        }}
                });

            return userRepository;
        }
    }
}
