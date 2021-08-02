using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using OChatApp.Hubs;
using OChatApp.Models;
using OChatApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OChatApp;

namespace OChatApp.Controllers
{
    using static ChatRoutes;


    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly ChatService _chat;
        public ChatsController(ChatService chat)
        {
            _chat = chat;
        }

        [HttpPost(Name = nameof(CreateChat))]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatWithModel model)
        {
            var chat = await _chat.CreateChatRoom(model.InitiatorId, model.TargetId, model.ChatName);
            return CreatedAtRoute(nameof(GetChat), new { chat.Id }, null);
        }

        [HttpGet(USER, Name = nameof(GetChatRooms))]
        public async Task<IActionResult> GetChatRooms(string userId)
        {
            var chats = await _chat.GetChatRooms(userId);

            if(chats == null)
                return Ok(new { result = "User has no chats." });

            return Ok(chats);
        }

        [HttpGet(HISTORY, Name = nameof(GetChatRoomMessageHistory))]
        public async Task<IActionResult> GetChatRoomMessageHistory(string chatId)
        {
            var messages = await _chat.GetChatRoomMessageHistory(chatId);

            return Ok(messages);
        }

        [HttpPost(MESSAGE, Name = nameof(SendMessage))]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
        {
            await _chat.SendMessage(model.ChatId, model.Message);
            return Ok();
        }

        [HttpGet(CHAT, Name = nameof(GetChat))]
        public async Task<IActionResult> GetChat(string chatId)
        {
            var users = await _chat.GetChat(chatId);
            return Ok(users);
        }
    }
}
