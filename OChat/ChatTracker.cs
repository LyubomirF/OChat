using System;

namespace OChat
{
    public class ChatTracker
    {
        public Guid Id { get; set; }

        public ChatRoom Chat { get; set; }

        public DateTime? LastReadMessageTimeStamp { get; set; }
    }
}
