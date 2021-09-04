using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OChat.Domain;

namespace OChat.Infrastructure.Repositories.Interfaces
{
    public interface IChatRepository : IRepository<ChatRoom>
    {
        Task DeleteChat(ChatRoom chat);

        Task<ChatRoom> GetChatRoomWithMessegesAsync(Guid chatId, Int32 page, Int32 pageSize);

        Task<ChatRoom> GetChatWithParticipantsAsync(Guid chatId);

        Task<IEnumerable<ChatRoom>> GetChatsForUser(Guid userId);

        Task<ChatRoom> GetChatWithMessagesAfter(Guid chatId, DateTime time);
    }
}
