using System;
using System.Threading.Tasks;
using OChat;
namespace OChatApp.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        //request for user with chats only
        Task<User> GetUserWithFriendsAsync(Guid userId);

        Task<User> GetUserWithFriendsAndFriendRequestsAsync(Guid userId);

        Task<User> GetUserWithFriendRequestsAsync(Guid userId);

        Task<User> GetUserWithConnectionsAsync(Guid userId);

        Task<User> GetUserWithPendingRequestsAsync(Guid userId);

        Task<User> GetUserWithChatTrackers(Guid userId);
    }
}
