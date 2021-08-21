using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OChatApp.Repositories.Exceptions;

namespace OChatApp.Repositories
{
    public class UserRepository : Repository<OChatAppUser>, IUserRepository
    {
        public UserRepository(OChatAppContext dbContext)
            : base(dbContext) { }

        public async Task<OChatAppUser> GetUserWithChatsAndConnectionsAsync(string userId, string exceptionMessage)
        {
            var user = await _dbContext.Users
                   .Include(u => u.Connections)
                   .Include(u => u.ChatRooms)
                   .ThenInclude(c => c.Users)
                   .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                throw new NotFoundException(exceptionMessage);

            return user;
        }


        public async Task<OChatAppUser> GetUserWithFriendsAsync(string userId, string exceptionMessage)
        {
            var user = await _dbContext.Users
                           .Include(u => u.Friends)
                           .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                throw new NotFoundException(exceptionMessage);

            return user;
        }

        public OChatAppUser GetUserWithConnections(string userId, string exceptionMessage)
        {
            var user =  _dbContext.Users
                           .Include(u => u.Connections)
                           .SingleOrDefault(u => u.Id == userId);

            if (user is null)
                throw new NotFoundException(exceptionMessage);

            return user;
        }

        public async Task<OChatAppUser> GetUserWithFriendsAndFriendRequestsAsync(string userId, string exceptionMessage)
        {
            var user = await _dbContext.Users
                           .Include(u => u.Friends)
                           .Include(u => u.FriendRequests)
                           .ThenInclude(r => r.FromUser)
                           .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                throw new NotFoundException(exceptionMessage);

            return user;
        }

        public async Task<OChatAppUser> GetUserWithPendingRequestsAsync(string userId, string exceptionMessage)
        {
            var user = await _dbContext.Users
                           .Include(u => u.FriendRequests
                               .Where(r => r.Status == RequestStatus.Pending))
                           .ThenInclude(r => r.FromUser)
                           .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                throw new NotFoundException(exceptionMessage);

            return user;
        }

        public async Task<OChatAppUser> GetUserWithFriendRequestsAsync(string userId, string exceptionMessage)
        {
            var user = await _dbContext.Users
                       .Include(u => u.FriendRequests)
                       .SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                throw new NotFoundException(exceptionMessage);

            return user;
        }
    }
}
