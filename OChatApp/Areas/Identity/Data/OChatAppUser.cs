using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OChatApp.Areas.Identity.Data
{
    public class OChatAppUser : IdentityUser
    {
        public ICollection<ChatRoom> ChatRooms { get; set; }

        public ICollection<Connection> Connections { get; set; }

        public ICollection<OChatAppUser> Friends { get; set; }

        public ICollection<FriendRequest> FriendRequests { get; set; }
    }
}
