using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using OChat.Core.Common.Repositories;
using OChatApp.UnitTests.Mocks;
using OChat.Core.Services;
using OChat.Core.OChat.Services.InputModels;
using OChat.Domain;
using OChat.Core.Services.Exceptions;

namespace OChatApp.UnitTests
{

    [TestFixture]
    class ChatServiceTests
    {
        private ChatServiceMockSetup _mockSetup;

        [SetUp]
        public void SetUp()
        {
            _mockSetup = new ChatServiceMockSetup();
        }

        [Test]
        public async Task CreateChatRoom_ValidCall()
        {
            var (chatRepositoryMock, userRepositoryMock) = _mockSetup.CreateMock_CreateChatRoom();
            var chatService = new ChatService(userRepositoryMock.Object, chatRepositoryMock.Object);
            var participantsIds = new Guid[] { It.IsAny<Guid>(), It.IsAny<Guid>() };
            var inputModel = new CreateChatRoomModel(It.IsAny<String>(), participantsIds);

            await chatService.CreateChatRoom(inputModel);

            userRepositoryMock.Verify(x => x.SaveEntityAsync(It.IsAny<User>()), Times.Exactly(2));
            chatRepositoryMock.Verify(x => x.SaveEntityAsync(It.IsAny<ChatRoom>()), Times.Once);
        }

        [Test]
        public async Task GetChatRoomMessageHistory_ValidCall()
        {
            var (chatRepositoryMock, userRepositoryMock) = _mockSetup.CreateMock_GetChatRoomMessageHistory();
            var chatService = new ChatService(userRepositoryMock.Object, chatRepositoryMock.Object);
            var inputModel = new GetChatRoomMessageHistoryModel(It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>());

            var messages = await chatService.GetChatRoomMessageHistory(inputModel);

            Assert.IsTrue(messages.Count() == 1);
        }

        [Test]
        public void GetChatRoomMessageHistory_ThrowsEmptyCollectionException()
        {
            var (chatRepositoryMock, userRepositoryMock) = _mockSetup.CreateMock_GetChatRoomMessageHistory_ThrowsEmptyCollectionException();
            var chatService = new ChatService(userRepositoryMock.Object, chatRepositoryMock.Object);
            var inputModel = new GetChatRoomMessageHistoryModel(It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<Int32>());

            Assert.ThrowsAsync<EmptyCollectionException>(() => chatService.GetChatRoomMessageHistory(inputModel), "Chat has no messages.");
        }

        [Test]
        public async Task GetTimeLastMessageWasSeen_ValidCall()
        {
            var (chatRepositoryMock, userRepositoryMock) = _mockSetup.CreateMock_GetTimeLastMessageWasSeen();
            var chatService = new ChatService(userRepositoryMock.Object, chatRepositoryMock.Object);
            var inputModel = new GetTimeLastMessageWasSeenModel(It.IsAny<Guid>(), Guid.Parse("025e253c-4d18-405c-9848-4491ce35ec1f"));

            var time = await chatService.GetTimeLastMessageWasSeen(inputModel);

            Assert.IsTrue(time == DateTime.Today);
        }

        [Test]
        public void GetTimeLastMessageWasSeen_ThrowsChatTrackerException()
        {
            var (chatRepositoryMock, userRepositoryMock) = _mockSetup.CreateMock_GetTimeLastMessageWasSeen_ThrowsChatTrackerException();
            var chatService = new ChatService(userRepositoryMock.Object, chatRepositoryMock.Object);
            var inputModel = new GetTimeLastMessageWasSeenModel(It.IsAny<Guid>(), It.IsAny<Guid>());

            Assert.ThrowsAsync<ChatTrackerException>(
                () => chatService.GetTimeLastMessageWasSeen(inputModel), 
                "Chat tracker for user is not found.");
        }


        [Test]
        public async Task UpdateTimeLastMessageWasSeen_ValidCall()
        {
            var (chatRepositoryMock, userRepositoryMock) = _mockSetup.CreateMock_UpdateTimeLastMessageWasSeen();
            var chatService = new ChatService(userRepositoryMock.Object, chatRepositoryMock.Object);
            var inputModel = new UpdateTimeLastMessageWasSeenModel(It.IsAny<Guid>(), Guid.Parse("025e253c-4d18-405c-9848-4491ce35ec1f"), DateTime.Now);

            await chatService.UpdateTimeLastMessageWasSeen(inputModel);

            userRepositoryMock.Verify(x => x.SaveEntityAsync(It.IsAny<User>()));
        }

        [Test]
        public async Task GetChatRooms_ValidCall()
        {
            var (chatRepositoryMock, userRepositoryMock) = _mockSetup.CreateMock_GetChatRooms();
            var chatService = new ChatService(userRepositoryMock.Object, chatRepositoryMock.Object);

            var chats = await chatService.GetChatRooms(It.IsAny<Guid>());

            Assert.IsTrue(chats.Count() > 0);
        }

        [Test]
        public async Task GetNewMessages_ValidCall()
        {
            var (chatRepositoryMock, userRepositoryMock) = _mockSetup.CreateMock_GetNewMessages();
            var chatService = new ChatService(userRepositoryMock.Object, chatRepositoryMock.Object);

            var chatWithMessages = await chatService.GetNewMessages(It.IsAny<Guid>());

            Assert.IsTrue(chatWithMessages.Count() == 2);
        }
    }
}
