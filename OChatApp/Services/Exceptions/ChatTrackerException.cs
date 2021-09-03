using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Services.Exceptions
{
    public class ChatTrackerException : Exception
    {
        public ChatTrackerException(string message) 
            : base(message) { }
    }
}
