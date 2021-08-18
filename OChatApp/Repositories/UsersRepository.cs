using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Repositories
{
    public class UserRepository : Repository<OChatAppUser>, IUserRepository
    {
        public UserRepository(OChatAppContext dbContext)
            : base(dbContext) { }

        public Task<OChatAppUser> GetUserWithChatsAndConnectionsAsync(string userId)
            => _dbContext.Users
                .Include(u => u.Connections)
                .Include(u => u.ChatRooms)
                .ThenInclude(c => c.Users)
                .SingleOrDefaultAsync(u => u.Id == userId);

        public Task<OChatAppUser> GetUserWithFriendsAsync(string userId)
            => _dbContext.Users
                .Include(u => u.Friends)
                .SingleOrDefaultAsync(u => u.Id == userId);

        public OChatAppUser GetUserWithConnections(string userId)
            => _dbContext.Users
                .Include(u => u.Connections)
                .SingleOrDefault(u => u.Id == userId);

        public Task<OChatAppUser> GetUserWithFriendsAndFriendRequestsAsync(string userId)
            => _dbContext.Users
                .Include(u => u.Friends)
                .Include(u => u.FriendRequests)
                .ThenInclude(r => r.FromUser)
                .SingleOrDefaultAsync(u => u.Id == userId);

        public async Task<IEnumerable<FriendRequest>> GetPendingRequestsAsync(string userId)
        {
            var user = await _dbContext.Users
                    .Include(u => u.FriendRequests
                        .Where(r => r.Status == RequestStatus.Pending))
                    .ThenInclude(r => r.FromUser)
                    .SingleOrDefaultAsync(u => u.Id == userId);

            return user.FriendRequests;
        }

        public Task<OChatAppUser> GetUserWithFriendRequestsAsync(string userId)
            => _dbContext.Users
                .Include(u => u.FriendRequests)
                .SingleOrDefaultAsync(u => u.Id == userId);
    }
}
