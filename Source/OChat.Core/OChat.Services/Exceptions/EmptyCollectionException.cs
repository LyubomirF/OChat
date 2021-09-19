using System;

namespace OChat.Core.Services.Exceptions
{
    public class EmptyCollectionException : Exception
    {
        public EmptyCollectionException(string message) 
            : base(message) { }
    }
}
