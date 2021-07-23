using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OChatApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the OChatAppUser class
    public class OChatAppUser : IdentityUser
    {
        public ICollection<ChatRoom> ChatRooms { get; set; }
    }
}
