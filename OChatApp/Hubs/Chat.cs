
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OChatApp.Data;

namespace OChatApp.Hubs
{
    public class Chat : Hub
    {
        private readonly static Dictionary<string, string> connectionMappings =
            new Dictionary<string, string>();

        private readonly OChatAppContext _dbContext;

        public Chat(OChatAppContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task CreateChatWith(string userId)
        {
            //adds caller and userId to a group
            //creates database record about chat and 
            //return chatId
        }

        public async Task GetChatMessageHistory(string chatId)
        {

        }

        public async Task SendMessage(string chatId, string message)
        {

        }

        public override Task OnConnectedAsync()
        {
            var userConnection = Context.ConnectionId;
            var user = _dbContext.Users
                .Where(x => x.UserName == Context.User.Identity.Name)
                .Include(u => u.ChatRooms)
                .SingleOrDefault();

            connectionMappings.Add(user.Id, userConnection);

            if(user.ChatRooms != null)
            {
                foreach (var chat in user.ChatRooms)
                    Groups.AddToGroupAsync(userConnection, chat.Id);

                Clients.Caller.SendAsync("ReceiveUserChats", user.ChatRooms);
            }

            //see if user has any chats
            //if yes, make groups
            //if not, just add him to connectionsMapping
            //return chatIds

            return base.OnConnectedAsync();
        }
    }
}
