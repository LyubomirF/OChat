using System;

namespace OChat.Domain
{

    public enum FriendRequestStatus
    {
        Pending,
        Accepted,
        Ignored
    }

    public class FriendRequest
    {
        public Guid Id { get; set; }

        public FriendRequestStatus Status { get; set; }

        public User From { get; set; }
    }
}