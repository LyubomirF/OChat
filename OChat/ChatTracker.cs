using System;

namespace OChat.Domain
{
    public class ChatTracker
    {
        public Guid Id { get; set; }

        public ChatRoom Chat { get; set; }

        public DateTime? LastReadMessageTimeStamp { get; set; }
    }
}
