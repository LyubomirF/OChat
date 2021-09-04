using System;

namespace OChat.Domain
{
    public class Message
    {
        public Guid Id { get; set; } 

        public DateTime SentOn { get; set; }

        public String Content { get; set; }

        public User Sender { get; set; }

    }
}
