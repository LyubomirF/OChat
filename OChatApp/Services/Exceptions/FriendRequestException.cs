using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Services.Exceptions
{
    public class FriendRequestException : Exception
    {
        public FriendRequestException(string message) 
            : base(message) { }
    }
}
