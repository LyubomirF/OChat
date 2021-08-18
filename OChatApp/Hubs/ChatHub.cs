using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using OChatApp.Repositories;
using OChatApp.Services.Exceptions;

namespace OChatApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IClient>
    {
        private readonly IUserRepository _userRepository;

        public ChatHub(IUserRepository userRepository)
            => _userRepository = userRepository;

        //public async Task SendMessage(string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", message);
        //}

        public override Task OnConnectedAsync()
        {
            if (Context.UserIdentifier is null)
                throw new NotFoundException("No logged user found.");

            var callerConnectionId = Context.ConnectionId;

            var user = _userRepository
                .GetUserWithConnections(Context.UserIdentifier);

            var newUserConnection = new Connection()
            {
                Id = callerConnectionId,
                Connected = true
            };

            if (user.Connections == null)
                user.Connections = new List<Connection>();

            user.Connections.Add(newUserConnection);

            _userRepository.Update(user);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = _userRepository
                .GetUserWithConnections(Context.UserIdentifier);

            var userConnection = user.Connections
                .SingleOrDefault(c => c.Id == Context.ConnectionId);

            user.Connections.Remove(userConnection);

            _userRepository.Update(user);

            return base.OnDisconnectedAsync(exception);
        }
    }
}