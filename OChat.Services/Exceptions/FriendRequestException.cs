using System;

namespace OChat.Services.Exceptions
{
    public class FriendRequestException : Exception
    {
        public FriendRequestException(string message) 
            : base(message) { }
    }
}
