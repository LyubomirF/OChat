using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using OChatApp.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Services
{
    public class ChatService
    {
        private readonly IHubContext<ChatHub, IClient> _hubContext;
        private readonly OChatAppContext _dbContext;

        public ChatService(IHubContext<ChatHub, IClient> hubContext, OChatAppContext dbContext)
        {
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        public async Task<ChatRoom> CreateChatRoom(string userId1, string userId2, string chatName)
        {
            var caller = await GetUserById(userId1);
            var otherUser = await GetUserById(userId2);

            var commonChat = GetCommonChat(caller, otherUser);

            if (commonChat != null)
                return commonChat;

            var chat = new ChatRoom()
            {
                Name = chatName,
                Messages = new List<Message>(),
                Users = new List<OChatAppUser>()
                    {
                        caller,
                        otherUser
                    }
            };

            await _dbContext.AddAsync(chat);

            foreach (var connection in caller.Connections)
                await _hubContext.Groups.AddToGroupAsync(connection.Id, chat.Id);

            foreach (var connection in otherUser.Connections)
                await _hubContext.Groups.AddToGroupAsync(connection.Id, chat.Id);

            return chat;
        }

        public async Task<IEnumerable<Message>> GetChatRoomMessageHistory(string chatId)
        {
            var chat = await _dbContext.ChatRooms
                .Include(c => c.Messages)
                .ThenInclude(m => m.From)
                .SingleOrDefaultAsync(c => c.Id == chatId);

            //return custom exception
            if (chat == null || chat.Messages == null)
                return null;

            return chat.Messages;
        }

        public async Task<IEnumerable<ChatRoom>> GetChatRooms(string userId)
        {
            var user = await GetUserById(userId);

            if (user.ChatRooms == null)
                return null;

            await AddUserToChatGroups(user);

            var chats = user.ChatRooms.Select(
                x => new ChatRoom()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return chats;
        }

        public async Task SendMessage(string chatId, string message)
            => await _hubContext.Clients.Group(chatId).ReceiveMessage(message);

        public async Task<ChatRoom> GetChat(string chatId)
        {
           var chat = await _dbContext.ChatRooms
                .Include(c => c.Users)
                .SingleOrDefaultAsync(c => c.Id == chatId);

            return chat;
        }

        private ChatRoom GetCommonChat(OChatAppUser user1, OChatAppUser user2)
        {
            if (user1.ChatRooms == null || user2.ChatRooms == null)
                return null;

            var hashset = new HashSet<string>();
            var list = new List<ChatRoom>();

            foreach (var chat in user1.ChatRooms)
                hashset.Add(chat.Id);

            foreach (var chat in user2.ChatRooms)
                if (hashset.Contains(chat.Id))
                    list.Add(chat);

            var commonChat = list
                .Where(c => c.Users.Count == 2)
                .SingleOrDefault();

            if (commonChat == null)
                return null;

            return commonChat;
        }

        private async Task<OChatAppUser> GetUserById(string userId)
            => await _dbContext.Users
                .Include(u => u.ChatRooms)
                .ThenInclude(x => x.Users)
                .Include(u => u.Connections)
                .SingleOrDefaultAsync(u => u.Id == userId);

        private async Task AddUserToChatGroups(OChatAppUser user)
        {
            foreach (var chat in user.ChatRooms)
                foreach (var connection in user.Connections)
                    await _hubContext.Groups.AddToGroupAsync(connection.Id, chat.Id);
        }
    }
}
