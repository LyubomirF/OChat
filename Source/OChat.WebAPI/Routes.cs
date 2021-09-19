using System;

namespace OChat.WebAPI
{
    public static class ChatRoutes
    {
        public const String CHATS = "chats";

        public const String HISTORY = "{chatId}/history";

        public const String USER = "{userId}";

        public const String CHAT = "{chatId}";

        public const String TIME_LAST_MESSAGE_WAS_READ = "{chatId}/lastReadMessage/{userId}";

        public const String NEW_MESSAGES = "{userId}/newMessages";
    }

    public static class UsersRoutes
    {
        public const String USERS = "users";

        public const String FRIENDS = "{userId}/friends";

        public const String REQESTS = "{userId}/requests";

        public const String REQUEST = "{userId}/request/{targetUserId}";

        public const String ACCEPT = "{userId}/requests/{requestId}/accept";

        public const String IGNORE = "{userId}/requests/{requestId}/ignore";

        public const String REMOVE = "{userId}/friends/{friendId}/remove";

        public const String PENDING = "{userId}/requests/pending";
    }

    public static class AuthenticationRoutes
    {
        public const String AUTHENTICATION = "auth";

        public const String TOKEN = "token";

        public const String REGISTER = "register";
    }
}
