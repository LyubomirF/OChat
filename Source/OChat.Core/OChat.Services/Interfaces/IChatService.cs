using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OChat.Core.OChat.Services.InputModels;
using OChat.Domain;

namespace OChat.Core.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatRoom> CreateChatRoom(CreateChatRoomModel input);

        Task<ChatRoom> GetChat(Guid chatId);

        Task<IEnumerable<Message>> GetChatRoomMessageHistory(GetChatRoomMessageHistoryModel input);

        Task<IEnumerable<ChatRoom>> GetChatRooms(Guid userId);

        Task<IEnumerable<ChatRoom>> GetNewMessages(Guid userId);

        Task<DateTime> GetTimeLastMessageWasSeen(GetTimeLastMessageWasSeenModel input);

        Task UpdateTimeLastMessageWasSeen(UpdateTimeLastMessageWasSeenModel input);
    }
}
