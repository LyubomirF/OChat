using System;

namespace OChat.Core.Services.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException(String message)
            : base(message) { }
    }
}
