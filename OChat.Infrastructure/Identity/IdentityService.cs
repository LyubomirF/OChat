using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OChat.Infrastructure.Identity
{
    class CreateUserOutput
    {
        public IdentityResult Result { get; set; }

        public String UserId { get; set; }
    }


    class IdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
            => _userManager = userManager;

        public async Task<String> GetUserNameAsync(String userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public async Task<CreateUserOutput> CreateUserAsync(String userName, String password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            return new CreateUserOutput() { Result = result, UserId = user.Id };
        }

        public async Task<IdentityResult> DeleteUserAsync(String userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
                return await DeleteUserAsync(user);

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
            => _userManager.DeleteAsync(user);

    }
}
