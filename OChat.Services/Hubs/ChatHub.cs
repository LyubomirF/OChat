using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OChat.Domain;
using OChat.Services.Exceptions;
using OChat.Services.Interfaces.Repositories;

namespace OChat.Services.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IClient>
    {
        private readonly IUserRepository _userRepository;

        public ChatHub(IUserRepository userRepository)
            => _userRepository = userRepository;

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
    }
}