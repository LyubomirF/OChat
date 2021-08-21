using OChatApp.Areas.Identity.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OChatApp.Repositories
{
    public interface IUserRepository : IRepository<OChatAppUser>
    {
        //request for user with chats only
        Task<OChatAppUser> GetUserWithFriendsAsync(string userId, string exceptionMessage);

        Task<OChatAppUser> GetUserWithChatsAndConnectionsAsync(string userId, string exceptionMessage);

        Task<OChatAppUser> GetUserWithFriendsAndFriendRequestsAsync(string userId, string exceptionMessage);

        Task<OChatAppUser> GetUserWithFriendRequestsAsync(string userId, string exceptionMessage);

        OChatAppUser GetUserWithConnections(string userId, string exceptionMessage);

        Task<OChatAppUser> GetUserWithPendingRequestsAsync(string userId, string exceptionMessage);
    }
}
