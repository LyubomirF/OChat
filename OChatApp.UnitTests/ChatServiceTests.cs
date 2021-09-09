using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.SignalR;
using OChatApp.Models.QueryParameters;
using OChatApp.UnitTests.Helper;

namespace OChatApp.UnitTests
{

    //[TestFixture]
    //class ChatServiceTests
    //{
    //    private Database _db;

    //    [SetUp]
    //    public void SetUp()
    //    {
    //        _db = new Database();
    //    }

    //    [Test]
    //    [TestCase("1")]
    //    public async Task GetChatRoomMessageHistory(string chatId)
    //    {
    //        //Arrange
    //        var userRepositoryMock = new Mock<IUserRepository>();
    //        var chatRepositoryMock = new Mock<IChatRepository>();
    //        var hubContextMock = new Mock<IHubContext<ChatHub, IClient>>();
    //        var queryParams = new QueryStringParams();

    //        var chat = _db.Chats.SingleOrDefault(x => x.Id == chatId);

    //        chatRepositoryMock
    //            .Setup(x => x.GetChatRoomWithMessegesAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<String>()))
    //            .ReturnsAsync(chat);

    //        var chatService = new ChatService(hubContextMock.Object, userRepositoryMock.Object, chatRepositoryMock.Object);

    //        //Act
    //        var messages = await chatService.GetChatRoomMessageHistory(chatId, queryParams);

    //        Assert.AreSame(chat.Messages, messages);
    //    }
    //}
}
