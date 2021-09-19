using System;

namespace OChat.Core.Services.Exceptions
{
    public class FriendRequestException : Exception
    {
        public FriendRequestException(string message)
            : base(message) { }
    }
}
