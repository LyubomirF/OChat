using System;
using System.Threading.Tasks;
using OChat.Domain;

namespace OChat.Core.Common.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(String username);

        //request for user with chats only
        Task<User> GetUserWithFriendsAsync(Guid userId);

        Task<User> GetUserWithFriendsAndFriendRequestsAsync(Guid userId);

        Task<User> GetUserWithFriendRequestsAsync(Guid userId);

        Task<User> GetUserWithConnectionsAsync(Guid userId);

        Task<User> GetUserWithPendingRequestsAsync(Guid userId);

        Task<User> GetUserWithChatTrackers(Guid userId);
    }
}
