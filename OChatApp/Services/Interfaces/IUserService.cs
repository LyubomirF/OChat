using OChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Services.Interfaces
{
    public interface IUserService
    {
        Task AcceptFriendRequest(Guid userId, Guid requestId);

        Task<IEnumerable<FriendRequest>> GetPendingRequests(Guid userId);

        Task<IEnumerable<User>> GetUserFriends(Guid userId);

        Task IgnoreFriendRequest(Guid userId, Guid requestId);

        Task RemoveFriend(Guid userId, Guid targetUserId);

        Task SendFriendRequest(Guid userId, Guid targetUserId);
    }
}
