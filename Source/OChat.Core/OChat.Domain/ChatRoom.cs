using System;
using System.Collections.Generic;

namespace OChat.Domain
{
    public class ChatRoom
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public ICollection<User> Participants { get; set; } = new List<User>();

        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}