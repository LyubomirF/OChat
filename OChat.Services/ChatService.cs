using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OChat.Domain;
using OChat.Services.Interfaces;
using OChat.Services.Exceptions;
using OChat.Infrastructure.Repositories.Interfaces;
using OChat.Infrastructure.Hubs;

namespace OChat.Services
{
    public class ChatService : IChatService
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

        public async Task<ChatRoom> CreateChatRoom(String chatName, params Guid[] participantsIds)
        {
            var participants = await GetParticipants(participantsIds);

            var newChat = await CreateNewChat(participants, chatName);

            await CreateChatTrackerFor(participants, newChat);

            return newChat;
        }

        public async Task<IEnumerable<Message>> GetChatRoomMessageHistory(Guid chatId, Int32 page, Int32 pageSize)
        {
            var chat = await _chatRepository
                .GetChatRoomWithMessegesAsync(chatId, page, pageSize);

            if (chat.Messages == null || chat.Messages.Count == 0)
                throw new EmptyCollectionException("Chat has no messages.");

            return chat.Messages;
        }

        public async Task<DateTime> GetLastReadMessageTimeStamp(Guid userId, Guid chatId)
        {
            var user = await _userRepository.GetUserWithChatTrackers(userId);

            var tracker = user.ChatTrackers
                .SingleOrDefault(t => t.Chat.Id == chatId);

            if (tracker is null)
                throw new ChatTrackerException("Chat tracker for user is not found.");

            return tracker.LastReadMessageTimeStamp.Value;
        }

        public async Task UpdateTimeLastMessageWasSeen(Guid userId, Guid chatId, DateTime timeLastMessageWasSeen)
        {
            var user = await _userRepository.GetUserWithChatTrackers(userId);

            var tracker = user.ChatTrackers
                .SingleOrDefault(t => t.Chat.Id == chatId);

            if (tracker is null)
                throw new ChatTrackerException("Chat tracker for user is not found.");

            tracker.LastReadMessageTimeStamp = timeLastMessageWasSeen;

            await _userRepository.SaveEntityAsync(user);
        }

        public async Task<IEnumerable<ChatRoom>> GetChatRooms(Guid userId)
        {
            var userChats = await _chatRepository.GetChatsForUser(userId);

            return !userChats.Any()
                ? throw new EmptyCollectionException("User is not included in any chats.")
                : userChats;
        }

        public async Task ConnectUserToChats(Guid userId)
        {
            var user = await _userRepository.GetUserWithConnectionsAsync(userId);
            var chats = await _chatRepository.GetChatsForUser(userId);

            await SetUpSignalRGroupsFor(chats, user);
        }

        public async Task SendMessage(Guid chatId, Guid senderId, String message)
        {
            Task saveMessage = SaveMessageToDatabase(chatId, senderId, message);
            Task sendMessage = _hubContext.Clients.Group(chatId.ToString()).ReceiveMessage(message);

            await Task.WhenAll(saveMessage, sendMessage);
        }

        public Task<ChatRoom> GetChat(Guid chatId)
            => _chatRepository.GetEntityByIdAsync(chatId);

        public async Task<IEnumerable<ChatRoom>> GetNewMessages(Guid userId)
        {
            var user = await _userRepository.GetUserWithChatTrackers(userId);

            IEnumerable<Task<ChatRoom>> chatsWithNewMessages = user.ChatTrackers
                .Select(async x => await _chatRepository.GetChatWithMessagesAfter(x.Chat.Id, x.LastReadMessageTimeStamp.Value));

            return await Task.WhenAll(chatsWithNewMessages);
        }

        private Task<User[]> GetParticipants(Guid[] participantsIds)
        {
            Task<User>[] getUsers = new Task<User>[participantsIds.Length];

            for (int i = 0; i < participantsIds.Length; i++)
                getUsers[i] = _userRepository.GetEntityByIdAsync(participantsIds[i]);

            return Task.WhenAll(getUsers);
        }

        private async Task<ChatRoom> CreateNewChat(User[] users, String chatName)
        {
            var newChat = new ChatRoom()
            {
                Name = chatName,
                Participants = users
            };

            await _chatRepository.SaveEntityAsync(newChat);

            return newChat;
        }

        private Task CreateChatTrackerFor(User[] participants, ChatRoom newChat)
        {
            var trackers = new List<ChatTracker>();

            for (int i = 0; i < participants.Length; i++)
                trackers.Add(new ChatTracker()
                {
                    Chat = newChat,
                    LastReadMessageTimeStamp = DateTime.Now
                });

            for (int i = 0; i < participants.Length; i++)
                participants[i].ChatTrackers.Add(trackers[i]);

            Task[] saveParticipants = new Task[participants.Length];

            for (int i = 0; i < participants.Length; i++)
                saveParticipants[i] = _userRepository.SaveEntityAsync(participants[i]);

            return Task.WhenAll(saveParticipants);
        }

        private async Task SaveMessageToDatabase(Guid chatId, Guid senderId, String message)
        {
            var sender = await _userRepository.GetEntityByIdAsync(senderId);
            var chat = await _chatRepository.GetEntityByIdAsync(chatId);

            var newMessage = new Message()
            {
                Content = message,
                SentOn = DateTime.Now,
                Sender = sender
            };

            chat.Messages.Add(newMessage);

            await _chatRepository.SaveEntityAsync(chat);
        }

        private Task SetUpSignalRGroupsFor(IEnumerable<ChatRoom> chats, User user)
        {
            var numberOfConnections = user.Connections.Count;
            var numberOfChats = chats.Count();

            Task[] addUserConnectionsToSignalRGroups = new Task[numberOfConnections * numberOfChats];

            Int32 taskCounter = 0;

            foreach (var chat in chats)
                foreach (var connection in user.Connections)
                    addUserConnectionsToSignalRGroups[taskCounter++] =
                        _hubContext.Groups.AddToGroupAsync(connection.Id.ToString(), chat.Id.ToString());

            return Task.WhenAll(addUserConnectionsToSignalRGroups);
        }
    }
}
