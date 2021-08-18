using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Services.Exceptions
{
    public class EmptyCollectionException : Exception
    {
        public EmptyCollectionException(string message) 
            : base(message) { }
    }
}
