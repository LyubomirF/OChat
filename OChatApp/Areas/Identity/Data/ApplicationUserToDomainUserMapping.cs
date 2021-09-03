using OChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChatApp.Areas.Identity.Data
{
    public class ApplicationUserToDomainUserMapping
    {
        public string ApplicationUserID { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Guid DomainUserId { get; set; }

        public User DomainUser { get; set; }
    }
}
