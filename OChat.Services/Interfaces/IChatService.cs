using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OChat.Domain;

namespace OChat.Services.Interfaces
{
    public interface IChatService
    {
        Task ConnectUserToChats(Guid userId);

        Task<ChatRoom> CreateChatRoom(String chatName, params Guid[] participantsIds);

        Task<ChatRoom> GetChat(Guid chatId);

        Task<IEnumerable<Message>> GetChatRoomMessageHistory(Guid chatId, Int32 page, Int32 pageSize);

        Task<IEnumerable<ChatRoom>> GetChatRooms(Guid userId);

        Task<IEnumerable<ChatRoom>> GetNewMessages(Guid userId);

        Task SendMessage(Guid chatId, Guid senderId, String message);

        Task<DateTime> GetLastReadMessageTimeStamp(Guid userId, Guid chatId);

        Task UpdateTimeLastMessageWasSeen(Guid userId, Guid chatId, DateTime timeLastMessageWasSeen);
    }
}
