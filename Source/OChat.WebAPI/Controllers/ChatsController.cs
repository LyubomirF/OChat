using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OChat.Core.OChat.Services.InputModels;
using OChat.Core.Services.Interfaces;
using OChat.WebAPI.Models.QueryParameters;
using OChat.WebAPI.Models;
using System;
using System.Threading.Tasks;

namespace OChat.WebAPI.Controllers
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
        public async Task<IActionResult> CreateChat([FromQuery] CreateChat request)
        {
            var chat = await _chatService.CreateChatRoom(new CreateChatRoomModel(request.ChatName, request.ParticipantsIds));
            return CreatedAtRoute(nameof(GetChatRoom), new { chat.Id }, null);
        }

        [HttpGet(HISTORY, Name = nameof(GetChatRoomMessageHistory))]
        public async Task<IActionResult> GetChatRoomMessageHistory(Guid chatId, [FromQuery] QueryStringParams chatQueryParams)
        {
            var messages = await _chatService.GetChatRoomMessageHistory(
                new GetChatRoomMessageHistoryModel(chatId, chatQueryParams.Page, chatQueryParams.PageSize));

            return Ok(messages);
        }

        [HttpGet(TIME_LAST_MESSAGE_WAS_READ, Name = nameof(GetTimeLastMessageWasSeen))]
        public async Task<IActionResult> GetTimeLastMessageWasSeen(TimeLastMessageWasSeen request)
        {
            var timeStamp = await _chatService.GetTimeLastMessageWasSeen(
                new GetTimeLastMessageWasSeenModel(request.UserId, request.ChatId));
            return Ok(timeStamp);
        }

        [HttpPatch(TIME_LAST_MESSAGE_WAS_READ, Name = nameof(UpdateChatLastReadMessageTimeStamp))]
        public async Task<IActionResult> UpdateChatLastReadMessageTimeStamp(TimeLastMessageWasSeen request, [FromBody] DateTime timeLastMessageWasSeen)
        {
            await _chatService.UpdateTimeLastMessageWasSeen(
                new UpdateTimeLastMessageWasSeenModel(request.UserId, request.ChatId, timeLastMessageWasSeen));

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
