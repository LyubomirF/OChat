using Moq;
using OChat.Core.Common.Repositories;
using OChat.Domain;
using System;
using System.Collections.Generic;

namespace OChatApp.UnitTests.Mocks
{
    class ChatServiceMockSetup
    {
        //chat between richard, greg
        public (Mock<IChatRepository>, Mock<IUserRepository>) CreateMock_CreateChatRoom()
        {
            var chatRepository = new Mock<IChatRepository>();
            var userRepository = new Mock<IUserRepository>();

            userRepository.SetupSequence(x => x.GetEntityByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User())
                .ReturnsAsync(new User());

            chatRepository.Setup(x => x.SaveEntityAsync(It.IsAny<ChatRoom>()));
            userRepository.Setup(x => x.SaveEntityAsync(It.IsAny<User>()));

            return (chatRepository, userRepository);
        }

        public (Mock<IChatRepository>, Mock<IUserRepository>) CreateMock_GetChatRoomMessageHistory()
        {
            var chatRepository = new Mock<IChatRepository>();
            var userRepository = new Mock<IUserRepository>();

            chatRepository.Setup(x => x.GetChatRoomWithMessegesAsync(It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                .ReturnsAsync(new ChatRoom()
                {
                    Messages =
                    {
                        new Message()
                    }
                });

            return (chatRepository, userRepository);
        }

        public (Mock<IChatRepository>, Mock<IUserRepository>) CreateMock_GetChatRoomMessageHistory_ThrowsEmptyCollectionException()
        {
            var chatRepository = new Mock<IChatRepository>();
            var userRepository = new Mock<IUserRepository>();

            chatRepository.Setup(x => x.GetChatRoomWithMessegesAsync(It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                .ReturnsAsync(new ChatRoom());

            return (chatRepository, userRepository);
        }

        public (Mock<IChatRepository>, Mock<IUserRepository>) CreateMock_GetTimeLastMessageWasSeen()
        {
            var chatRepository = new Mock<IChatRepository>();
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(x => x.GetUserWithChatTrackers(It.IsAny<Guid>()))
                .ReturnsAsync(new User()
                {
                    ChatTrackers =
                    {
                        new ChatTracker()
                        {
                            LastReadMessageTimeStamp = DateTime.Today,
                            Chat = new ChatRoom() { Id = Guid.Parse("025e253c-4d18-405c-9848-4491ce35ec1f")}
                        }

                    }
                });

            return (chatRepository, userRepository);
        }

        public (Mock<IChatRepository>, Mock<IUserRepository>) CreateMock_GetTimeLastMessageWasSeen_ThrowsChatTrackerException()
        {
            var chatRepository = new Mock<IChatRepository>();
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(x => x.GetUserWithChatTrackers(It.IsAny<Guid>()))
                .ReturnsAsync(new User()
                {
                    ChatTrackers =
                    {
                        new ChatTracker()
                        {
                            Chat = new ChatRoom()
                            {
                                Id = Guid.NewGuid()
                            }
                        }
                    }
                });

            return (chatRepository, userRepository);
        }

        public (Mock<IChatRepository>, Mock<IUserRepository>) CreateMock_UpdateTimeLastMessageWasSeen()
        {
            var chatRepository = new Mock<IChatRepository>();
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(x => x.GetUserWithChatTrackers(It.IsAny<Guid>()))
                .ReturnsAsync(new User()
                {
                    ChatTrackers =
                    {
                        new ChatTracker()
                        {
                            LastReadMessageTimeStamp = DateTime.Today,
                            Chat = new ChatRoom() { Id = Guid.Parse("025e253c-4d18-405c-9848-4491ce35ec1f")}
                        }

                    }
                });

            userRepository.Setup(x => x.SaveEntityAsync(It.IsAny<User>()));

            return (chatRepository, userRepository);
        }

        public (Mock<IChatRepository>, Mock<IUserRepository>) CreateMock_GetChatRooms()
        {
            var chatRepository = new Mock<IChatRepository>();
            var userRepository = new Mock<IUserRepository>();

            chatRepository.Setup(x => x.GetChatsForUser(It.IsAny<Guid>()))
                .ReturnsAsync(new List<ChatRoom>() { new ChatRoom() });

            return (chatRepository, userRepository);
        }

        public (Mock<IChatRepository>, Mock<IUserRepository>) CreateMock_GetNewMessages()
        {
            var chatRepository = new Mock<IChatRepository>();
            var userRepository = new Mock<IUserRepository>();

            userRepository.Setup(x => x.GetUserWithChatTrackers(It.IsAny<Guid>()))
                .ReturnsAsync(new User()
                {
                    ChatTrackers =
                    {
                        new ChatTracker()
                        {
                            Chat = new ChatRoom()
                            {
                                Id = Guid.NewGuid()
                            },
                            LastReadMessageTimeStamp = DateTime.Now
                        },
                        new ChatTracker()
                        {
                            Chat = new ChatRoom()
                            {
                                Id = Guid.NewGuid()
                            },
                            LastReadMessageTimeStamp = DateTime.Now
                        }
                    }
                });

            chatRepository.SetupSequence(x => x.GetChatWithMessagesAfter(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new ChatRoom())
                .ReturnsAsync(new ChatRoom());

            return (chatRepository, userRepository);
        }
    }
}
