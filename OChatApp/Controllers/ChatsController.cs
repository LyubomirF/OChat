using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OChatApp.Models;
using System;
using System.Threading.Tasks;
using OChatApp.Models.QueryParameters;
using OChat.Services.Interfaces;

namespace OChatApp.Controllers
{
    using static ChatRoutes;

    [Route(CHATS)]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost(Name = nameof(CreateChat))]
        public async Task<IActionResult> CreateChat([FromQuery] CreateChatWithModel model)
        {
            var chat = await _chatService.CreateChatRoom(model.ChatName, model.ParticipantsIds);
            return CreatedAtRoute(nameof(GetChatRoom), new { chat.Id }, null);
        }

        [HttpGet(HISTORY, Name = nameof(GetChatRoomMessageHistory))]
        public async Task<IActionResult> GetChatRoomMessageHistory(Guid chatId, [FromQuery] QueryStringParams chatQueryParams)
        {
            var messages = await _chatService.GetChatRoomMessageHistory(chatId, chatQueryParams.Page, chatQueryParams.PageSize);
            return Ok(messages);
        }

        [HttpGet(LAST_READ_MESSAGE_TIMESTAMP, Name = nameof(GetChatLastReadMessageTimeStamp))]
        public async Task<IActionResult> GetChatLastReadMessageTimeStamp(Guid userId, Guid chatId)
        {
            var timeStamp = await _chatService.GetLastReadMessageTimeStamp(userId, chatId);
            return Ok(timeStamp);
        }

        [HttpPost(UPDATE_LAST_READ_MESSAGE_TIMESTAMP, Name = nameof(UpdateChatLastReadMessageTimeStamp))]
        public async Task<IActionResult> UpdateChatLastReadMessageTimeStamp([FromQuery] UpdateTimeLastMessageWasSeenModel model)
        {
            await _chatService.UpdateTimeLastMessageWasSeen(model.UserId, model.UserId, model.TimeLastMessageWasSeen);
            return Ok();
        }

        [HttpGet(CONNECT, Name = nameof(ConnectUserToChats))]
        public async Task<IActionResult> ConnectUserToChats(Guid userId)
        {
            await _chatService.ConnectUserToChats(userId);
            return Ok();
        }

        [HttpPost(MESSAGE, Name = nameof(SendMessage))]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
        {
            await _chatService.SendMessage(model.ChatId, model.SenderId, model.Message);
            return Ok();
        }

        [HttpGet(USER, Name = nameof(GetChatRooms))]
        public async Task<IActionResult> GetChatRooms(Guid userId)
        {
            var chats = await _chatService.GetChatRooms(userId);
            return Ok(chats);
        }

        [HttpGet(CHAT, Name = nameof(GetChatRoom))]
        public async Task<IActionResult> GetChatRoom(Guid chatId)
        {
            var users = await _chatService.GetChat(chatId);
            return Ok(users);
        }

        [HttpGet(NEW_MESSAGES, Name = nameof(GetNewMessagesForUserChats))]
        public async Task<IActionResult> GetNewMessagesForUserChats(Guid userId)
        {
            var chats = await _chatService.GetNewMessages(userId);
           
            return Ok(chats);
        }
    }
}
