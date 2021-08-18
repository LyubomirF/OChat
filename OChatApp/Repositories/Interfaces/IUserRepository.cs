using OChatApp.Areas.Identity.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OChatApp.Repositories
{
    public interface IUserRepository : IRepository<OChatAppUser>
    {
        //request for user with chats only
        Task<OChatAppUser> GetUserWithFriendsAsync(string userId);

        Task<OChatAppUser> GetUserWithChatsAndConnectionsAsync(string userId);

        Task<OChatAppUser> GetUserWithFriendsAndFriendRequestsAsync(string userId);

        Task<OChatAppUser> GetUserWithFriendRequestsAsync(string userId);

        OChatAppUser GetUserWithConnections(string userId);

        Task<IEnumerable<FriendRequest>> GetPendingRequestsAsync(string userId);
    }
}
