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

namespace OChatApp.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chat;
        public ChatController(ChatService chat)
        {
            _chat = chat;
        }

        //api design
        // POST api/chat/new pars: userId, chatName
        // GET api/history/{chatId} pars: chatId
        // POST api/chat/message/ pars: chatId, message
        // GET api/chat/chats/ pars: userId

        [HttpPost("chat/new")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatWithModel model)
        {
            await _chat.CreateChatRoom(User.FindFirst(ClaimTypes.NameIdentifier).Value, model.UserId, model.ChatName);
            return Ok();
        }

        [HttpGet("chats/{userId}")]
        public async Task<IActionResult> GetChatRooms(string userId)
        {
            var chats = await _chat.GetChatRooms(userId);

            if(chats == null)
                return Ok(new { result = "User has no chats." });

            return Ok(chats);
        }

        [HttpGet("history/{chatId}")]
        public async Task<IActionResult> GetChatRoomMessageHistory(string chatId)
        {
            var messages = await _chat.GetChatRoomMessageHistory(chatId);

            return Ok(messages);
        }

        [HttpPost("chat/message")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
        {
            await _chat.SendMessage(model.ChatId, model.Message);
            return Ok();
        }
    }
}
