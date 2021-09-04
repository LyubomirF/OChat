using System;

namespace OChat.Services.Exceptions
{
    public class ChatTrackerException : Exception
    {
        public ChatTrackerException(string message) 
            : base(message) { }
    }
}
