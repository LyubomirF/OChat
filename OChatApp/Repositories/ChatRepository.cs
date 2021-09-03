using Microsoft.EntityFrameworkCore;
using OChat;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using OChatApp.Repositories.Exceptions;
using OChatApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Repositories
{
    using static ExceptionMessages;

    public class ChatRepository : Repository<ChatRoom>, IChatRepository
    {
        public ChatRepository(OChatAppContext dbContext)
            : base(dbContext) { }

        public Task<ChatRoom> GetEntityByIdAsync(Guid chatId)
            =>  GetEntityByIdAsync(chatId, CHAT_NOT_FOUND);

        public async Task<ChatRoom> GetChatRoomWithMessegesAsync(Guid chatId, Int32 page, Int32 pageSize)
        {
            var chat = await _dbContext.ChatRooms
                .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
                .SingleOrDefaultAsync(c => c.Id == chatId);

            if (chat is null)
                throw new NotFoundException(CHAT_NOT_FOUND);

            chat.Messages = chat.Messages
                .OrderBy(m => m.SentOn.Date)
                .ThenBy(m => m.SentOn.TimeOfDay)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return chat;
        }

        public async Task<ChatRoom> GetChatWithParticipantsAsync(Guid chatId)
        {
            var chat = await _dbContext.ChatRooms
                           .Include(c => c.Participants)
                           .SingleOrDefaultAsync(c => c.Id == chatId);

            return chat is null
                ? throw new NotFoundException(CHAT_NOT_FOUND)
                : chat;
        }

        public async Task<IEnumerable<ChatRoom>> GetChatsForUser(Guid userId)
        {
            var user = await _dbContext.DomainUsers
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                throw new NotFoundException(USER_NOT_FOUND);

            return _dbContext.ChatRooms
                  .Include(c => c.Participants)
                  .Where(c => c.Participants.Where(p => p.Id == userId).Any())
                  .ToList();
        }

        public Task<ChatRoom> GetChatWithMessagesAfter(Guid chatId, DateTime time)
        {
            var chat = _dbContext.ChatRooms
                .Include(c => c.Messages.Where(m => m.SentOn > time))
                .ThenInclude(m => m.Sender)
                .SingleOrDefaultAsync(c => c.Id == chatId);

            return chat is null 
                ? throw new NotFoundException(CHAT_NOT_FOUND)
                : chat;
        }

        public Task DeleteChat(ChatRoom chat)
            => Delete(chat);

    }
}
