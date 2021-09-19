using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OChat.Domain;
using OChat.Core.Services.Interfaces;
using OChat.Core.Common.Repositories;
using OChat.Core.Services.Exceptions;
using OChat.Core.OChat.Services.InputModels;

namespace OChat.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;

        public ChatService(
            IUserRepository userRepostiry,
            IChatRepository chatRepository)
        {
            _userRepository = userRepostiry;
            _chatRepository = chatRepository;
        }

        public async Task<ChatRoom> CreateChatRoom(CreateChatRoomModel input)
        {
            var participants = await GetParticipants(input.ParticipantsIds);

            var newChat = await CreateNewChat(participants, input.ChatName);

            await CreateChatTrackerFor(participants, newChat);

            return newChat;
        }

        public async Task<IEnumerable<Message>> GetChatRoomMessageHistory(GetChatRoomMessageHistoryModel input)
        {
            var chat = await _chatRepository
                .GetChatRoomWithMessegesAsync(input.ChatId, input.Page, input.PageSize);

            if (chat.Messages == null || chat.Messages.Count == 0)
                throw new EmptyCollectionException("Chat has no messages.");

            return chat.Messages;
        }

        public async Task<DateTime> GetTimeLastMessageWasSeen(GetTimeLastMessageWasSeenModel input)
        {
            var user = await _userRepository.GetUserWithChatTrackers(input.UserId);

            var tracker = user.ChatTrackers
                .SingleOrDefault(t => t.Chat.Id == input.ChatId);

            if (tracker is null)
                throw new ChatTrackerException("Chat tracker for user is not found.");

            return tracker.LastReadMessageTimeStamp.Value;
        }

        public async Task UpdateTimeLastMessageWasSeen(UpdateTimeLastMessageWasSeenModel input)
        {
            var user = await _userRepository.GetUserWithChatTrackers(input.UserId);

            var tracker = user.ChatTrackers
                .SingleOrDefault(t => t.Chat.Id == input.ChatId);

            if (tracker is null)
                throw new ChatTrackerException("Chat tracker for user is not found.");

            tracker.LastReadMessageTimeStamp = input.TimeLastMessageWasSeen;

            await _userRepository.SaveEntityAsync(user);
        }

        public async Task<IEnumerable<ChatRoom>> GetChatRooms(Guid userId)
        {
            var userChats = await _chatRepository.GetChatsForUser(userId);

            return !userChats.Any()
                ? throw new EmptyCollectionException("User is not included in any chats.")
                : userChats;
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
    }
}
