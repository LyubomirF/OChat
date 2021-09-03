using OChat;
using OChatApp.Models.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Services.Interfaces
{
    public interface IChatService
    {
        Task ConnectUserToChats(Guid userId);

        Task<ChatRoom> CreateChatRoom(String chatName, params Guid[] participantsIds);

        Task<ChatRoom> GetChat(Guid chatId);

        Task<IEnumerable<Message>> GetChatRoomMessageHistory(Guid chatId, QueryStringParams messageQueryParams);

        Task<IEnumerable<ChatRoom>> GetChatRooms(Guid userId);

        Task<IEnumerable<ChatRoom>> GetNewMessages(Guid userId);

        Task SendMessage(Guid chatId, Guid senderId, String message);

        Task<DateTime> GetLastReadMessageTimeStamp(Guid userId, Guid chatId);

        Task UpdateTimeLastMessageWasSeen(Guid userId, Guid chatId, DateTime timeLastMessageWasSeen);
    }
}
