using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp
{
    public static class ChatRoutes
    {
        public const string CHATS = "chats";

        public const string MESSAGE = "{chatId}/message";

        public const string HISTORY = "{chatId}/history";

        public const string USER = "{userId}";

        public const string CHAT = "{chatId}";
        }

}
