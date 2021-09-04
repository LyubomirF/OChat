using System;

namespace OChat.Services.Exceptions
{
    public class EmptyCollectionException : Exception
    {
        public EmptyCollectionException(string message) 
            : base(message) { }
    }
}
