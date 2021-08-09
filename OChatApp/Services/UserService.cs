using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OChatApp.Services.Exceptions;

namespace OChatApp.Services
{
    public class UserService
    {
        private readonly OChatAppContext _dbContext;

        public UserService(OChatAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OChatAppUser>> GetUserFriends(string userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Friends)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            if (user.Friends == null || user.Friends.Count == 0)
                return null;

            return user.Friends;
        }

        public async Task SendFriendRequest(string userId, string targetUserId)
        {
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var targetUser = await _dbContext.Users
                .Include(u => u.FriendRequests)
                .SingleOrDefaultAsync(u => u.Id == targetUserId);

            if (targetUser == null)
                throw new NotFoundException("Target user not found.");

            if (targetUser.FriendRequests == null)
                targetUser.FriendRequests = new List<FriendRequest>();

            targetUser.FriendRequests.
                Add(new FriendRequest()
                {
                    Status = RequestStatus.Pending,
                    FromUser = user
                });

            await _dbContext.SaveChangesAsync();
        }

        public async Task AcceptFriendRequest(string userId, string requestId)
        {
            var user = await _dbContext.Users
                    .Include(u => u.FriendRequests)
                    .ThenInclude(r => r.FromUser)
                    .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var request = user.FriendRequests
                .SingleOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new NotFoundException("Request not found.");

            var otherUser = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Id == request.FromUser.Id);

            request.Status = RequestStatus.Accepted;
            user.Friends.Add(request.FromUser);

            otherUser.Friends.Add(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RejectFriendRequest(string userId, string requestId)
        {
            var user = await _dbContext.Users
                .Include(u => u.FriendRequests)
                .ThenInclude(r => r.FromUser)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var request = user.FriendRequests
                .SingleOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new NotFoundException("Request not found.");

            request.Status = RequestStatus.Rejected;

            await _dbContext.SaveChangesAsync();

        }

        public async Task RemoveFriend(string userId, string targetUserId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Friends)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var targetUser = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Id == targetUserId);

            if (targetUser == null)
                throw new NotFoundException("Target user not found.");

            user.Friends.Remove(targetUser);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<FriendRequest>> GetPendingRequests(string userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.FriendRequests)
                .ThenInclude(r => r.FromUser)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var requests = user.FriendRequests
                .Where(r => r.Status == RequestStatus.Pending)
                .ToList();

            if (requests.Count == 0)
                return null;

            return requests;
        }

    }
}
