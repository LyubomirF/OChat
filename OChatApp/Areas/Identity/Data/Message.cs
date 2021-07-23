using System;

namespace OChatApp.Areas.Identity.Data
{
    public class Message
    {
        public string Id { get; set; }

        public DateTime SentOn { get; set; }

        public ChatRoom ChatRoom { get; set; }

        public OChatAppUser From { get; set; }
    }
}