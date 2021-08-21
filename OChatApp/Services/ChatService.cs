using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using OChatApp.Hubs;
using OChatApp.Models.QueryParameters;
using OChatApp.Services.Exceptions;
using OChatApp.Repositories;

namespace OChatApp.Services
{
    using static ExceptionMessages;

    public class ChatService
    {
        private readonly IHubContext<ChatHub, IClient> _hubContext;
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;

        public ChatService(
            IHubContext<ChatHub, IClient> hubContext,
            IUserRepository userRepostiry,
            IChatRepository chatRepository)
        {
            _hubContext = hubContext;
            _userRepository = userRepostiry;
            _chatRepository = chatRepository;
        }

        public async Task<ChatRoom> CreateChatRoom(string initiatorId, string targetId, string chatName)
        {
            var initiator = await _userRepository.GetUserWithChatsAndConnectionsAsync(initiatorId, INITIATOR_NOT_FOUND);

            var target = await _userRepository.GetUserWithChatsAndConnectionsAsync(targetId, TARGET_NOT_FOUND);

            var commonChat = GetCommonChat(initiator, target);

            if (commonChat != null)
                return commonChat;

            var newChat = new ChatRoom()
            {
                Name = chatName,
                Messages = new List<Message>(),
                Users = new List<OChatAppUser>()
                    {
                        initiator,
                        target
                    }
            };

            initiator.ChatRooms.Add(newChat);
            target.ChatRooms.Add(newChat);

            await _userRepository.Update(initiator);
            await _userRepository.Update(target);

            await AddUserConnectionsToSignalRGroup(initiator.Connections, newChat);
            await AddUserConnectionsToSignalRGroup(target.Connections, newChat);

            return newChat;
        }

        public async Task<IEnumerable<Message>> GetChatRoomMessageHistory(string chatId, QueryStringParams messageQueryParams)
        {
            var chat = await _chatRepository
                .GetChatRoomWithMessegesAsync(chatId, messageQueryParams.Page, messageQueryParams.PageSize, CHAT_NOT_FOUND);

            if (chat.Messages == null || chat.Messages.Count() == 0)
                throw new EmptyCollectionException("Chat has no messages.");

            return chat.Messages;
        }

        public async Task<IEnumerable<ChatRoom>> GetChatRooms(string userId)
        {
            var user = await _userRepository.GetUserWithChatsAndConnectionsAsync(userId, USER_NOT_FOUND);

            if (user.ChatRooms == null || user.ChatRooms.Count == 0)
                throw new EmptyCollectionException("User is not included in any chats.");

            await SetUpSignalRGroupsForExistingUserChats(user);

            return user.ChatRooms;
        }

        public async Task SendMessage(string chatId, string message)
            => await _hubContext.Clients.Group(chatId).ReceiveMessage(message);

        public async Task<ChatRoom> GetChat(string chatId)
        {
            var chat = await _chatRepository.GetChatWithUsersAsync(chatId, CHAT_NOT_FOUND);

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

        private async Task SetUpSignalRGroupsForExistingUserChats(OChatAppUser user)
        {
            foreach (var connection in user.Connections)
                foreach (var chat in user.ChatRooms)
                    await _hubContext.Groups.AddToGroupAsync(connection.Id, chat.Id);
        }
       
        private async Task AddUserConnectionsToSignalRGroup(IEnumerable<Connection> userConnections, ChatRoom chat)
        {
            foreach (var connection in userConnections)
                await _hubContext.Groups.AddToGroupAsync(connection.Id, chat.Id);
        }
    }
}
