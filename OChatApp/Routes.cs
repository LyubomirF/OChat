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

    public static class UsersRoutes
    {
        public const string USERS = "users";

        public const string FRIENDS = "{userId}/friends";

        public const string REQESTS = "{userId}/requests";

        public const string REQUEST = "{userId}/request/{targetUserId}";

        public const string ACCEPT = "{userId}/requests/{requestId}/accept";

        public const string REJECT = "{userId}/requests/{requestId}/reject";

        public const string REMOVE = "{userId}/friends/{friendId}/remove";

    }

}
