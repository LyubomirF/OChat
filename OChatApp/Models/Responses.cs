using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Services
{
    public static class  AuthenticationResponses
    {
        public const string USER_NOT_FOUND = "User not found.";

        public const string SIGN_IN_FAILED = "Sign in failed.";

    }

    public static class ChatResponses
    {
        public const string USER_HAS_NO_CHATS = "User has no chats";
    }

    public static class UserResponses
    {
        public const string USER_HAS_NO_REQUESTS = "User has no friend requests.";
    }

    public class ErrorResponse
    {
        public int Status { get; set; }
        public string Description { get; set; }
    }
}
