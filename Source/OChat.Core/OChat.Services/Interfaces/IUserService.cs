using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OChat.Core.OChat.Services.InputModels;
using OChat.Domain;

namespace OChat.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task AcceptFriendRequest(AcceptFriendRequestModel input);

        Task<IEnumerable<FriendRequest>> GetPendingRequests(Guid userId);

        Task<IEnumerable<User>> GetUserFriends(Guid userId);

        Task IgnoreFriendRequest(IgnoreFriendRequestModel input);

        Task RemoveFriend(RemoveFriendModel input);

        Task SendFriendRequest(SendFriendRequestModel input);
    }
}
