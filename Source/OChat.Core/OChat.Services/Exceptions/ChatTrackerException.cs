using System;

namespace OChat.Core.Services.Exceptions
{
    public class ChatTrackerException : Exception
    {
        public ChatTrackerException(string message) 
            : base(message) { }
    }
}
