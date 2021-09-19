using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OChat.Core.Common.Repositories;
using OChat.Core.Communication.Exceptions;
using OChat.Domain;

namespace OChat.Core.Communication
{
    public class ChatHub : Hub<IClient>
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;

        public ChatHub(IUserRepository userRepository, IChatRepository chatRepository)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.UserIdentifier is null)
                throw new UserNotFoundException("No logged user found.");

            var callerConnectionId = Context.ConnectionId;

            var user = await _userRepository
                .GetUserWithConnectionsAsync(Guid.Parse(Context.UserIdentifier));

            var newUserConnection = new Connection()
            {
                Id = callerConnectionId
            };

            user.Connections.Add(newUserConnection);

            await _userRepository.SaveEntityAsync(user);

            await base.OnConnectedAsync();
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
            Task sendMessage = Clients.Group(chatId.ToString()).ReceiveMessage(message);

            await Task.WhenAll(saveMessage, sendMessage);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _userRepository
                .GetUserWithConnectionsAsync(Guid.Parse(Context.UserIdentifier));

            var userConnection = user.Connections
                .SingleOrDefault(c => c.Id == Context.ConnectionId);

            user.Connections.Remove(userConnection);

            await _userRepository.SaveEntityAsync(user);

            await base.OnDisconnectedAsync(exception);
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
                        Groups.AddToGroupAsync(connection.Id.ToString(), chat.Id.ToString());

            return Task.WhenAll(addUserConnectionsToSignalRGroups);
        }

    }
}