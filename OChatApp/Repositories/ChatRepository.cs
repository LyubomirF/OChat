using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using OChatApp.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Repositories
{
    public class ChatRepository : Repository<ChatRoom>, IChatRepository
    {
        public ChatRepository(OChatAppContext dbContext)
            : base(dbContext) { }

        public async Task<ChatRoom> GetChatRoomWithMessegesAsync(string chatId, int page, int pageSize)
        {
            var chat = await _dbContext.ChatRooms
                .Include(c => c.Messages)
                .ThenInclude(m => m.From)
                .SingleOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
                throw new NotFoundException("No chat was found.");

            chat.Messages = chat.Messages
                .OrderBy(m => m.SentOn.Date)
                .ThenBy(m => m.SentOn.TimeOfDay)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return chat;
        }

        public Task<ChatRoom> GetChatWithUsersAsync(string chatId)
            => _dbContext.ChatRooms
                .Include(c => c.Users)
                .SingleOrDefaultAsync(c => c.Id == chatId);
    }
}
