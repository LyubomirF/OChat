using System;
using System.Linq;
using System.Threading.Tasks;
using OChatApp.Repositories.Exceptions;
using OChat;
using Microsoft.EntityFrameworkCore;
using OChatApp.Data;
using OChatApp.Repositories.Interfaces;

namespace OChatApp.Repositories
{
    using static ExceptionMessages;

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(OChatAppContext dbContext)
            : base(dbContext) { }

        public Task<User> GetEntityByIdAsync(Guid userId)
            => GetEntityByIdAsync(userId, USER_NOT_FOUND);

        public async Task<User> GetUserWithFriendsAsync(Guid userId)
        {
            var user = await _dbContext.DomainUsers
                           .Include(u => u.Friends)
                           .SingleOrDefaultAsync(u => u.Id == userId);

            return user is null 
                ? throw new NotFoundException(USER_NOT_FOUND) 
                : user;
        }

        public async Task<User> GetUserWithConnectionsAsync(Guid userId)
        {
            var user = await _dbContext.DomainUsers
                           .Include(u => u.Connections)
                           .SingleOrDefaultAsync(u => u.Id == userId);

            return user is null 
                ? throw new NotFoundException(USER_NOT_FOUND) 
                : user;
        }

        public async Task<User> GetUserWithFriendsAndFriendRequestsAsync(Guid userId)
        {
            var user = await _dbContext.DomainUsers
                           .Include(u => u.Friends)
                           .Include(u => u.FriendRequests)
                           .ThenInclude(r => r.From)
                           .SingleOrDefaultAsync(u => u.Id == userId);

            return user is null
                ? throw new NotFoundException(USER_NOT_FOUND)
                : user;
        }

        public async Task<User> GetUserWithPendingRequestsAsync(Guid userId)
        {
            var user = await _dbContext.DomainUsers
                           .Include(u => u.FriendRequests
                               .Where(r => r.Status == FriendRequestStatus.Pending))
                           .ThenInclude(r => r.From)
                           .SingleOrDefaultAsync(u => u.Id == userId);

            return user is null 
                ? throw new NotFoundException(USER_NOT_FOUND) 
                : user;
        }

        public async Task<User> GetUserWithFriendRequestsAsync(Guid userId)
        {
            var user = await _dbContext.DomainUsers
                       .Include(u => u.FriendRequests)
                       .SingleOrDefaultAsync(u => u.Id == userId);

            return user is null 
                ? throw new NotFoundException(USER_NOT_FOUND) 
                : user;
        }

        public async Task<User> GetUserWithChatTrackers(Guid userId)
        {
            var user = await _dbContext.DomainUsers
                .Include(u => u.ChatTrackers)
                .ThenInclude(t => t.Chat)
                .SingleOrDefaultAsync(u => u.Id == userId);

            return user is null
                ? throw new NotFoundException(USER_NOT_FOUND)
                : user;
        }
    }
}
