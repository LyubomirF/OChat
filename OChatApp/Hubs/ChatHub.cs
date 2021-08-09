﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using OChatApp.Services.Exceptions;

namespace OChatApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IClient>
    {
        private readonly OChatAppContext _dbContext;

        public ChatHub(OChatAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task SendMessage(string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", message);
        //}

        public override Task OnConnectedAsync()
        {
            if (Context.UserIdentifier is null)
                throw new NotFoundException("No logged user found.");

            var callerConnectionId = Context.ConnectionId;

            var user = _dbContext.Users
                .Include(u => u.Connections)
                .SingleOrDefault(x => x.Id == Context.UserIdentifier);

            if (user.Connections == null)
                user.Connections = new List<Connection>();

            user.Connections.Add(new Connection()
            {
                Id = callerConnectionId,
                Connected = true
            });

            _dbContext.SaveChanges();

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = _dbContext.Users
                .Include(u => u.Connections)
                .SingleOrDefault(x => x.Id == Context.UserIdentifier);

            var userConnection = _dbContext.Connections.Find(Context.ConnectionId);

            user.Connections.Remove(userConnection);

            _dbContext.SaveChanges();

            return base.OnDisconnectedAsync(exception);
        }
    }
}