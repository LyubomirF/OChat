﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OChat;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using OChatApp.Repositories;
using OChatApp.Repositories.Exceptions;
using OChatApp.Repositories.Interfaces;
using OChatApp.Services.Exceptions;

namespace OChatApp.Hubs
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
                throw new NotFoundException("No logged user found.");

            var callerConnectionId = Context.ConnectionId;

            var user = await _userRepository
                .GetUserWithConnectionsAsync(Guid.Parse(Context.UserIdentifier));

            var newUserConnection = new Connection()
            {
                Id = Guid.Parse(callerConnectionId)
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
                .SingleOrDefault(c => c.Id == Guid.Parse(Context.ConnectionId));

            user.Connections.Remove(userConnection);

            await _userRepository.SaveEntityAsync(user);

            await base.OnDisconnectedAsync(exception);
        }
    }
}