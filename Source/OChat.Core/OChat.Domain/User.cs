using System;
using System.Collections.Generic;

namespace OChat.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public String Username { get; set; }

        public String Email { get; set; }

        public String PasswordHash { get; set; }

        public ICollection<User> Friends { get; set; } = new List<User>();

        public ICollection<FriendRequest> FriendRequests { get; set; } = new List<FriendRequest>();

        public ICollection<Connection> Connections { get; set; } = new List<Connection>();

        public ICollection<ChatTracker> ChatTrackers { get; set; } = new List<ChatTracker>();
    }
}