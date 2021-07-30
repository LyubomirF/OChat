using OChatApp.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Services
{
    public interface IChat
    {
        Task CreateChat(string userId1, string userId2, string chatName);

        Task<IEnumerable<ChatRoom>> GetChats(string userId);

        Task<IEnumerable<Message>> GetChatRoomMessageHistory(string chatId);

        Task SendMessage(string chatId, string message);


    }
}
