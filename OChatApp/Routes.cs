using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp
{
    public static class ChatRoutes
    {
        public const String CHATS = "chats";

        public const String MESSAGE = "{chatId}/message";

        public const String HISTORY = "{chatId}/history";

        public const String USER = "{userId}";

        public const String CHAT = "{chatId}";

        public const String LAST_READ_MESSAGE_TIMESTAMP = "{chatId}/lastReadMessage/{userId}";

        public const String UPDATE_LAST_READ_MESSAGE_TIMESTAMP = "{chatId}/lastReadMessage/{userId}/{lastReadMessageTimeStamp}";

        public const String CONNECT = "{userId}/connect";

        public const String NEW_MESSAGES = "{userId}/newMessages";
    }

    public static class UsersRoutes
    {
        public const String USERS = "users";

        public const String FRIENDS = "{userId}/friends";

        public const String REQESTS = "{userId}/requests";

        public const String REQUEST = "{userId}/request/{targetUserId}";

        public const String ACCEPT = "{userId}/requests/{requestId}/accept";

        public const String REJECT = "{userId}/requests/{requestId}/reject";

        public const String REMOVE = "{userId}/friends/{friendId}/remove";
    }

    public static class AuthenticationRoutes
    {
        public const String AUTHENTICATION = "auth";

        public const String TOKEN = "token";

        public const String REGISTER = "register";

        public const String LOGOUT = "logout";
    }

}
